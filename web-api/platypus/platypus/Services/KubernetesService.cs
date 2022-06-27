using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Infrastructure.Options;
using Nssol.Platypus.Infrastructure.Types;
using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.Models;
using Nssol.Platypus.ServiceModels.ClusterManagementModels;
using Nssol.Platypus.ServiceModels.KubernetesModels;
using Nssol.Platypus.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Nssol.Platypus.Services
{
    /// <summary>
    /// k8sとのREST通信を行うための管理クラス。
    /// 
    /// k8sへのPOSTリクエストはyaml形式で行う。
    /// アプリの中でyamlを全て組み立てると保守性・可読性が下がるため、yamlをテンプレートファイル化しておき、RazorLightを使ってリクエストボディを作る。
    /// </summary>
    /// <remarks>
    /// 基本方針として、k8sとの通信でエラーが発生した際、そのエラーメッセージはこのクラス内でログ出力し、呼び出し元には渡さない。
    /// k8sのエラーメッセージをユーザに見せることはないため、途中で何かしらの変換がかかるはず。
    /// </remarks>
    public class KubernetesService : PlatypusServiceBase, IClusterManagementService
    {
        private ContainerManageOptions containerOptions;

        /// <summary>
        /// 仕切り線。ログ出力などする際に、複数の問い合わせ結果を区別するために使用する。
        /// </summary>
        private static readonly string Separator = Environment.NewLine + "----" + Environment.NewLine;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="commonDiLogic">DIロジック</param>
        /// <param name="containerOptions">設定情報</param>
        public KubernetesService(
            ICommonDiLogic commonDiLogic,
            IOptions<ContainerManageOptions> containerOptions) : base(commonDiLogic, @"ServiceModels/KubernetesModels/Templates")
        {
            this.containerOptions = containerOptions.Value;
        }


        #region コンテナ管理
        /// <summary>
        /// Kubernetesの指定した名前空間に、新規にコンテナを作成する。
        /// 失敗した場合はnullが返る。
        /// 
        /// コンテナの作成は、以下の四つをステップで行う
        /// 1.ConfigMap作成(スタートアップスクリプトを変更する場合のみ)
        /// 2.Job作成
        /// 3.Service作成
        /// 4.ステータスの確認
        /// </summary>
        public async Task<Result<RunContainerOutputModel, string>> RunContainerAsync(RunContainerInputModel inModel, string baseUrl)
        {
            string config = "";
            //1. コンフィグマップを作って、そこにスクリプトの内容を登録する

            // 共通スクリプト
            var commonScriptBody = await CreateCommonScriptConfigMapBodyAsync(inModel.Name, inModel.TenantName, inModel.KqiToken, inModel.ScriptType);
            var createCommonScriptConfigMapResult = await CreateConfigMapAsync(inModel.TenantName, inModel.ClusterManagerToken, commonScriptBody);

            if (createCommonScriptConfigMapResult.IsSuccess == false)
            {
                LogError("ConfigMap作成に失敗: " + createCommonScriptConfigMapResult.Error);
                return Result<RunContainerOutputModel, string>.CreateErrorResult(createCommonScriptConfigMapResult.Error);
            }
            config = createCommonScriptConfigMapResult.Value.Trim() + Separator;

            // 個別スクリプト
            var scriptBody = await CreateScriptConfigMapBodyAsync(inModel.Name, inModel.TenantName, inModel.ScriptType, inModel.EntryPoint);
            var createScriptConfigMapResult = await CreateConfigMapAsync(inModel.TenantName, inModel.ClusterManagerToken, scriptBody);

            if (createScriptConfigMapResult.IsSuccess == false)
            {
                LogError("ConfigMap作成に失敗: " + createScriptConfigMapResult.Error);
                return Result<RunContainerOutputModel, string>.CreateErrorResult(createScriptConfigMapResult.Error);
            }
            config = createScriptConfigMapResult.Value.Trim() + Separator;


            //2.Jobを作る
            RunContainerOutputModel result = new RunContainerOutputModel();
            var createJobResult = await CreateJobAsync(inModel, result);
            if (createJobResult.IsSuccess == false)
            {
                LogError("Job作成に失敗: " + createJobResult.Error);
                //失敗したら、作ったConfigMapを消す
                await DeleteContainerAsync(inModel.Name, inModel.TenantName, inModel.ClusterManagerToken, false, false, true);
                return Result<RunContainerOutputModel, string>.CreateErrorResult(createJobResult.Error);
            }
            config += createJobResult.Value.Trim();

            //3.ポートマッピングが指定されていればサービスを同時に作成する
            bool hasService = inModel.PortMappings != null && inModel.PortMappings.Count() > 0;
            if (hasService)
            {
                //Jobが起動するまで少しラグがあるので、ステータス確認の前にSerivceを作る。成功すればresultの内容が更新される。
                var createServiceResult = await CreateServiceAsync(inModel, result);
                if (createServiceResult.IsSuccess == false)
                {
                    LogError("Service作成に失敗: " + createServiceResult.Error);
                    //作ったJobとConfigMapを消す
                    await DeleteContainerAsync(inModel.Name, inModel.TenantName, inModel.ClusterManagerToken, false, true, true);
                    return Result<RunContainerOutputModel, string>.CreateErrorResult(createServiceResult.Error);
                }
                config += Separator + createServiceResult.Value.Trim();
            }

            //4.ステータスを確認する。
            //これはReadなので、configには含めない
            var containerInfo = await GetPodForJobWithRetryAsync(inModel.Name, inModel.TenantName, inModel.ClusterManagerToken);
            if (containerInfo.Status.Succeed() == false)
            {
                //必要なリソースは作れたが、明らかな異常状態なので、作ったServiceとJobとConfigMapを消す
                await DeleteContainerAsync(inModel.Name, inModel.TenantName, inModel.ClusterManagerToken, hasService, true, true);
                return Result<RunContainerOutputModel, string>.CreateErrorResult("Can not access to created container. Status: " + containerInfo.Status.Name);
            }
            result.Host = containerInfo.Host; //まだPodが立っていなければnullになる
            result.Status = containerInfo.Status;
            result.Configuration = config;
            return Result<RunContainerOutputModel, string>.CreateResult(result);
        }

        /// <summary>
        /// コンテナを削除する。
        /// 対象コンテナが存在しない場合はエラーになる。
        /// </summary>
        public async Task<bool> DeleteContainerAsync(ContainerType type, string containerName, string tenantName, string token, string )
        {
            //ServiceとConfigMapはない可能性があるので、要否を確認
            bool hasService = false;
            bool hasConfigMap = false;
            switch (type)
            {
                case ContainerType.Training:
                    hasService = true;
                    hasConfigMap = true;
                    break;
                case ContainerType.Inferencing:
                    hasService = true;
                    hasConfigMap = true;
                    break;
                case ContainerType.Preprocessing:
                    hasService = false;
                    hasConfigMap = true;
                    break;
                case ContainerType.TensorBoard:
                    hasService = true;
                    hasConfigMap = true;
                    break;
                case ContainerType.Notebook:
                    hasService = true;
                    hasConfigMap = true;
                    break;
                case ContainerType.DeleteTenant:
                    hasService = false;
                    hasConfigMap = true;
                    break;
                default:
                    hasConfigMap = await ExistConfigMapAsync(containerName + "-scripts", tenantName, token);
                    hasService = (await GetServiceAsync(tenantName, containerName, token)).IsSuccess;
                    break;
            }

            return await DeleteContainerAsync(containerName, tenantName, token, hasService, true, hasConfigMap);
        }

        /// <summary>
        /// コンテナを構成する各パーツを削除する。
        /// 対象コンテナが存在しない場合はエラーになる。
        /// </summary>
        /// <param name="containerName">コンテナ名</param>
        /// <param name="tenantName">テナント名</param>
        /// <param name="token">削除要求をしたユーザの認証トークン</param>
        /// <param name="deleteService">Serviceを消すか</param>
        /// <param name="deleteJob">Jobを消すか</param>
        /// <param name="deleteConfigMap">ConfigMapを消すか</param>
        /// <returns>成否</returns>
        private async Task<bool> DeleteContainerAsync(string containerName, string tenantName, string token, bool deleteService, bool deleteJob, bool deleteConfigMap)
        {
            bool success = true;
            if (deleteService)
            {
                //Serviceを消す
                var deleteServiceResult = await DeleteServiceAsync(containerName, tenantName, token);
                if (deleteServiceResult.IsSuccess == false)
                {
                    LogError("Service削除に失敗: " + deleteServiceResult.Error);
                    success = false;
                    //Service削除に失敗しても、とりあえずJob削除にトライする
                }
            }

            if (deleteJob)
            {
                //作ったJobを消す
                var deleteJobResult = await DeleteJobAsync(containerName, tenantName, token);
                if (deleteJobResult.IsSuccess == false)
                {
                    LogError("Job削除に失敗: " + deleteJobResult.Error);
                    success = false;
                    //Job削除に失敗しても、とりあえずConfigMap削除にトライする
                }
            }

            if (deleteConfigMap)
            {
                //ConfigMapを消す
                var delteConfigMapResult = await DeleteConfigMapAsync(containerName + "-common-scripts", tenantName, token);
                if (delteConfigMapResult.IsSuccess == false)
                {
                    LogError("ConfigMap削除に失敗: " + delteConfigMapResult.Error);
                    success = false;
                }
                delteConfigMapResult = await DeleteConfigMapAsync(containerName + "-scripts", tenantName, token);
                if (delteConfigMapResult.IsSuccess == false)
                {
                    LogError("ConfigMap削除に失敗: " + delteConfigMapResult.Error);
                    success = false;
                }
            }

            return success;
        }

        /// <summary>
        /// 指定したコンテナのログを取得する
        /// </summary>
        public async Task<Result<Stream, ContainerStatus>> DownloadLogAsync(string containerName, string tenantName, string token)
        {
            //Job名ではなくPod名が分からないとログを取得できないので、まずはJobのステータス情報を取得する
            var status = await GetPodForJobAsync(containerName, tenantName, token);
            if (status.IsSuccess == false)
            {
                //ステータスが稼働状態ではないので、現在のステータスを返す。
                return Result<Stream, ContainerStatus>.CreateErrorResult(status.Error);
            }

            var response = await SendGetRequestAndReturnStreamAsync(new RequestParam()
            {
                BaseUrl = containerOptions.ContainerServiceBaseUrl,
                ApiPath = $"api/v1/namespaces/{tenantName}/pods/{status.Value.Metadata.Name}/log",
                Token = token,
                QueryParams = new Dictionary<string, string>() { { "container", "main" } }
            });
            if (response.IsSuccess)
            {
                return Result<Stream, ContainerStatus>.CreateResult(response.Value);
            }
            else
            {
                return Result<Stream, ContainerStatus>.CreateErrorResult(ContainerStatus.Failed);
            }
        }

        /// <summary>
        /// 指定したテナントのイベントを取得する。
        /// 失敗した場合はnullが返る。
        /// </summary>
        public async Task<Result<IEnumerable<ContainerEventInfo>, ContainerStatus>> GetEventsAsync(Tenant tenant, string token)
        {
            var response = await SendGetRequestAsync(new RequestParam()
            {
                BaseUrl = containerOptions.ContainerServiceBaseUrl,
                ApiPath = $"api/v1/namespaces/{tenant.Name}/events",
                Token = token
            });

            // 結果の返却
            if (response.IsSuccess)
            {
                var result = ConvertResult<GetEventsOutputModel>(response);

                var events = result.Items.Where(i => i.InvolvedObject.Kind == "Pod" || i.InvolvedObject.Kind == "Job").Select(i => new ContainerEventInfo()
                {
                    TenantId = tenant.Id,
                    TenantName = i.Metadata.Namespace,
                    ContainerName = i.InvolvedObject.ContainerName,
                    Message = i.Message,
                    Details = i.Reason,
                    IsError = i.Type != "Normal",
                    FirstTimestamp = i.FirstTimestamp != null ? DateTime.Parse(i.FirstTimestamp).ToLocalFormatedString() : null,
                    LastTimestamp = i.LastTimestamp != null ? DateTime.Parse(i.LastTimestamp).ToLocalFormatedString() : null
                });
                return Result<IEnumerable<ContainerEventInfo>, ContainerStatus>.CreateResult(events);
            }
            else
            {
                LogError("イベント取得に失敗: " + response.Error);
                return Result<IEnumerable<ContainerEventInfo>, ContainerStatus>.CreateErrorResult(ContainerStatus.Failed);
            }
        }

        #region Job管理
        /// <summary>
        /// Jobを新規に作成する。
        /// 作成した際のレスポンスだけだとステータスその他の情報が取れないので、それは別途リクエストを送って収集する。
        /// 結果は引数の<paramref name="outModel"/>に格納する。
        /// 成功した場合、リクエストの内容を返す。
        /// </summary>
        private async Task<Result<string, string>> CreateJobAsync(RunContainerInputModel inModel, RunContainerOutputModel outModel)
        {
            var containerSharedPath = inModel.ContainerSharedPath ?? new Dictionary<string, string>();
            //リクエストボディの作成
            // prepare, main, finishの各コンテナを作成。
            // mainでユーザーの指定コンテナで指定コマンドを実行する
            // prepare, finishはCLIのコンテナでmainの前後に処理を行う
            // prepare, finishが不要な場合はconfigMapのスクリプトで何も実行しない
            string prepareContainer = await RenderEngine.CompileRenderAsync("create_container.yaml", new
            {
                Name = "prepare",
                ContainerImage = inModel.KqiImage,
                Cpu = inModel.Cpu,
                Memory = inModel.Memory + "G",
                Gpu = 0,
                PortMappings = inModel.PortMappings,
                NfsVolumeMounts = inModel.NfsVolumeMounts,
                ContainerSharedPath = containerSharedPath,
                ScriptType = inModel.ScriptType,
                EnvList = inModel.PrepareAndFinishContainerEnvList,
                LogPath = inModel.LogPath
            });
            string mainContainer = await RenderEngine.CompileRenderAsync("create_container.yaml", new
            {
                Name = "main",
                ContainerImage = inModel.ContainerImage,
                Cpu = inModel.Cpu,
                Memory = inModel.Memory + "G",
                Gpu = inModel.Gpu,
                PortMappings = inModel.PortMappings,
                NfsVolumeMounts = inModel.NfsVolumeMounts,
                ContainerSharedPath = containerSharedPath,
                ScriptType = inModel.ScriptType,
                EnvList = inModel.MainContainerEnvList,
                LogPath = inModel.LogPath
            });
            string finishContainer = await RenderEngine.CompileRenderAsync("create_container.yaml", new
            {
                Name = "finish",
                ContainerImage = inModel.KqiImage,
                Cpu = "0.2",
                Memory = "200M",
                Gpu = 0,
                PortMappings = inModel.PortMappings,
                NfsVolumeMounts = inModel.NfsVolumeMounts,
                ContainerSharedPath = containerSharedPath,
                ScriptType = inModel.ScriptType,
                EnvList = inModel.PrepareAndFinishContainerEnvList,
                LogPath = inModel.LogPath
            });

            string body = await RenderEngine.CompileRenderAsync("create_job.yaml", new
            {
                LoginUser = inModel.LoginUser,
                NameSpace = inModel.TenantName,
                Name = inModel.Name,
                InitContainers = new List<string> { prepareContainer },
                Containers = new List<string> { mainContainer, finishContainer },
                ConstraintList = inModel.ConstraintList,
                PortMappings = inModel.PortMappings,

                RepositoryTokenName = inModel.RegistryTokenName,
                ScriptType = inModel.ScriptType,
                Cmd = inModel.Cmd,
                NfsVolumeMounts = inModel.NfsVolumeMounts,
                ContainerSharedPath = containerSharedPath
            });
            var response = await SendPostRequestAsync(new RequestParam()
            {
                BaseUrl = containerOptions.ContainerServiceBaseUrl,
                ApiPath = $"/apis/batch/v1/namespaces/{inModel.TenantName}/jobs",
                Token = inModel.ClusterManagerToken,
                Body = body,
                MediaType = RequestParam.MediaTypeYaml
            });

            //成否判定
            if (response.IsSuccess)
            {
                var createJobResultModel = base.ConvertResult<CreateJobOutputModel>(response);
                outModel.Name = createJobResultModel.Metadata.Name;
                return Result<string, string>.CreateResult(body);
            }
            else
            {
                LogError(body);
                return Result<string, string>.CreateErrorResult(response.Error);
            }
        }

        /// <summary>
        /// Jobを削除する。
        /// 成功した場合、リクエストの内容を返す。
        /// </summary>
        private async Task<Result<string, string>> DeleteJobAsync(string jobName, string tenantName, string token)
        {
            //リクエストボディの作成
            string body = await RenderEngine.CompileRenderAsync<object>("delete_job.json", null);
            var response = await SendDeleteRequestAsync(new RequestParam()
            {
                BaseUrl = containerOptions.ContainerServiceBaseUrl,
                ApiPath = $"/apis/batch/v1/namespaces/{tenantName}/jobs/{jobName}",
                Token = token,
                Body = body,
                MediaType = RequestParam.MediaTypeJson
            });

            //成否判定
            if (response.IsSuccess)
            {
                return Result<string, string>.CreateResult(body);
            }
            else
            {
                return Result<string, string>.CreateErrorResult(response.Error);
            }
        }
        #endregion

        #region Pod管理

        /// <summary>
        /// 全コンテナ情報を取得する
        /// ※ job 以外の実行形態も収集対象にしたいので、最小単位のPod単位でカウントする
        /// </summary>
        public async Task<Result<IEnumerable<ContainerDetailsInfo>, ContainerStatus>> GetAllContainerDetailsInfosAsync(string token, string tenantName = null)
        {
            // API 呼び出し
            var response = await this.SendGetRequestAsync(new RequestParam()
            {
                BaseUrl = containerOptions.ContainerServiceBaseUrl,
                ApiPath = string.IsNullOrEmpty(tenantName) ? "/api/v1/pods" : $"/api/v1/namespaces/{tenantName}/pods",
                Token = token
            });

            // 結果の返却
            if (response.IsSuccess)
            {
                var result = ConvertResult<GetPodsOutputModel>(response);



                // IsIgnoreNamespaceでtrueのものは除外する
                return Result<IEnumerable<ContainerDetailsInfo>, ContainerStatus>.CreateResult(
                    result.Items.Where(item => this.IsIgnoreNamespace(item.Metadata.Namespace) == false)
                    .Select(i => ConvertModel(i)));
            }
            else
            {
                LogError("Jobのステータス確認に失敗: " + response.Error);
                return Result<IEnumerable<ContainerDetailsInfo>, ContainerStatus>.CreateErrorResult(ContainerStatus.Failed);
            }
        }
        /// <summary>
        /// KQI管理から除外すべきnamespaceかどうかの判定。
        /// k8sのシステム管理や、除外指定されているnamespaceか調べる
        /// </summary>
        /// <param name="namespaceName"></param>
        /// <returns></returns>
        private bool IsIgnoreNamespace(string namespaceName)
        {
            if (namespaceName.StartsWith(containerOptions.KubernetesNamespacePrefix, StringComparison.CurrentCulture))
            {
                return true;
            }

            return containerOptions.IgnoreNamespacesList.Contains(namespaceName);

        }

        /// <summary>
        /// 指定したJobのステータスを取得する。
        /// </summary>
        public async Task<ContainerStatus> GetContainerStatusAsync(string containerName, string tenantName, string token)
        {
            //Podのステータス確認
            var result = await GetPodForJobAsync(containerName, tenantName, token);
            if (result.IsSuccess)
            {
                // OOM Killedの場合その内容を返却する
                if (result.Value.Status.isOOMKilled)
                {
                    return ContainerStatus.OOMKilled;
                }
                return new ContainerStatus(result.Value.Status.Phase);
            }
            return result.Error;
        }

        /// <summary>
        /// 指定したJobの詳細情報を取得する。
        /// </summary>
        public async Task<ContainerDetailsInfo> GetContainerDetailsInfoAsync(string jobName, string tenantName, string token)
        {
            //Podのステータス確認
            var result = await GetPodForJobAsync(jobName, tenantName, token);
            if (result.IsSuccess)
            {
                return ConvertModel(result.Value);
            }
            else
            {
                return new ContainerDetailsInfo()
                {
                    Name = jobName,
                    Status = result.Error
                };
            }
        }

        /// <summary>
        /// k8sのノード情報を汎用モデルに変換する
        /// </summary>
        private ContainerDetailsInfo ConvertModel(GetPodsOutputModel.ItemModel item)
        {
            var info = new ContainerDetailsInfo()
            {
                // ユーザが入力した名前はJobNameでAppと一致させているので、基本的にこれを使用するが、
                // KAMONOHASHI管理外で item.Metadata.Labels がnullの場合、Pod名である item.Metadata.Name を使用する。
                Name = item.Metadata.Labels != null ? item.Metadata.Labels.App : item.Metadata.Name,
                TenantName = item.Metadata.Namespace,
                NodeName = item.Spec.NodeName,
                Status = item.Status.isOOMKilled ? ContainerStatus.OOMKilled : new ContainerStatus(item.Status.Phase),
                NodeIpAddress = item.Status.HostIP,
                CreatedBy = item.Spec.ServiceAccountName //サービスアカウント名（＝ランダム文字列）
            };

            info.ConditionNote = item.ConditionNote;

            if (item.Spec.Containers != null && item.Spec.Containers.Count > 0)
            {
                foreach (var container in item.Spec.Containers)
                {
                    info.Image += container.Image;
                    if (container.Resources?.Requests == null)
                    {
                        //KAMONOHASHIで立てたコンテナは全てResources.Requestsが指定されているはずだが、
                        //KAMONOHASHI外から立てたコンテナが存在する場合など、NULLになってしまう時は警告だけ出してスキップ
                        //UI表示は上流で行う
                        LogWarning($"{info.TenantName}/{info.Name}の要求リソースサイズが取得できません");
                    }
                    else
                    {
                        info.Cpu += container.Resources.Requests.CpuNum;
                        info.Memory += container.Resources.Requests.MemoryGB;
                        info.Gpu += container.Resources.Requests.Gpu;
                    }
                }
            }
            if (DateTime.TryParse(item.Status.StartTime, out DateTime d))
            {
                info.CreatedAt = d;
            }

            return info;
        }

        /// <summary>
        /// 指定したJobの情報をエンドポイント付きで取得する。
        /// エンドポイント情報を収集するためにAPI問い合わせを追加で行う。
        /// </summary>
        public async Task<ContainerEndpointInfo> GetContainerEndpointInfoAsync(string containerName, string tenantName, string token)
        {
            var containerInfo = new ContainerEndpointInfo()
            {
                Name = containerName
            };

            //Podのステータス確認
            var podResult = await GetPodForJobAsync(containerName, tenantName, token);
            if (podResult.IsSuccess == false)
            {
                containerInfo.Status = podResult.Error;
                return containerInfo;
            }

            //まだPodが生きている＝実行中

            containerInfo.Node = podResult.Value.Spec.NodeName;
            containerInfo.ConditionNote = podResult.Value.ConditionNote;
            if (DateTime.TryParse(podResult.Value.Status.StartTime, out DateTime d))
            {
                containerInfo.StartedAt = d;
            }

            //Serviceを取得して、エンドポイント情報を揃える
            var serviceResult = await GetServiceAsync(tenantName, containerName, token);
            if (serviceResult.IsSuccess)
            {
                containerInfo.EndPoints = serviceResult.Value.Spec.Ports.Select(p => new EndPointInfo()
                {
                    Key = p.Name,
                    Host = podResult.Value.Status.HostIP,
                    Port = p.NodePort
                });

                // OOM Killedの場合その内容を返却する
                if (podResult.Value.Status.isOOMKilled)
                {
                    containerInfo.Status = ContainerStatus.OOMKilled;
                }
                else
                {
                    containerInfo.Status = new ContainerStatus(podResult.Value.Status.Phase);
                }

                return containerInfo;
            }
            else
            {
                //PodはあるのにServiceがない＝エラー
                containerInfo.Status = ContainerStatus.Failed;
                return containerInfo;
            }
        }

        /// <summary>
        /// 特定のJobに紐づいたPod情報を取得する。
        /// Podが起動直後の場合、情報が取得可能になるまでタイムラグがあるので、失敗時も複数回リトライする。
        /// 成功した場合、PodのホストIPとステータスを返す。
        /// <see cref="ContainerInfo.Port"/> は取得しないので注意。
        /// </summary>
        private async Task<ContainerInfo> GetPodForJobWithRetryAsync(string jobName, string tenantName, string token)
        {
            var containerInfo = new ContainerInfo()
            {
                Name = jobName,
            };
            // 値が取れない場合、1s間隔で10回リトライする
            int retry = 10;
            for (int i = 0; i < retry; i++)
            {
                //Podのステータス確認
                var result = await GetPodForJobAsync(jobName, tenantName, token);

                if (result.IsSuccess)
                {
                    containerInfo.Host = result.Value.Status.HostIP;
                    containerInfo.Status = new ContainerStatus(result.Value.Status.Phase);
                    return containerInfo;
                }
                // 結果の返却
                if (result.Error == ContainerStatus.None || result.Error == ContainerStatus.Empty)
                {
                    //Podが一つも見つからなかったら、ラグの可能性を考慮して、少し待ってからリトライ
                    LogInformation($"No pod is found for job {jobName}@{tenantName}. {i}/{retry}");
                    await Task.Delay(1000);
                }
                else
                {
                    containerInfo.Status = result.Error;
                    return containerInfo;
                }
            }
            LogWarning($"{tenantName} のJob {jobName} に紐づくPodが見つかりません。");
            containerInfo.Status = ContainerStatus.Empty;
            return containerInfo;
        }

        /// <summary>
        /// 特定のJobに紐づいたPod情報を取得する。
        /// </summary>
        /// <remarks>
        /// Podはk8sのコンテナ管理の最小粒度で、JobやDeploymentで起動しても、内部的にはPodが起動する。
        /// KAMONOHASHIではPodが複数含まれるコンテナは使用しないため、仕様的にはJobに複数Podが紐づきうるが、運用上は必ず一つ以下のPodが返る前提。
        /// </remarks>
        private async Task<Result<GetPodsOutputModel.ItemModel, ContainerStatus>> GetPodForJobAsync(string jobName, string tenantName, string token)
        {
            // API 呼び出し
            var response = await this.SendGetRequestAsync(new RequestParam()
            {
                BaseUrl = containerOptions.ContainerServiceBaseUrl,
                ApiPath = $"/api/v1/namespaces/{tenantName}/pods",
                Token = token,
                QueryParams = new Dictionary<string, string>()
                    {
                        {"labelSelector", $"job-name={jobName}"}
                    },
            });

            // 結果の返却
            // Jobがない場合でもsucessで返ってくるので注意
            if (response.IsSuccess)
            {
                var result = ConvertResult<GetPodsOutputModel>(response);

                if (result.Items.Count > 1)
                {
                    // 取得されるPodは一つ以下の前提なので、何か想定外のことが起きている
                    string message = string.Join(",", result.Items.Select(item => $"{item.Status.HostIP}({item.Status.Phase})"));
                    LogError($"{tenantName} のJob {jobName} に複数のPodが紐づいています。: {message}");
                    return Result<GetPodsOutputModel.ItemModel, ContainerStatus>.CreateErrorResult(ContainerStatus.Multiple);
                }
                if (result.Items.Count == 1)
                {
                    //Podが一つ見つかったら、それを返す
                    return Result<GetPodsOutputModel.ItemModel, ContainerStatus>.CreateResult(result.Items[0]);
                }
                else
                {
                    //Podが一つも見つからなかったら、「なし」のステータスでエラー扱い
                    return Result<GetPodsOutputModel.ItemModel, ContainerStatus>.CreateErrorResult(ContainerStatus.None);
                }
            }
            else
            {
                LogError("Jobのステータス確認に失敗: " + response.Error);
                return Result<GetPodsOutputModel.ItemModel, ContainerStatus>.CreateErrorResult(ContainerStatus.Failed);
            }
        }

        /// <summary>
        /// 指定したテナントの appName に対応する Pod 名を取得する
        /// </summary>
        public async Task<Result<string, ContainerStatus>> GetPodNameAsync(string tenantName, string appName, int limit, string token)
        {
            // ログ用の url 文字列
            string url = $"GET /api/v1/namespaces/{tenantName}/pods?labelSelector=app%3D{appName}&limit={limit}";
            try
            {
                // API 呼び出し
                var response = await this.SendGetRequestAsync(new RequestParam()
                {
                    BaseUrl = containerOptions.ContainerServiceBaseUrl,
                    ApiPath = $"/api/v1/namespaces/{tenantName}/pods",
                    Token = token,
                    QueryParams = new Dictionary<string, string>()
                    {
                        {"labelSelector", $"app={appName}"},
                        {"limit", $"{limit}"}
                    },
                });
                if (!response.IsSuccess)
                {
                    LogError($"Pod 名を取得する Kuerbernetes API を実行しましたが失敗しました。url=\"{url}\"");
                    return Result<string, ContainerStatus>.CreateErrorResult(ContainerStatus.Failed);
                }
                var model = ConvertResult<GetPodsOutputModel>(response);
                if (model.Items.Count > 1)
                {
                    LogWarning($"Pod 名を取得する Kuerbernetes API を実行しましたが結果が複数件でした。url=\"{url}\", count={model.Items.Count}");
                    return Result<string, ContainerStatus>.CreateErrorResult(ContainerStatus.Multiple);
                }
                else if (model.Items.Count == 0)
                {
                    LogWarning($"Pod 名を取得する Kuerbernetes API を実行しましたが結果が 0 件でした。url=\"{url}\"");
                    return Result<string, ContainerStatus>.CreateErrorResult(ContainerStatus.None);
                }
                else if (model.Items[0].Metadata == null)
                {
                    LogWarning($"Pod 名を取得する Kuerbernetes API を実行しましたが結果の Metadata が null でした。url=\"{url}\"");
                    return Result<string, ContainerStatus>.CreateErrorResult(ContainerStatus.Empty);
                }
                // Pod 名を含んだ GetPodsOutputModel.MetadataModel を返却
                return Result<string, ContainerStatus>.CreateResult(model.Items[0].Metadata.Name);
            }
            catch (Exception e)
            {
                LogError($"Pod 名を取得する Kuerbernetes API を実行しましたが例外が発生しました。url=\"{url}\", msg=\"{e.Message}\"");
                return Result<string, ContainerStatus>.CreateErrorResult(ContainerStatus.Error);
            }
        }

        /// <summary>
        /// 指定したテナントの Pod 上でコマンドを実行する。
        /// 実行コンテナ名と、コマンド実行終了を確認する最大回数 maxLoopCount やその間隔 intervalMillisec も指定する。
        /// </summary>
        /// <remarks>
        /// API としては async だが、内部ではコマンド実行結果を待つために wait している
        /// </remarks>
        public async Task<bool> ExecBashCommandAsync(string tenantName, string podName, string command, string container, string token, int intervalMillisec, int maxLoopCount)
        {
            // コマンドに stderr=true&stdout=true を付加し、実行結果をハンドリングする
            // bash -c command : おそらく環境変数などを引き継ぎできるので bash -c とするのが良いと思う。
            string apiUri = $"/api/v1/namespaces/{tenantName}/pods/{podName}/exec?command=/bin/bash&command=-c&command={command}&container={container}&stderr=true&stdout=true";
            try
            {
                //
                // ClientWebSocket 経由でコマンドを実行するための環境設定
                //

                // wss プロトコルで接続
                string k8sHost = containerOptions.KubernetesHostName;
                string k8sPort = containerOptions.KubernetesPort;
                string kubernetesUri = "wss://" + k8sHost + ":" + k8sPort;

                // kubernetes のトークン設定
                ClientWebSocket kubernetesWebSocket = new ClientWebSocket();
                kubernetesWebSocket.Options.SetRequestHeader("Authorization", "Bearer " + token);

                // SSL証明書チェック無効化
                ServicePointManager.ServerCertificateValidationCallback = (s, certificate, chain, sslPolicyError) => true;
                // certificate chain errorを回避(中間証明書の認証エラーを回避)
                kubernetesWebSocket.Options.RemoteCertificateValidationCallback = (s, certificate, chain, sslPolicyErrors) => true;

                //
                // コマンド実行
                //

                // Kubernetesとのwebsocketを確立してコマンド実行
                Uri url = new Uri(kubernetesUri + apiUri);
                var webSocket = kubernetesWebSocket.ConnectAsync(url, CancellationToken.None);
                webSocket.Wait();
                LogDebug($"API呼び出し WebSocket {url}");

                // 実行結果のハンドリング
                // メッセージを受信し、ステータスが CloseReceived なら正常終了とする
                // ステータスが Open のままなら sleep(intervalMillisec) をして結果を待つが、 maxLoopCount を超えたら break する
                bool result = true;
                var count = 0;
                while (true)
                {
                    // 受信データ(stdout/stderr 出力)の待ち受け
                    var buff = new ArraySegment<byte>(new byte[2048]);
                    var receive = kubernetesWebSocket.ReceiveAsync(buff, CancellationToken.None);
                    receive.Wait();

                    byte stdtype = buff[0];
                    if (stdtype == 1 || stdtype == 2)
                    {
                        if (stdtype == 2)
                        {
                            // stderr への出力を認識した。
                            // 存在しないコマンドや実行権限の無いコマンドを実行した可能性もある。
                            // 返却値 result には false を設定する。
                            result = false;
                        }
                        var receivedBytes = new List<byte>(buff);
                        receivedBytes.RemoveAt(0);
                        receivedBytes.RemoveAll(b => b == 0);
                        // TODO: 改行コードが存在するので、何らかの文字に置換すること(現状、debugログが改行込みで出力されてしまう)
                        if (receivedBytes.Count > 0)
                        {
                            // データを受信
                            var recvData = Encoding.UTF8.GetString(receivedBytes.ToArray());
                            if (stdtype == 2)
                            {
                                LogError($"stderr への出力データを受信しました。受信データ=\"{recvData}\", url=\"WebSocket {apiUri}\"");
                            }
                            else
                            {
                                LogDebug($"stdout への出力データを受信しました。受信データ=\"{recvData}\", url=\"WebSocket {apiUri}\"");
                            }
                        }
                    }

                    // ステータスのチェック (Connecting or Open 以外なら終了)
                    if (kubernetesWebSocket.State != WebSocketState.Connecting && kubernetesWebSocket.State != WebSocketState.Open)
                    {
                        if (kubernetesWebSocket.State == WebSocketState.CloseReceived)
                        {
                            // データ受信済みなので終了し result を返却
                            // なお、受信データが stderr 出力なら result=false となっている
                            LogDebug($"ステータス CloseReceived を確認したので終了します。 result={result}, url=\"WebSocket {apiUri}\"");
                            return result;
                        }
                        else
                        {
                            // ステータスが CloseReceived 以外でコマンド実行が終了してしまった。コマンドが正常終了していない可能性がある。
                            LogWarning($"指定したテナントの Pod 上でコマンドを実行しましたが予想外のステータスで終了しました。コマンドが正常終了していない可能性があります。status={kubernetesWebSocket.State}, url=\"WebSocket {apiUri}\"");
                            return false;
                        }
                    }

                    // sleep 回数が最大数を超えたので終了
                    if (count >= maxLoopCount)
                    {
                        break;
                    }
                    // しばらく sleep() する
                    count++;
                    LogDebug($"ステータスが {kubernetesWebSocket.State} なので sleep して終了を待ちます。sleep回数=\"{count}\", intervalMillisec={intervalMillisec}, url=\"WebSocket {apiUri}\"");
                    System.Threading.Thread.Sleep(intervalMillisec);
                }
                // コマンドの終了が確認できていないので false を返却
                // TODO: close 処理の正確な実装方法の確認。usingを使うなど。
                await kubernetesWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                LogWarning($"指定したテナントの Pod 上でコマンドを実行しましたがステータスの終了を確認できませんでした。status={kubernetesWebSocket.State}, url=\"WebSocket {apiUri}\"");
                return false;
            }
            catch (Exception e)
            {
                LogError($"指定したテナントの Pod 上でコマンドを実行しましたが例外が発生しました。url=\"WebSocket {apiUri}\", msg=\"{e.Message}\"");
                return false;
            }
        }
        #endregion

        #region Service管理
        /// <summary>
        /// Serviceを新規に作成する。
        /// 結果は引数の<paramref name="outModel"/>に格納する。
        /// 成功した場合、リクエストの内容を返す。
        /// </summary>
        private async Task<Result<string, string>> CreateServiceAsync(RunContainerInputModel inModel, RunContainerOutputModel outModel)
        {
            //リクエストボディの作成
            string body = await RenderEngine.CompileRenderAsync("create_service.yaml", new
            {
                NameSpace = inModel.TenantName,
                Name = inModel.Name,
                PortMappings = inModel.PortMappings
            });
            var response = await SendPostRequestAsync(new RequestParam()
            {
                BaseUrl = containerOptions.ContainerServiceBaseUrl,
                ApiPath = $"/api/v1/namespaces/{inModel.TenantName}/services",
                Token = inModel.ClusterManagerToken,
                Body = body,
                MediaType = RequestParam.MediaTypeYaml
            });

            //成否判定
            if (response.IsSuccess)
            {
                //ポートマップは自動採番のことがある。
                //そのため実際に割り当てられたポートをレスポンスから確認する。
                var createServiceResultModel = base.ConvertResult<CreateServiceOutputModel>(response);
                outModel.PortMappings = createServiceResultModel.Spec.Ports;
                return Result<string, string>.CreateResult(body);
            }
            else
            {
                return Result<string, string>.CreateErrorResult(response.Error);
            }
        }

        /// <summary>
        /// Service情報を取得する。
        /// </summary>
        private async Task<Result<CreateServiceOutputModel, string>> GetServiceAsync(string tenantName, string serviceName, string token)
        {
            var response = await SendGetRequestAsync(new RequestParam()
            {
                BaseUrl = containerOptions.ContainerServiceBaseUrl,
                ApiPath = $"/api/v1/namespaces/{tenantName}/services/{serviceName}",
                Token = token
            });

            //成否判定
            if (response.IsSuccess)
            {
                //結果は作成時と同じフォーマットで返ってくるので、同じモデルを使いまわす。
                var createServiceResultModel = base.ConvertResult<CreateServiceOutputModel>(response);
                return Result<CreateServiceOutputModel, string>.CreateResult(createServiceResultModel);
            }
            else
            {
                LogError("Serviceの取得失敗: " + response.Error);
                return Result<CreateServiceOutputModel, string>.CreateErrorResult(response.Error);
            }
        }

        /// <summary>
        /// Serivceを削除する。
        /// 成功した場合、リクエストの内容を返す。
        /// </summary>
        private async Task<Result<string, string>> DeleteServiceAsync(string serviceName, string tenantName, string token)
        {
            //リクエストボディの作成
            string body = await RenderEngine.CompileRenderAsync<object>("delete_job.json", null);
            var response = await SendDeleteRequestAsync(new RequestParam()
            {
                BaseUrl = containerOptions.ContainerServiceBaseUrl,
                ApiPath = $"/api/v1/namespaces/{tenantName}/services/{serviceName}",
                Token = token,
                Body = body,
                MediaType = RequestParam.MediaTypeJson
            });

            //成否判定
            if (response.IsSuccess)
            {
                return Result<string, string>.CreateResult(body);
            }
            else
            {
                return Result<string, string>.CreateErrorResult(response.Error);
            }
        }
        #endregion

        #region ConfigMap管理
        /// <summary>
        /// ConfigMapが存在するか確認する。
        /// </summary>
        private async Task<bool> ExistConfigMapAsync(string name, string tenantName, string token)
        {
            var exists = (await SendGetRequestAsync(new RequestParam()
            {
                BaseUrl = containerOptions.ContainerServiceBaseUrl,
                ApiPath = $"/api/v1/namespaces/{tenantName}/configmaps/{name}",
                Token = token,
            }));
            return exists.IsSuccess;
        }

        /// <summary>
        /// ConfigMapを新規に作成する。
        /// ConfigMapはコンテナ内のstartup.shを任意のスクリプトに置換することに利用する。
        /// 成功した場合、リクエストの内容を返す。
        /// </summary>
        private async Task<ResponseResult> CreateConfigMapAsync(string tenantName, string token, string body)
        {
            var response = await SendPostRequestAsync(new RequestParam()
            {
                BaseUrl = containerOptions.ContainerServiceBaseUrl,
                ApiPath = $"/api/v1/namespaces/{tenantName}/configmaps",
                Token = token,
                Body = body,
                MediaType = RequestParam.MediaTypeYaml
            });
            return response;
        }

        /// <summary>
        /// 共通スクリプトConfigMapのbodyを生成する
        /// </summary>
        private async Task<string> CreateCommonScriptConfigMapBodyAsync(string name, string tenantName, string kqiToken, string scriptType)
        {
            string body = await RenderEngine.CompileRenderAsync($"ConfigMap/common_scripts.yaml", new
            {
                Name = name + "-common-scripts",
                NameSpace = tenantName,
                Token = kqiToken,
                Server = containerOptions.WebServerUrl,
                ScriptType = scriptType
            });
            return body;
        }

        /// <summary>
        /// コンテナで実行するスクリプトConfigMapのbodyを生成する
        /// </summary>
        private async Task<string> CreateScriptConfigMapBodyAsync(string name, string tenantName, string scriptType, string entryPoint)
        {
            //エントリポイントに改行が入っている場合、ConfigMapはYamlなので、インデントしないとフォーマットが崩れる
            //\nの後ろをインデントしてしまえば、\r\nの場合も対応可能
            // tensorboardの場合はentryPointはnull
            string cmd = entryPoint ?? "";
            if (cmd.Contains("\n", StringComparison.CurrentCulture))
            {
                cmd = cmd.Replace("\n", "\n    ", StringComparison.CurrentCulture);
            }

            string body = await RenderEngine.CompileRenderAsync($"ConfigMap/{scriptType}_scripts.yaml", new
            {
                Name = name + "-scripts",
                NameSpace = tenantName,
                EntryPoint = cmd,
                ScriptType = scriptType
            });
            return body;
        }

        /// <summary>
        /// ConfigMapを削除する。
        /// </summary>
        private async Task<ResponseResult> DeleteConfigMapAsync(string name, string tenantName, string token)
        {
            var response = await SendDeleteRequestAsync(new RequestParam()
            {
                BaseUrl = containerOptions.ContainerServiceBaseUrl,
                ApiPath = $"/api/v1/namespaces/{tenantName}/configmaps/{name}",
                Token = token
            });
            return response;
        }
        #endregion

        #endregion

        #region テナント管理

        /// <summary>
        /// Kubernetesにテナントを登録する。具体的には以下二つを行う。
        /// 1. 名前空間の追加
        /// 2. ロールの追加
        /// </summary>
        public async Task<bool> RegistTenantAsync(string tenantName)
        {
            //Admin権限で実行するため、共通トークンを使用する
            string token = containerOptions.ResourceManageKey;

            //名前空間を作成する
            if (await CreateNameSpaceAsync(tenantName, token) == false)
            {
                return false;
            }

            //ロールを作成する
            return await CreateRoleAsync(tenantName, token);
        }

        /// <summary>
        /// 名前空間を作成する。すでにある場合は何もしない。
        /// </summary>
        private async Task<bool> CreateNameSpaceAsync(string tenantName, string token)
        {
            //既に名前空間があるか確認
            var exists = (await SendGetRequestAsync(new RequestParam()
            {
                BaseUrl = containerOptions.ContainerServiceBaseUrl,
                ApiPath = $"/api/v1/namespaces/{tenantName}/status",
                Token = token
            }, false)).IsSuccess;

            if (exists)
            {
                LogInformation($"名前空間 {tenantName} は既に作成済み");
                return true;
            }

            //名前空間を作成
            string body = await RenderEngine.CompileRenderAsync("create_namespace.yaml", new
            {
                TenantName = tenantName
            });
            var param = new RequestParam()
            {
                BaseUrl = containerOptions.ContainerServiceBaseUrl,
                ApiPath = $"/api/v1/namespaces",
                Token = token,
                Body = body,
                MediaType = RequestParam.MediaTypeYaml
            };
            var result = await SendPostRequestAsync(param);
            if (result.IsSuccess)
            {
                LogInformation($"名前空間 {tenantName} を新規作成");
                return true;
            }
            else
            {
                LogError($"名前空間 {tenantName} 作成に失敗: " + result.Error);
                return false;
            }
        }

        /// <summary>
        /// ロールを作成する。すでにある場合は何もしない。
        /// </summary>
        /// <remarks>
        /// 認可はWebアプリケーション側で行うため、k8s側には全権限を持った単一ロール（ロール名は名前空間と同一）のみを作成する。
        /// </remarks>
        private async Task<bool> CreateRoleAsync(string tenantName, string token)
        {
            //既にロールがあるか確認
            var exists = (await SendGetRequestAsync(new RequestParam()
            {
                BaseUrl = containerOptions.ContainerServiceBaseUrl,
                ApiPath = $"/apis/rbac.authorization.k8s.io/v1/namespaces/{tenantName}/roles/{tenantName}",
                Token = token
            }, false)).IsSuccess;

            if (exists)
            {
                LogInformation($"{tenantName} のロールは既に作成済み");
                return true;
            }

            //ロールを作成
            string body = await RenderEngine.CompileRenderAsync("create_role.yaml", new
            {
                TenantName = tenantName
            });
            var param = new RequestParam()
            {
                BaseUrl = containerOptions.ContainerServiceBaseUrl,
                ApiPath = $"/apis/rbac.authorization.k8s.io/v1/namespaces/{tenantName}/roles",
                Token = token,
                Body = body,
                MediaType = RequestParam.MediaTypeYaml
            };
            var result = await SendPostRequestAsync(param);
            if (result.IsSuccess)
            {
                LogInformation($"{tenantName} のロールを新規作成");
                return true;
            }
            else
            {
                LogError($"{tenantName} のロール作成に失敗: " + result.Error);
                return false;
            }
        }

        /// <summary>
        /// Kubernetesの指定した名前空間に、DockerRegistryのトークン（シークレット）を登録する。
        /// これをやっておかないと、k8sが当該レジストリからイメージを取得することができない。
        /// 既に存在する場合は、削除した後に新しいトークンを追加する
        /// </summary>
        public async Task<bool> RegistRegistryTokenyAsync(RegistRegistryTokenInputModel model)
        {
            string secretName = model.RegistryTokenKey;
            if (secretName == null)
            {
                //認証情報が未設定の場合はシークレットが作れないので、失敗扱いにする
                return false;
            }

            //Admin権限で実行するため、共通トークンを使用する
            string token = containerOptions.ResourceManageKey;

            //既にシークレットが存在するか確認
            var exists = (await SendGetRequestAsync(new RequestParam()
            {
                BaseUrl = containerOptions.ContainerServiceBaseUrl,
                ApiPath = $"/api/v1/namespaces/{model.TenantName}/secrets/{secretName}",
                Token = token
            })).IsSuccess;

            if (exists)
            {
                LogInformation($"シークレット{model.TenantName}/{secretName}を再作成");
                //存在した場合は、一回消す
                var deleteSecretResult = await SendDeleteRequestAsync(new RequestParam()
                {
                    BaseUrl = containerOptions.ContainerServiceBaseUrl,
                    ApiPath = $"/api/v1/namespaces/{model.TenantName}/secrets/{secretName}",
                    Token = token
                });
                if (deleteSecretResult.IsSuccess == false)
                {
                    LogError($"シークレット{model.TenantName}/{secretName} 削除に失敗: " + deleteSecretResult.Error);
                    return false;
                }
            }

            //シークレットを作成
            string quote = "\""; //ダブルクオートがあると文字列があまりに読みにくいので、変数化
            string dockerCfg = Convert.ToBase64String(Encoding.UTF8.GetBytes(
                    "{" + quote + model.Url + quote + ":{" + model.DockerCfgAuthString + "}}"
            ));

            string body = await RenderEngine.CompileRenderAsync("create_secret.yaml", new
            {
                Name = secretName,
                NameSpace = model.TenantName,
                Dockercfg = dockerCfg
            });
            var param = new RequestParam()
            {
                BaseUrl = containerOptions.ContainerServiceBaseUrl,
                ApiPath = $"/api/v1/namespaces/{model.TenantName}/secrets",
                Token = token,
                Body = body,
                MediaType = RequestParam.MediaTypeYaml
            };
            var result = await SendPostRequestAsync(param);
            if (result.IsSuccess)
            {
                LogInformation($"シークレット{model.TenantName}/{secretName} を新規作成");
                return true;
            }
            else
            {
                LogError($"シークレット{model.TenantName}/{secretName} 作成に失敗: " + result.Error);
                return false;
            }
        }

        /// <summary>
        /// クラスタ管理サービスよりテナントを抹消(削除)する。
        /// 具体的には Kubernetes よりテナントを抹消(削除)するが、以下のオペレーションを実行する。
        /// 1. ロールの削除
        /// 2. 名前空間の抹消(削除)
        /// </summary>
        public async Task<bool> EraseTenantAsync(string tenantName)
        {
            //Admin権限で実行するため、共通トークンを使用する
            string token = containerOptions.ResourceManageKey;

            // ロールの削除
            if (!await DeleteRoleAsync(tenantName, token))
            {
                return false;
            }

            // 名前空間の抹消(削除)
            return await DeleteNameSpaceAsync(tenantName, token);
        }
        /// <summary>
        /// ロールを削除する。すでに存在しないなら何もしない。
        /// </summary>
        private async Task<bool> DeleteRoleAsync(string tenantName, string token)
        {
            if (!(await ContainsRoleAsync(tenantName, token)))
            {
                LogInformation($"KubernetesService#DeleteRoleAsync(): ロール {tenantName} は既に抹消済み");
                return true;
            }
            LogInformation($"KubernetesService#DeleteRoleAsync(): ロール {tenantName} の削除ロジックが作成中");

            // ロールの削除
            var param = new RequestParam()
            {
                BaseUrl = containerOptions.ContainerServiceBaseUrl,
                ApiPath = $"/apis/rbac.authorization.k8s.io/v1/namespaces/{tenantName}/roles",
                Token = token,
                MediaType = RequestParam.MediaTypeYaml
            };
            ResponseResult result = await SendDeleteRequestAsync(param);
            if (result.IsSuccess)
                LogInformation($"KubernetesService#DeleteRoleAsync(): ロール {tenantName} を削除しました。");
            else
                LogError($"KubernetesService#DeleteRoleAsync(): ロール {tenantName} の削除に失敗しました。");
            return result.IsSuccess;
        }
        /// <summary>
        /// 名前空間を削除する。すでに存在しないなら何もしない。
        /// </summary>
        private async Task<bool> DeleteNameSpaceAsync(string tenantName, string token)
        {
            if (!(await ContainsNameSpaceAsync(tenantName, token)))
            {
                LogInformation($"KubernetesService#DeleteNameSpaceAsync(): 名前空間 {tenantName} は既に抹消済み");
                return true;
            }
            LogInformation($"KubernetesService#DeleteNameSpaceAsync(): 名前空間 {tenantName} の削除ロジックが作成中");

            var param = new RequestParam()
            {
                BaseUrl = containerOptions.ContainerServiceBaseUrl,
                ApiPath = $"/api/v1/namespaces/{tenantName}",
                Token = token,
                MediaType = RequestParam.MediaTypeYaml
            };
            ResponseResult result = await SendDeleteRequestAsync(param);
            if (result.IsSuccess)
                LogInformation($"KubernetesService#DeleteNameSpaceAsync(): 名前空間 {tenantName} を削除しました。");
            else
                LogError($"KubernetesService#DeleteNameSpaceAsync(): 名前空間 {tenantName} の削除に失敗しました。");
            return result.IsSuccess;
        }
        /// <summary>
        /// ロールの存在有無を返却。
        /// </summary>
        private async Task<bool> ContainsRoleAsync(string tenantName, string token)
        {
            bool exists = (await SendGetRequestAsync(new RequestParam()
            {
                BaseUrl = containerOptions.ContainerServiceBaseUrl,
                ApiPath = $"/apis/rbac.authorization.k8s.io/v1/namespaces/{tenantName}/roles/{tenantName}",
                Token = token
            })).IsSuccess;
            return exists;
        }
        /// <summary>
        /// 名前空間の存在有無を返却。
        /// </summary>
        private async Task<bool> ContainsNameSpaceAsync(string tenantName, string token)
        {
            bool exists = (await SendGetRequestAsync(new RequestParam()
            {
                BaseUrl = containerOptions.ContainerServiceBaseUrl,
                ApiPath = $"/api/v1/namespaces/{tenantName}/status",
                Token = token
            })).IsSuccess;
            return exists;
        }
        #endregion

        #region ユーザ管理

        /// <summary>
        /// Kubernetesにユーザを登録し、トークンを取得する。
        /// 既に追加済みの場合、その認証トークンを取得する。
        /// 取得に失敗した場合、nullが返る。
        /// 
        /// 具体的には以下三つを行う。
        /// 1. サービスアカウントの追加
        /// 2. サービスアカウントへのロールバインディング
        /// 3. 認証トークンの作成
        /// </summary>
        /// <remarks>
        /// 名前空間とロールは作成済みの前提。存在確認は行わない。
        /// </remarks>
        public async Task<string> RegistUserAsync(string tenantName, string userName)
        {
            //Admin権限で実行するため、共通トークンを使用する
            string token = containerOptions.ResourceManageKey;

            //サービスアカウント（＝k8s側のユーザアカウント）を作成する
            if (await CreateServiceAccountAsync(tenantName, userName, token) == false)
            {
                return null;
            }

            //ロールバインディングを行う
            if (await BindRoleToServiceAccountAsync(tenantName, userName, token) == false)
            {
                return null;
            }

            //トークンを作成する
            return await GetUserTokenAsync(tenantName, userName, token);
        }

        /// <summary>
        /// サービスアカウントが存在するか確認する。
        /// 存在する場合はその認証トークン（シークレット）を返す。
        /// </summary>
        private async Task<ResponseResult> GetServiceAccountAsync(string tenantName, string userName, string token)
        {
            //既にアカウントがあるか確認
            var exists = (await SendGetRequestAsync(new RequestParam()
            {
                BaseUrl = containerOptions.ContainerServiceBaseUrl,
                ApiPath = $"/api/v1/namespaces/{tenantName}/serviceaccounts/{userName}",
                Token = token
            }));

            return exists;
        }

        /// <summary>
        /// サービスアカウントを作成する。すでにある場合は何もしない。
        /// </summary>
        private async Task<bool> CreateServiceAccountAsync(string tenantName, string userName, string token)
        {
            //既にアカウントがあるか確認
            var exists = await GetServiceAccountAsync(tenantName, userName, token);
            if (exists.IsSuccess)
            {
                LogInformation($"サービスアカウント {userName}@{tenantName} は既に作成済み");
                return true;
            }

            //サービスアカウントを作成
            string body = await RenderEngine.CompileRenderAsync("create_service_account.yaml", new
            {
                TenantName = tenantName,
                UserName = userName
            });
            var param = new RequestParam()
            {
                BaseUrl = containerOptions.ContainerServiceBaseUrl,
                ApiPath = $"/api/v1/namespaces/{tenantName}/serviceaccounts",
                Token = token,
                Body = body,
                MediaType = RequestParam.MediaTypeYaml
            };
            var result = await SendPostRequestAsync(param);
            if (result.IsSuccess)
            {
                LogInformation($"サービスアカウント {userName}@{tenantName} を新規作成");
                return true;
            }
            else
            {
                LogError($"サービスアカウント {userName}@{tenantName} 作成に失敗: " + result.Error);
                return false;
            }
        }

        /// <summary>
        /// 指定したサービスアカウントにロールを紐づける。
        /// </summary>
        private async Task<bool> BindRoleToServiceAccountAsync(string tenantName, string userName, string token)
        {
            //既にバインド済みか確認
            var exists = (await SendGetRequestAsync(new RequestParam()
            {
                BaseUrl = containerOptions.ContainerServiceBaseUrl,
                ApiPath = $"/apis/rbac.authorization.k8s.io/v1/namespaces/{tenantName}/rolebindings/{tenantName}-{userName}",
                Token = token
            })).IsSuccess;

            if (exists)
            {
                LogInformation($"{userName}@{tenantName} は既にロールバインド済み");
                return true;
            }

            //ロールバインディング
            string body = await RenderEngine.CompileRenderAsync("bind_role.yaml", new
            {
                TenantName = tenantName,
                UserName = userName
            });
            var param = new RequestParam()
            {
                BaseUrl = containerOptions.ContainerServiceBaseUrl,
                ApiPath = $"/apis/rbac.authorization.k8s.io/v1/namespaces/{tenantName}/rolebindings",
                Token = token,
                Body = body,
                MediaType = RequestParam.MediaTypeYaml
            };
            var result = await SendPostRequestAsync(param);
            if (result.IsSuccess)
            {
                LogInformation($"{userName}@{tenantName} へ新規にロールをバインド");
                return true;
            }
            else
            {
                LogError($"{userName}@{tenantName} へのロールバインディングに失敗: " + result.Error);
                return false;
            }
        }

        /// <summary>
        /// サービスアカウントに紐づいたトークンを取得する。
        /// 存在しない場合は作成する。
        /// 取得に失敗した場合はnullを返す。
        /// </summary>
        private async Task<string> GetUserTokenAsync(string tenantName, string userName, string token)
        {
            // トークン取得前処理
            string secret = string.Empty;
            // k8s側でトークンの登録はアカウントの改廃とは非同期に行われているようなので
            // 最大５回までポーリングする
            for (int i = 0; i < 5; i++)
            {
                var result = await GetServiceAccountAsync(tenantName, userName, token);

                //そもそもアカウントが作られていない場合はエラー
                if (result.IsSuccess == false)
                {
                    LogError($"{userName}@{tenantName} のサービスアカウント取得に失敗: " + result.Error);
                    return null;
                }

                var serviceAccount = base.ConvertResult<GetServiceAccountOutputModel>(result);
                secret = serviceAccount?.Secrets?.FirstOrDefault()?.Name;
                if (string.IsNullOrEmpty(secret))
                {
                    //アカウントはあるのに、シークレットがない（＝まだ非同期で作っている最中）
                    LogInformation($"{userName}@{tenantName} のシークレット作成を待機中 {i}/5");
                    await Task.Delay(1000);
                }
                else
                {
                    //シークレットが作られているので、ポーリングをやめて実態を取りに行く
                    break;
                }
            }

            // トークン取得
            var response = (await SendGetRequestAsync(new RequestParam()
            {
                BaseUrl = containerOptions.ContainerServiceBaseUrl,
                ApiPath = $"/api/v1/namespaces/{tenantName}/secrets/{secret}",
                Token = token
            }));
            if (response.IsSuccess)
            {
                var result = base.ConvertResult<GetSercretOutputModel>(response).DecodedToken;
                LogInformation($"{userName}@{tenantName} のシークレットを取得");
                return result;
            }
            else
            {
                LogError($"{userName}@{tenantName} のシークレット取得に失敗: " + response.Error);
                return null;
            }
        }
        #endregion

        #region クラスタ管理

        /// <summary>
        /// ノード単位で、指定したキーのラべルリストを取得する。
        /// 当該ラベルがないノードは結果に含まれない。
        /// </summary>
        public async Task<Result<Dictionary<string, string>, string>> GetNodeLabelMapAsync(string labelKey, List<string> registeredNodeNames)
        {
            var nodes = await GetAllNodesAsync(true, registeredNodeNames);
            if (nodes == null)
            {
                return Result<Dictionary<string, string>, string>.CreateErrorResult("Failed to get node list.");
            }

            var labelMap = new Dictionary<string, string>();
            foreach (var node in nodes)
            {
                //ラベルがある場合のみ、結果に追加
                if (node.Metadata.Labels != null && node.Metadata.Labels.ContainsKey(labelKey))
                {
                    labelMap.Add(node.Metadata.Name, node.Metadata.Labels[labelKey]);
                }
            }
            return Result<Dictionary<string, string>, string>.CreateResult(labelMap);
        }

        /// <summary>
        /// 全ノード情報を取得する。
        /// 取得失敗した場合はnullが返る。
        /// </summary>
        public async Task<IEnumerable<NodeInfo>> GetAllNodesAsync(List<string> registeredNodeNames)
        {
            var nodes = await GetAllNodesAsync(true, registeredNodeNames);
            if (nodes == null)
            {
                return null;
            }
            return nodes.Select(n => new NodeInfo()
            {
                Name = n.Metadata.Name,
                Labels = n.Metadata.Labels,
                Memory = n.Status.Capacity.MemoryGB,
                Cpu = n.Status.Capacity.CpuNum,
                Gpu = n.Status.Capacity.Gpu
            });
        }

        /// <summary>
        /// 全ノード情報を取得する。
        /// 取得失敗した場合はnullが返る。
        /// </summary>
        /// <param name="excludeManagementNode">管理ノードを除外するか</param>
        /// <param name="registeredNodeNames">登録しているノード名のリスト</param>
        private async Task<IEnumerable<GetNodeOutputModel.ItemModel>> GetAllNodesAsync(bool excludeManagementNode, List<string> registeredNodeNames)
        {
            var nodeResponse = await SendGetRequestAsync(new RequestParam()
            {
                BaseUrl = containerOptions.ContainerServiceBaseUrl,
                ApiPath = "/api/v1/nodes",
                Token = containerOptions.ResourceManageKey
            });
            if (nodeResponse.IsSuccess == false)
            {
                LogError("Nodeの取得失敗: " + nodeResponse.Error);
                return null;
            }
            var nodes = base.ConvertResult<GetNodeOutputModel>(nodeResponse);
            if (excludeManagementNode)
            {
                // 登録されているノードのみに絞り込み、システム管理用のノードを除外する                
                return nodes.Items.Where(i => registeredNodeNames.Contains(i.Metadata.Name));
            }
            return nodes.Items;
        }

        /// <summary>
        /// 指定されたノート情報を取得する。
        /// 失敗した場合はnullが返る。
        /// </summary>
        private async Task<GetNodeOutputModel.ItemModel> GetNodeAsync(string nodeName)
        {
            var nodeResponse = await SendGetRequestAsync(new RequestParam()
            {
                BaseUrl = containerOptions.ContainerServiceBaseUrl,
                ApiPath = $"/api/v1/nodes/{nodeName}",
                Token = containerOptions.ResourceManageKey
            });
            if (nodeResponse.IsSuccess == false)
            {
                LogError("Nodeの取得失敗: " + nodeResponse.Error);
                return null;
            }
            var node = base.ConvertResult<GetNodeOutputModel.ItemModel>(nodeResponse);
            return node;
        }

        /// <summary>
        /// 指定されたノードラベルを設定する。
        /// 既にラベルが存在すれば上書きし、存在しない場合は作成する。
        /// 空文字列を指定した場合は、削除する。
        /// </summary>
        /// <remarks>
        /// 更新する必要がなかった場合もtrueが返る。
        /// </remarks>
        /// <param name="nodeName">対象ノード名</param>
        /// <param name="label">ラベル名</param>
        /// <param name="value">設定値</param>
        /// <returns>更新結果、更新できた場合、true</returns>
        public async Task<bool> SetNodeLabelAsync(string nodeName, string label, string value)
        {
            string baseUrl = this.containerOptions.ContainerServiceBaseUrl;

            // まず存在チェック
            var node = await GetNodeAsync(nodeName);
            if (node == null)
            {
                return false;
            }
            bool labelExists = node.Metadata.Labels.ContainsKey(label);
            string operation = null;
            if (labelExists)
            {
                if (string.IsNullOrEmpty(value))
                {
                    //ラベルが存在し、設定値が空文字＝削除
                    operation = "remove";
                }
                else if (value == node.Metadata.Labels[label])
                {
                    //ラベルが存在し、設定値が変更後文字列と一致＝何もしなくていい
                    return true;
                }
                else
                {
                    //ラベルが存在し、設定値が空文字でない＝更新
                    operation = "replace";
                }
            }
            else
            {
                if (string.IsNullOrEmpty(value))
                {
                    //ラベルが存在せず、設定値が空文字＝ないものを消そうとしている＝何もしなくていい
                    return true;
                }
                else
                {
                    //ラベルが存在せず、設定値が空文字でない＝新規作成
                    operation = "add";
                }
            }

            //リクエストボディの作成
            string body = await RenderEngine.CompileRenderAsync("set_node_label.json", new
            {
                Operation = operation,
                Label = label,
                Value = value
            });
            var response = await SendPatchRequestAsync(new RequestParam()
            {
                BaseUrl = containerOptions.ContainerServiceBaseUrl,
                ApiPath = $"/api/v1/nodes/{nodeName}",
                Token = containerOptions.ResourceManageKey,
                Body = body
            });

            if (response.IsSuccess == false)
            {
                LogError("Node Labelの更新失敗: " + response.Error);
                return false;
            }
            return true;
        }


        /// <summary>
        /// 指定されたテナントにクォータを設定する。
        /// 既にクォータ設定が存在すれば上書きし、存在しない場合は作成する。
        /// 0が指定された場合、無制限となる。
        /// </summary>
        /// <param name="cpu">CPUコア数</param>
        /// <param name="memory">メモリ容量（GB）</param>
        /// <param name="gpu">GPU数</param>
        /// <param name="tenantName">テナント名</param>
        public async Task<bool> SetQuotaAsync(string tenantName, int cpu, int memory, int gpu)
        {
            string name = $"{tenantName}-compute-resources";

            //リクエストボディを作成
            string body = await RenderEngine.CompileRenderAsync("create_quota.yaml", new
            {
                Name = name,
                Cpu = cpu,
                Memory = memory > 0 ? $"{memory}G" : null,
                Gpu = gpu
            });

            //存在確認(=404が返ってこなければあるとみなす)
            var getResponse = await SendGetRequestAsync(new RequestParam()
            {
                BaseUrl = containerOptions.ContainerServiceBaseUrl,
                ApiPath = $"/api/v1/namespaces/{tenantName}/resourcequotas/{name}",
                Token = containerOptions.ResourceManageKey
            }, false);
            if (getResponse.IsSuccess)
            {
                //既存がある＝更新

                var putResponse = await SendPutRequestAsync(new RequestParam()
                {
                    BaseUrl = containerOptions.ContainerServiceBaseUrl,
                    ApiPath = $"/api/v1/namespaces/{tenantName}/resourcequotas/{name}",
                    Token = containerOptions.ResourceManageKey,
                    Body = body,
                    MediaType = RequestParam.MediaTypeYaml
                });
                if (putResponse.IsSuccess == false)
                {
                    LogError("クォータの更新失敗: " + getResponse.Error);
                    return false;
                }
                return true;
            }
            else
            {
                //既存がない＝新規

                var putResponse = await SendPostRequestAsync(new RequestParam()
                {
                    BaseUrl = containerOptions.ContainerServiceBaseUrl,
                    ApiPath = $"/api/v1/namespaces/{tenantName}/resourcequotas",
                    Token = containerOptions.ResourceManageKey,
                    Body = body,
                    MediaType = RequestParam.MediaTypeYaml
                });
                if (putResponse.IsSuccess == false)
                {
                    LogError("クォータの作成失敗: " + getResponse.Error);
                    return false;
                }
                return true;
            }
        }

        #endregion


        /// <summary>
        /// レスポンスからエラーメッセージを抽出するためのメソッド。
        /// サービスによってメッセージのフォーマットが異なるため、オーバーライドして上書き可能にしている。
        /// </summary>
        protected override string GetErrorMessageFromResponse(string content)
        {
            string msg = string.Empty;
            if (!string.IsNullOrEmpty(content) && content.StartsWith("{", StringComparison.CurrentCulture))
            {
                var json = (JObject)JsonConvert.DeserializeObject(content);
                msg = (string)json?["message"];
            }
            return msg;
        }

        #region WebSocket通信
        /// <summary>
        /// kubernetesとのwebsocket接続を確立
        /// </summary>
        public async Task<Result<ClientWebSocket, ContainerStatus>> ConnectWebSocketAsync(string jobName, string tenantName, string token)
        {
            //// ジョブに対応するポッドが存在する場合のみ、websocketを確立
            var result = await GetPodForJobAsync(jobName, tenantName, token);
            if (result.Error != null)
                return Result<ClientWebSocket, ContainerStatus>.CreateErrorResult(result.Error);

            string podName = result.Value.Metadata.Name;
            string k8sHost = containerOptions.KubernetesHostName;
            string k8sPort = containerOptions.KubernetesPort;
            string kubernetesUri = "wss://" + k8sHost + ":" + k8sPort;
            string apiUri = "/api/v1/namespaces/" + tenantName + "/pods/" + podName + "/exec?command=/bin/bash&stdin=true&stderr=true&stdout=true&tty=true&container=main";

            ClientWebSocket kubernetesWebSocket = new ClientWebSocket();
            kubernetesWebSocket.Options.SetRequestHeader("Authorization", "Bearer " + token);

            // SSL証明書チェック無効化
            ServicePointManager.ServerCertificateValidationCallback = (s, certificate, chain, sslPolicyError) => true;
            // certificate chain errorを回避(中間証明書の認証エラーを回避)
            kubernetesWebSocket.Options.RemoteCertificateValidationCallback = (s, certificate, chain, sslPolicyErrors) => true;

            Uri url = new Uri(kubernetesUri + apiUri);

            LogDebug($"API呼び出し WebSocket {url}");

            // Kubernetesとのwebsocketを確立
            await kubernetesWebSocket.ConnectAsync(url, CancellationToken.None);
            return Result<ClientWebSocket, ContainerStatus>.CreateResult(kubernetesWebSocket);
        }
        #endregion
    }
}
