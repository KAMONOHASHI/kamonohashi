using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Infrastructure.Options;
using Nssol.Platypus.Infrastructure.Types;
using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.Models;
using Nssol.Platypus.Models.TenantModels;
using Nssol.Platypus.ServiceModels.ClusterManagementModels;
using Nssol.Platypus.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Nssol.Platypus.Logic
{
    public class ClusterManagementLogic : PlatypusLogicBase, IClusterManagementLogic
    {
        // for DI
        private readonly IUserRepository userRepository;
        private readonly INodeRepository nodeRepository;
        private readonly ITensorBoardContainerRepository tensorBoardContainerRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ILoginLogic loginLogic;
        private readonly IGitLogic gitLogic;
        private readonly IRegistryLogic registryLogic;
        private readonly IVersionLogic versionLogic;
        private readonly IClusterManagementService clusterManagementService;
        private readonly ContainerManageOptions containerOptions;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ClusterManagementLogic(
            ICommonDiLogic commonDiLogic,
            IUserRepository userRepository,
            INodeRepository nodeRepository,
            ITensorBoardContainerRepository tensorBoardContainerRepository,
            IClusterManagementService clusterManagementService,
            IUnitOfWork unitOfWork,
            ILoginLogic loginLogic,
            IGitLogic gitLogic,
            IRegistryLogic registryLogic,
            IVersionLogic versionLogic,
            IOptions<ContainerManageOptions> containerOptions
            ) : base(commonDiLogic)
        {
            this.tensorBoardContainerRepository = tensorBoardContainerRepository;
            this.userRepository = userRepository;
            this.nodeRepository = nodeRepository;
            this.clusterManagementService = clusterManagementService;
            this.loginLogic = loginLogic;
            this.gitLogic = gitLogic;
            this.registryLogic = registryLogic;
            this.versionLogic = versionLogic;
            this.unitOfWork = unitOfWork;
            this.containerOptions = containerOptions.Value;
        }

        #region コンテナ管理

        /// <summary>
        /// クラスタ管理サービスにアクセスするための認証トークンを取得する
        /// </summary>
        private async Task<string> GetTokenAsync(bool force)
        {
            if (force)
            {
                return containerOptions.ResourceManageKey;
            }
            else
            {
                return await GetUserAccessTokenAsync();
            }
        }

        /// <summary>
        /// 全コンテナの情報を取得する
        /// </summary>
        public async Task<Result<IEnumerable<ContainerDetailsInfo>, ContainerStatus>> GetAllContainerDetailsInfosAsync()
        {
            string token = await GetTokenAsync(true);
            var result = await clusterManagementService.GetAllContainerDetailsInfosAsync(token);
            return result;
        }

        /// <summary>
        /// 特定のテナントに紐づいた全コンテナの情報を取得する
        /// </summary>
        public async Task<Result<IEnumerable<ContainerDetailsInfo>, ContainerStatus>> GetAllContainerDetailsInfosAsync(string tenantName)
        {
            //トークンは管理者ではなくユーザの物を使用する
            string token = await GetTokenAsync(false);
            var result = await clusterManagementService.GetAllContainerDetailsInfosAsync(token, tenantName);
            return result;
        }

        /// <summary>
        /// 指定したコンテナのエンドポイント付きの情報をクラスタ管理サービスに問い合わせる。
        /// </summary>
        public async Task<ContainerEndpointInfo> GetContainerEndpointInfoAsync(string containerName, string tenantName, bool force)
        {
            string token = await GetTokenAsync(force);
            if (token == null)
            {
                //トークンがない場合、エラー状態を返す
                return new ContainerEndpointInfo()
                {
                    Name = containerName,
                    Status = ContainerStatus.Failed
                };
            }

            var result = await clusterManagementService.GetContainerEndpointInfoAsync(containerName, tenantName, token);
            return result;
        }

        /// <summary>
        /// 指定したコンテナの詳細情報をクラスタ管理サービスに問い合わせる。
        /// </summary>
        public async Task<ContainerDetailsInfo> GetContainerDetailsInfoAsync(string containerName, string tenantName, bool force)
        {
            string token = await GetTokenAsync(force);
            if (token == null)
            {
                //トークンがない場合、エラー状態を返す
                return new ContainerDetailsInfo()
                {
                    Name = containerName,
                    Status = ContainerStatus.Failed
                };
            }

            var result = await clusterManagementService.GetContainerDetailsInfoAsync(containerName, tenantName, token);
            return result;
        }

        /// <summary>
        /// 指定したコンテナのステータスをクラスタ管理サービスに問い合わせる。
        /// </summary>
        public async Task<ContainerStatus> GetContainerStatusAsync(string containerName, string tenantName, bool force)
        {
            string token = await GetTokenAsync(force);
            if (token == null)
            {
                //トークンがない場合、エラー状態を返す
                return ContainerStatus.Failed;
            }

            var result = await clusterManagementService.GetContainerStatusAsync(containerName, tenantName, token);
            return result;
        }

        /// <summary>
        /// 指定したコンテナを削除する。
        /// 対象コンテナが存在しない場合はエラーになる。
        /// </summary>
        public async Task<bool> DeleteContainerAsync(ContainerType type, string containerName, string tenantName, bool force)
        {
            string token = await GetTokenAsync(force);
            if (token == null)
            {
                //トークンがない場合、エラー
                return false;
            }

            //コンテナサービスに削除を依頼
            return await clusterManagementService.DeleteContainerAsync(type, containerName, tenantName, token);
        }

        /// <summary>
        /// 指定したコンテナのログを取得する。
        /// 失敗した場合はコンテナのステータスを返す。
        /// </summary>
        public async Task<Result<System.IO.Stream, ContainerStatus>> DownloadLogAsync(string containerName, string tenantName, bool force)
        {
            string token = await GetTokenAsync(force);
            if (token == null)
            {
                //トークンがない場合、エラー
                return Result<System.IO.Stream, ContainerStatus>.CreateErrorResult(ContainerStatus.Forbidden);
            }

            //対象コンテナが稼働中なので、ログを取得する
            var logfileStream = await clusterManagementService.DownloadLogAsync(containerName, tenantName, token);
            return logfileStream;
        }

        /// <summary>
        /// 指定したテナントのイベントを取得する
        /// </summary>
        public async Task<Result<IEnumerable<ContainerEventInfo>, ContainerStatus>> GetEventsAsync(Tenant tenant, bool force)
        {
            string token = await GetTokenAsync(force);
            if (token == null)
            {
                //トークンがない場合、エラー
                return Result<IEnumerable<ContainerEventInfo>, ContainerStatus>.CreateErrorResult(ContainerStatus.Forbidden);
            }

            return await clusterManagementService.GetEventsAsync(tenant, token);
        }

        /// <summary>
        /// 指定したコンテナのイベントを取得する
        /// </summary>
        public async Task<Result<IEnumerable<ContainerEventInfo>, ContainerStatus>> GetEventsAsync(Tenant tenant, string containerName, bool force, bool errorOnly)
        {
            var result = await GetEventsAsync(tenant, force);

            if (result.IsSuccess)
            {
                var events = errorOnly ?
                    result.Value.Where(r => r.ContainerName == containerName && r.IsError) :
                    result.Value.Where(r => r.ContainerName == containerName);

                result = Result<IEnumerable<ContainerEventInfo>, ContainerStatus>.CreateResult(events);
            }
            return result;
        }

        #region 前処理コンテナ管理

        /// <summary>
        /// 新規に前処理用コンテナを作成する。
        /// </summary>
        /// <param name="preprocessHistory">対象の前処理履歴</param>
        /// <returns>作成したコンテナのステータス</returns>
        public async Task<Result<ContainerInfo, string>> RunPreprocessingContainerAsync(PreprocessHistory preprocessHistory)
        {
            string token = await GetUserAccessTokenAsync();
            if (token == null)
            {
                //トークンがない場合、結果はnull
                return Result<ContainerInfo, string>.CreateErrorResult("Access denied. Failed to get token to access the cluster management system.");
            }

            var registryMap = registryLogic.GetCurrentRegistryMap(preprocessHistory.Preprocess.ContainerRegistryId.Value);

            string tags = "-t " + preprocessHistory.Preprocess.Name; //生成されるデータのタグを設定
            foreach (var tag in preprocessHistory.InputData.Tags)
            {
                tags += " -t " + tag;
            }

            // 上書き可の環境変数
            var editableEnvList = new Dictionary<string, string>()
            {
                { "http_proxy", containerOptions.Proxy },
                { "https_proxy", containerOptions.Proxy },
                { "no_proxy", containerOptions.NoProxy },
                { "HTTP_PROXY", containerOptions.Proxy },
                { "HTTPS_PROXY", containerOptions.Proxy },
                { "NO_PROXY", containerOptions.NoProxy },
                { "COLUMNS", containerOptions.ShellColumns }
            };

            // 上書き不可の環境変数
            var notEditableEnvList = new Dictionary<string, string>()
            {
                { "DATA_ID", preprocessHistory.InputDataId.ToString()},
                { "DATA_NAME", preprocessHistory.InputData.Name },
                { "PREPROCESSING_ID", preprocessHistory.PreprocessId.ToString()},
                { "TAGS", tags },
                { "COMMIT_ID", preprocessHistory.Preprocess.RepositoryCommitId},
                { "KQI_SERVER", containerOptions.WebServerUrl },
                { "KQI_TOKEN", loginLogic.GenerateToken().AccessToken },
                { "PYTHONUNBUFFERED", "true" }, // python実行時の標準出力・エラーのバッファリングをなくす
                { "LC_ALL", "C.UTF-8"},  // python実行時のエラー回避
                { "LANG", "C.UTF-8"},  // python実行時のエラー回避
                { "KQI_VERSION", versionLogic.GetVersion() }  // KAMONOHASHIのバージョン情報
            };

            //コンテナを起動するために必要な設定値をインスタンス化
            var inputModel = new RunContainerInputModel()
            {
                ID = preprocessHistory.Id,
                TenantName = TenantName,
                LoginUser = CurrentUserInfo.Alias, //アカウントはエイリアスから指定
                Name = preprocessHistory.Name,
                ContainerImage = registryMap.Registry.GetImagePath(preprocessHistory.Preprocess.ContainerImage, preprocessHistory.Preprocess.ContainerTag),
                ScriptType = "preproc", // 実行スクリプトの指定
                Cpu = preprocessHistory.Cpu.Value,
                Memory = preprocessHistory.Memory.Value,
                Gpu = preprocessHistory.Gpu.Value,
                KqiToken = loginLogic.GenerateToken().AccessToken,
                KqiImage = "kamonohashi/cli:" + versionLogic.GetVersion(),
                LogPath = "/kqi/attach/preproc_stdout_stderr_${PREPROCESSING_ID}_${DATA_ID}.log", // 前処理履歴IDは現状ユーザーに見えないので前処理+データIDをつける
                NfsVolumeMounts = new List<NfsVolumeMountModel>()
                {
                    // 添付ファイルを保存するディレクトリ
                    // 前処理結果ディレクトリを前処理完了時にzip圧縮して添付するために使用
                    new NfsVolumeMountModel()
                    {
                        Name = "nfs-preproc-attach",
                        MountPath = "/kqi/attach",
                        SubPath = preprocessHistory.Id.ToString(),
                        Server = CurrentUserInfo.SelectedTenant.Storage.NfsServer,
                        ServerPath = CurrentUserInfo.SelectedTenant.PreprocContainerAttachedNfsPath,
                        ReadOnly = false
                    }
                },
                ContainerSharedPath = new Dictionary<string, string>()
                {
                    { "tmp", "/kqi/tmp/" },
                    { "input", "/kqi/input/" },
                    { "git", "/kqi/git/" },
                    { "output", "/kqi/output/" }
                },

                PrepareAndFinishContainerEnvList = editableEnvList, // 上書き可の環境変数を設定
                MainContainerEnvList = editableEnvList, // 上書き可の環境変数を設定

                EntryPoint = preprocessHistory.Preprocess.EntryPoint,

                ClusterManagerToken = token,
                RegistryTokenName = registryMap.RegistryTokenKey,
                IsNodePort = true
            };

            // 前処理はGitの未指定も許可するため、その判定
            if (preprocessHistory.Preprocess.RepositoryGitId != null)
            {
                long gitId = preprocessHistory.Preprocess.RepositoryGitId == -1 ?
                    CurrentUserInfo.SelectedTenant.DefaultGitId.Value : preprocessHistory.Preprocess.RepositoryGitId.Value;

                var gitEndpointResult = await gitLogic.GetPullUrlAsync(preprocessHistory.Preprocess.RepositoryGitId.Value, preprocessHistory.Preprocess.RepositoryName, preprocessHistory.Preprocess.RepositoryOwner);

                if (!gitEndpointResult.IsSuccess)
                {
                    return Result<ContainerInfo, string>.CreateErrorResult(gitEndpointResult.Error);
                }

                if (gitEndpointResult.Value == null)
                {
                    //Git情報は必須にしているので、無ければエラー
                    return Result<ContainerInfo, string>.CreateErrorResult("Git credential is not valid.");
                }

                var gitEndpoint = gitEndpointResult.Value;
                notEditableEnvList.Add("MODEL_REPOSITORY", gitEndpoint.FullUrl);
                notEditableEnvList.Add("MODEL_REPOSITORY_URL", gitEndpoint.Url);
                notEditableEnvList.Add("MODEL_REPOSITORY_TOKEN", gitEndpoint.Token);

            }

            // ユーザの任意追加環境変数をマージする
            AddEnvListToInputModel(preprocessHistory.OptionDic, inputModel.MainContainerEnvList);

            // 上書き不可の追加環境変数をマージする
            AddEnvListToInputModel(notEditableEnvList, inputModel.PrepareAndFinishContainerEnvList);
            AddEnvListToInputModel(notEditableEnvList, inputModel.MainContainerEnvList);

            // 使用できるノードを取得する
            var nodes = GetAccessibleNode();
            if (nodes == null || nodes.Count == 0)
            {
                // デプロイ可能なノードがゼロなら、エラー扱い
                return Result<ContainerInfo, string>.CreateErrorResult("Access denied.　There is no node this tenant can use.");
            }
            else
            {
                // 制約に追加
                inputModel.ConstraintList = new Dictionary<string, List<string>>()
                {
                    { TenantName, new List<string> { "true" } } // tenantNameの許可がされているサーバでのみ実行
                };
            }

            if (string.IsNullOrEmpty(preprocessHistory.Partition) == false)
            {
                // パーティション指定があれば追加
                inputModel.ConstraintList.Add(containerOptions.ContainerLabelPartition, new List<string> { preprocessHistory.Partition });
            }

            var outModel = await clusterManagementService.RunContainerAsync(inputModel);
            if (outModel.IsSuccess == false)
            {
                return Result<ContainerInfo, string>.CreateErrorResult(outModel.Error);
            }
            return Result<ContainerInfo, string>.CreateResult(new ContainerInfo()
            {
                Name = outModel.Value.Name,
                Status = outModel.Value.Status,
                Host = outModel.Value.Host,
                Configuration = outModel.Value.Configuration
            });
        }
        #endregion

        #region Trainコンテナ管理

        /// <summary>
        /// 新規に画像認識の訓練用コンテナを作成する。
        /// </summary>
        /// <param name="trainHistory">対象の学習履歴</param>
        /// <param name="scriptType">コンテナ起動時に実行するスクリプトの種類。</param>
        /// <param name="regisryTokenName">レジストリの認証トークン</param>
        /// <param name="gitToken">Gitの認証トークン</param>
        /// <returns>作成したコンテナのステータス</returns>
        public async Task<Result<ContainerInfo, string>> RunTrainContainerAsync(TrainingHistory trainHistory, string scriptType,
            string regisryTokenName, string gitToken)
        {
            string token = await GetUserAccessTokenAsync();
            if (token == null)
            {
                //トークンがない場合、結果はnull
                return Result<ContainerInfo, string>.CreateErrorResult("Access denied. Failed to get token to access the cluster management system.");
            }

            long gitId = trainHistory.ModelGitId == -1 ?
                CurrentUserInfo.SelectedTenant.DefaultGitId.Value : trainHistory.ModelGitId;

            var registryMap = registryLogic.GetCurrentRegistryMap(trainHistory.ContainerRegistryId.Value);
            var gitEndpointResult = await gitLogic.GetPullUrlAsync(gitId, trainHistory.ModelRepository, trainHistory.ModelRepositoryOwner, gitToken);

            if (!gitEndpointResult.IsSuccess)
            {
                return Result<ContainerInfo, string>.CreateErrorResult(gitEndpointResult.Error);
            }

            if (gitEndpointResult.Value == null)
            {
                //Git情報は必須にしているので、無ければエラー
                return Result<ContainerInfo, string>.CreateErrorResult("Git credential is not valid.");
            }

            var gitEndpoint = gitEndpointResult.Value;

            var nodes = GetAccessibleNode();
            if (nodes == null || nodes.Count == 0)
            {
                //デプロイ可能なノードがゼロなら、エラー扱い
                return Result<ContainerInfo, string>.CreateErrorResult("Access denied.　There is no node this tenant can use.");
            }

            // 上書き可の環境変数
            var editableEnvList = new Dictionary<string, string>()
            {
                { "http_proxy", containerOptions.Proxy },
                { "https_proxy", containerOptions.Proxy },
                { "no_proxy", containerOptions.NoProxy },
                { "HTTP_PROXY", containerOptions.Proxy },
                { "HTTPS_PROXY", containerOptions.Proxy },
                { "NO_PROXY", containerOptions.NoProxy },
                { "COLUMNS", containerOptions.ShellColumns }
            };

            // 上書き不可の環境変数
            var notEditableEnvList = new Dictionary<string, string>()
            {
                { "DATASET_ID", trainHistory.DataSetId.ToString()},
                { "TRAINING_ID", trainHistory.Id.ToString()},
                { "PARENT_ID", trainHistory.ParentMaps != null ? string.Join(",", trainHistory.ParentMaps.Select(t => t.ParentId)) : ""},
                { "MODEL_REPOSITORY", gitEndpoint.FullUrl},
                { "MODEL_REPOSITORY_URL", gitEndpoint.Url},
                { "MODEL_REPOSITORY_TOKEN", gitEndpoint.Token},
                { "COMMIT_ID", trainHistory.ModelCommitId},
                { "KQI_SERVER", containerOptions.WebServerUrl },
                { "KQI_TOKEN", loginLogic.GenerateToken().AccessToken },
                { "PYTHONUNBUFFERED", "true" }, // python実行時の標準出力・エラーのバッファリングをなくす
                { "LC_ALL", "C.UTF-8"},  // python実行時のエラー回避
                { "LANG", "C.UTF-8"},  // python実行時のエラー回避
                { "ZIP_FILE_CREATED", trainHistory.Zip.ToString() },  // 結果をzip圧縮するか否か
                { "LOCAL_DATASET", trainHistory.LocalDataSet.ToString() },  // ローカルにデータをコピーするか否か
                { "KQI_VERSION", versionLogic.GetVersion() }  // KAMONOHASHIのバージョン情報
            };

            //コンテナを起動するために必要な設定値をインスタンス化
            var inputModel = new RunContainerInputModel()
            {
                ID = trainHistory.Id,
                TenantName = TenantName,
                LoginUser = CurrentUserInfo.Alias, //アカウントはエイリアスから指定
                Name = trainHistory.Key,
                ContainerImage = registryMap.Registry.GetImagePath(trainHistory.ContainerImage, trainHistory.ContainerTag),
                ScriptType = scriptType,
                Cpu = trainHistory.Cpu,
                Memory = trainHistory.Memory,
                Gpu = trainHistory.Gpu,
                KqiImage = "kamonohashi/cli:" + versionLogic.GetVersion(),
                KqiToken = loginLogic.GenerateToken().AccessToken,
                LogPath = "/kqi/attach/training_stdout_stderr_${TRAINING_ID}.log",
                NfsVolumeMounts = new List<NfsVolumeMountModel>()
                {
                    // 結果保存するディレクトリ
                    new NfsVolumeMountModel()
                    {
                        Name = "nfs-output",
                        MountPath = "/kqi/output",
                        SubPath = trainHistory.Id.ToString(),
                        Server = CurrentUserInfo.SelectedTenant.Storage.NfsServer,
                        ServerPath = CurrentUserInfo.SelectedTenant.TrainingContainerOutputNfsPath,
                        ReadOnly = false
                    },
                    // 添付ファイルを保存するディレクトリ
                    // 学習結果ディレクトリを学習完了時にzip圧縮して添付するために使用
                    new NfsVolumeMountModel()
                    {
                        Name = "nfs-attach",
                        MountPath = "/kqi/attach",
                        SubPath = trainHistory.Id.ToString(),
                        Server = CurrentUserInfo.SelectedTenant.Storage.NfsServer,
                        ServerPath = CurrentUserInfo.SelectedTenant.TrainingContainerAttachedNfsPath,
                        ReadOnly = false
                    },
                    
                    // データをマウントするディレクトリ
                    // テナントのDataディレクトリを/kqi/rawにマウントする
                    new NfsVolumeMountModel()
                    {
                        Name = "nfs-data",
                        MountPath = "/kqi/raw",
                        Server = CurrentUserInfo.SelectedTenant.Storage.NfsServer,
                        ServerPath = CurrentUserInfo.SelectedTenant.DataNfsPath,
                        ReadOnly = true
                    }
                },
                ContainerSharedPath = new Dictionary<string, string>()
                {
                    { "tmp", "/kqi/tmp/" },
                    { "input", "/kqi/input/" },
                    { "git", "/kqi/git/" }
                },

                PrepareAndFinishContainerEnvList = editableEnvList, // 上書き可の環境変数を設定
                MainContainerEnvList = editableEnvList, // 上書き可の環境変数を設定

                EntryPoint = trainHistory.EntryPoint,

                ClusterManagerToken = token,
                RegistryTokenName = regisryTokenName ?? registryMap.RegistryTokenKey,
                IsNodePort = true
            };
            // 親を指定した場合は親の出力結果を/kqi/parentにマウント
            if (trainHistory.ParentMaps != null)
            {
                if (trainHistory.ParentMaps.Count() > 1)
                {
                    // 親が複数の場合、/kqi/parent/{parentId}にマウントする
                    foreach (var parentMap in trainHistory.ParentMaps)
                    {
                        inputModel.NfsVolumeMounts.Add(new NfsVolumeMountModel()
                        {
                            Name = "nfs-parent-" + parentMap.ParentId.ToString(),
                            MountPath = "/kqi/parent/" + parentMap.ParentId.ToString(),
                            SubPath = parentMap.ParentId.ToString(),
                            Server = CurrentUserInfo.SelectedTenant.Storage.NfsServer,
                            ServerPath = CurrentUserInfo.SelectedTenant.TrainingContainerOutputNfsPath,
                            ReadOnly = true
                        });
                    }
                }
                else if (trainHistory.ParentMaps.Count() == 1)
                {
                    // 親が1件のみの場合、過去の再現性を担保するため、/kqi/parent直下にマウントする
                    inputModel.NfsVolumeMounts.Add(new NfsVolumeMountModel()
                    {
                        Name = "nfs-parent",
                        MountPath = "/kqi/parent",
                        SubPath = trainHistory.ParentMaps.First().ParentId.ToString(),
                        Server = CurrentUserInfo.SelectedTenant.Storage.NfsServer,
                        ServerPath = CurrentUserInfo.SelectedTenant.TrainingContainerOutputNfsPath,
                        ReadOnly = true
                    });
                }
            }

            // 開放するポート番号を指定
            if (trainHistory.PortList != null)
            {
                var portMappingList = new List<PortMappingModel>();
                foreach (var port in trainHistory.PortList)
                {
                    var portMappingModel = new PortMappingModel()
                    {
                        Protocol = "TCP",
                        Port = port,
                        TargetPort = port,
                        Name = port.ToString()
                    };
                    portMappingList.Add(portMappingModel);
                }
                inputModel.PortMappings = portMappingList.ToArray();
            }

            // ユーザの任意追加環境変数をマージする
            AddEnvListToInputModel(trainHistory.OptionDic, inputModel.MainContainerEnvList);

            // 上書き不可の追加環境変数をマージする
            AddEnvListToInputModel(notEditableEnvList, inputModel.PrepareAndFinishContainerEnvList);
            AddEnvListToInputModel(notEditableEnvList, inputModel.MainContainerEnvList);

            //使用できるノードを制約に追加
            inputModel.ConstraintList = new Dictionary<string, List<string>>()
            {
                { TenantName, new List<string> { "true" } } // tenantNameの許可がされているサーバでのみ実行
            };

            if (string.IsNullOrEmpty(trainHistory.Partition) == false)
            {
                // パーティション指定があれば追加
                inputModel.ConstraintList.Add(containerOptions.ContainerLabelPartition, new List<string> { trainHistory.Partition });
            }

            var outModel = await clusterManagementService.RunContainerAsync(inputModel);
            if (outModel.IsSuccess == false)
            {
                return Result<ContainerInfo, string>.CreateErrorResult(outModel.Error);
            }
            return Result<ContainerInfo, string>.CreateResult(new ContainerInfo()
            {
                Name = outModel.Value.Name,
                Status = outModel.Value.Status,
                Host = outModel.Value.Host,
                Configuration = outModel.Value.Configuration
            });
        }

        /// <summary>
        /// 新規に画像認識の推論用コンテナを作成する。
        /// </summary>
        /// <param name="inferenceHistory">対象の推論履歴</param>
        /// <returns>作成したコンテナのステータス</returns>
        public async Task<Result<ContainerInfo, string>> RunInferenceContainerAsync(InferenceHistory inferenceHistory)
        {
            string token = await GetUserAccessTokenAsync();
            if (token == null)
            {
                //トークンがない場合、結果はnull
                return Result<ContainerInfo, string>.CreateErrorResult("Access denied. Failed to get token to access the cluster management system.");
            }

            long gitId = inferenceHistory.ModelGitId == -1 ?
                CurrentUserInfo.SelectedTenant.DefaultGitId.Value : inferenceHistory.ModelGitId.Value;

            var registryMap = registryLogic.GetCurrentRegistryMap(inferenceHistory.ContainerRegistryId.Value);
            var gitEndpointResult = await gitLogic.GetPullUrlAsync(gitId, inferenceHistory.ModelRepository, inferenceHistory.ModelRepositoryOwner);

            if (!gitEndpointResult.IsSuccess)
            {
                return Result<ContainerInfo, string>.CreateErrorResult(gitEndpointResult.Error);
            }

            if (gitEndpointResult.Value == null)
            {
                //Git情報は必須にしているので、無ければエラー
                return Result<ContainerInfo, string>.CreateErrorResult("Git credential is not valid.");
            }

            var gitEndpoint = gitEndpointResult.Value;

            var nodes = GetAccessibleNode();
            if (nodes == null || nodes.Count == 0)
            {
                //デプロイ可能なノードがゼロなら、エラー扱い
                return Result<ContainerInfo, string>.CreateErrorResult("Access denied.　There is no node this tenant can use.");
            }

            // 上書き可の環境変数
            var editableEnvList = new Dictionary<string, string>()
            {
                { "http_proxy", containerOptions.Proxy },
                { "https_proxy", containerOptions.Proxy },
                { "no_proxy", containerOptions.NoProxy },
                { "HTTP_PROXY", containerOptions.Proxy },
                { "HTTPS_PROXY", containerOptions.Proxy },
                { "NO_PROXY", containerOptions.NoProxy },
                { "COLUMNS", containerOptions.ShellColumns }
            };

            // 上書き不可の環境変数
            var notEditableEnvList = new Dictionary<string, string>()
            {
                { "DATASET_ID", inferenceHistory.DataSetId.ToString()},
                { "INFERENCE_ID", inferenceHistory.Id.ToString()},
                { "PARENT_ID", inferenceHistory.ParentMaps != null ? string.Join(",", inferenceHistory.ParentMaps.Select(t => t.ParentId)) : ""},
                { "MODEL_REPOSITORY", gitEndpoint.FullUrl},
                { "MODEL_REPOSITORY_URL", gitEndpoint.Url},
                { "MODEL_REPOSITORY_TOKEN", gitEndpoint.Token},
                { "COMMIT_ID", inferenceHistory.ModelCommitId},
                { "KQI_SERVER", containerOptions.WebServerUrl },
                { "KQI_TOKEN", loginLogic.GenerateToken().AccessToken },
                { "PYTHONUNBUFFERED", "true" }, // python実行時の標準出力・エラーのバッファリングをなくす
                { "LC_ALL", "C.UTF-8"},  // python実行時のエラー回避
                { "LANG", "C.UTF-8"},  // python実行時のエラー回避
                { "ZIP_FILE_CREATED", inferenceHistory.Zip.ToString() },  // 結果をzip圧縮するか否か
                { "LOCAL_DATASET", inferenceHistory.LocalDataSet.ToString() },  // ローカルにデータをコピーするか否か
                { "KQI_VERSION", versionLogic.GetVersion() }  // KAMONOHASHIのバージョン情報
            };

            //コンテナを起動するために必要な設定値をインスタンス化
            var inputModel = new RunContainerInputModel()
            {
                ID = inferenceHistory.Id,
                TenantName = TenantName,
                LoginUser = CurrentUserInfo.Alias, //アカウントはエイリアスから指定
                Name = inferenceHistory.Key,
                ContainerImage = registryMap.Registry.GetImagePath(inferenceHistory.ContainerImage, inferenceHistory.ContainerTag),
                ScriptType = "inference",
                Cpu = inferenceHistory.Cpu,
                Memory = inferenceHistory.Memory,
                Gpu = inferenceHistory.Gpu,
                KqiImage = "kamonohashi/cli:" + versionLogic.GetVersion(),
                KqiToken = loginLogic.GenerateToken().AccessToken,
                LogPath = "/kqi/attach/inference_stdout_stderr_${INFERENCE_ID}.log",
                NfsVolumeMounts = new List<NfsVolumeMountModel>()
                {
                    // 結果保存するディレクトリ
                    new NfsVolumeMountModel()
                    {
                        Name = "nfs-output",
                        MountPath = "/kqi/output",
                        SubPath = inferenceHistory.Id.ToString(),
                        Server = CurrentUserInfo.SelectedTenant.Storage.NfsServer,
                        ServerPath = CurrentUserInfo.SelectedTenant.InferenceContainerOutputNfsPath,
                        ReadOnly = false
                    },
                    // 添付ファイルを保存するディレクトリ
                    // 学習結果ディレクトリを学習完了時にzip圧縮して添付するために使用
                    new NfsVolumeMountModel()
                    {
                        Name = "nfs-attach",
                        MountPath = "/kqi/attach",
                        SubPath = inferenceHistory.Id.ToString(),
                        Server = CurrentUserInfo.SelectedTenant.Storage.NfsServer,
                        ServerPath = CurrentUserInfo.SelectedTenant.InferenceContainerAttachedNfsPath,
                        ReadOnly = false
                    },
                    // データをマウントするディレクトリ
                    // テナントのDataディレクトリを/kqi/rawにマウントする
                    new NfsVolumeMountModel()
                    {
                        Name = "nfs-data",
                        MountPath = "/kqi/raw",
                        Server = CurrentUserInfo.SelectedTenant.Storage.NfsServer,
                        ServerPath = CurrentUserInfo.SelectedTenant.DataNfsPath,
                        ReadOnly = true
                    }
                },
                ContainerSharedPath = new Dictionary<string, string>()
                {
                    { "tmp", "/kqi/tmp/" },
                    { "input", "/kqi/input/" },
                    { "git", "/kqi/git/" },
                },

                PrepareAndFinishContainerEnvList = editableEnvList, // 上書き可の環境変数を設定
                MainContainerEnvList = editableEnvList, // 上書き可の環境変数を設定

                EntryPoint = inferenceHistory.EntryPoint,

                ClusterManagerToken = token,
                RegistryTokenName = registryMap.RegistryTokenKey,
                IsNodePort = true
            };
            // 親を指定した場合は親の出力結果を/kqi/parentにマウント
            // 推論ジョブにおける親ジョブは学習ジョブとなるので、SubPathとServerPathの指定に注意
            if (inferenceHistory.ParentMaps != null)
            {
                if (inferenceHistory.ParentMaps.Count() > 1)
                {
                    // 親ジョブが複数の場合、/kqi/parent/{parentId}にマウントする
                    foreach (var parentMap in inferenceHistory.ParentMaps)
                    {
                        inputModel.NfsVolumeMounts.Add(new NfsVolumeMountModel()
                        {
                            Name = "nfs-parent-" + parentMap.ParentId.ToString(),
                            MountPath = "/kqi/parent/" + parentMap.ParentId.ToString(),
                            SubPath = parentMap.ParentId.ToString(),
                            Server = CurrentUserInfo.SelectedTenant.Storage.NfsServer,
                            ServerPath = CurrentUserInfo.SelectedTenant.TrainingContainerOutputNfsPath,
                            ReadOnly = true
                        });
                    }
                }
                else if (inferenceHistory.ParentMaps.Count() == 1)
                {
                    // 親ジョブが1件のみの場合、過去の再現性を担保するため/kqi/parent直下にマウントする
                    inputModel.NfsVolumeMounts.Add(new NfsVolumeMountModel()
                    {
                        Name = "nfs-parent",
                        MountPath = "/kqi/parent",
                        SubPath = inferenceHistory.ParentMaps.First().ParentId.ToString(),
                        Server = CurrentUserInfo.SelectedTenant.Storage.NfsServer,
                        ServerPath = CurrentUserInfo.SelectedTenant.TrainingContainerOutputNfsPath,
                        ReadOnly = true
                    });
                }
            }
            // 親推論を指定した場合は親推論の出力結果を/kqi/inferenceにマウント
            if (inferenceHistory.ParentInferenceMaps != null)
            {
                foreach (var parentInferenceMap in inferenceHistory.ParentInferenceMaps)
                {
                    inputModel.NfsVolumeMounts.Add(new NfsVolumeMountModel()
                    {
                        Name = "nfs-inference-" + parentInferenceMap.ParentId.ToString(),
                        MountPath = "/kqi/inference/" + parentInferenceMap.ParentId.ToString(),
                        SubPath = parentInferenceMap.ParentId.ToString(),
                        Server = CurrentUserInfo.SelectedTenant.Storage.NfsServer,
                        ServerPath = CurrentUserInfo.SelectedTenant.InferenceContainerOutputNfsPath,
                        ReadOnly = true
                    });
                }
            }

            // ユーザの任意追加環境変数をマージする
            AddEnvListToInputModel(inferenceHistory.OptionDic, inputModel.MainContainerEnvList);

            // 上書き不可の追加環境変数をマージする
            AddEnvListToInputModel(notEditableEnvList, inputModel.PrepareAndFinishContainerEnvList);
            AddEnvListToInputModel(notEditableEnvList, inputModel.MainContainerEnvList);

            //使用できるノードを制約に追加
            inputModel.ConstraintList = new Dictionary<string, List<string>>()
            {
                { TenantName, new List<string> { "true" } } // tenantNameの許可がされているサーバでのみ実行
            };

            if (string.IsNullOrEmpty(inferenceHistory.Partition) == false)
            {
                // パーティション指定があれば追加
                inputModel.ConstraintList.Add(containerOptions.ContainerLabelPartition, new List<string> { inferenceHistory.Partition });
            }

            var outModel = await clusterManagementService.RunContainerAsync(inputModel);
            if (outModel.IsSuccess == false)
            {
                return Result<ContainerInfo, string>.CreateErrorResult(outModel.Error);
            }
            return Result<ContainerInfo, string>.CreateResult(new ContainerInfo()
            {
                Name = outModel.Value.Name,
                Status = outModel.Value.Status,
                Host = outModel.Value.Host,
                Configuration = outModel.Value.Configuration
            });
        }
        #endregion

        #region TensorBoardコンテナ管理
        /// <summary>
        /// 新規にTensorBoard表示用のコンテナを作成する。
        /// 成功した場合は作成結果が、失敗した場合はnullが返る。
        /// </summary>
        /// <param name="trainingHistory">対象の学習履歴</param>
        /// <param name="expiresIn">生存期間(秒)</param>
        /// <param name="selectedHistoryIds">追加でマウントする学習履歴ID</param>
        /// <returns>作成したコンテナのステータス</returns>
        public async Task<ContainerInfo> RunTensorBoardContainerAsync(TrainingHistory trainingHistory, int expiresIn, List<long> selectedHistoryIds)
        {
            //コンテナ名は自動生成
            //使用できる文字など、命名規約はコンテナ管理サービス側によるが、
            //ユーザ入力値検証の都合でどうせ決め打ちしないといけないので、ロジック層で作ってしまう
            string tenantId = CurrentUserInfo.SelectedTenant.Id.ToString("0000");
            string containerName = $"tensorboard-{tenantId}-{trainingHistory.Id}-{DateTime.Now.ToString("yyyyMMddHHmmssffffff")}";

            string token = await GetUserAccessTokenAsync();
            if (token == null)
            {
                //トークンがない場合、結果はnull
                return new ContainerInfo() { Status = ContainerStatus.Forbidden };
            }

            var nodes = GetAccessibleNode();
            if (nodes == null || nodes.Count == 0)
            {
                //デプロイ可能なノードがゼロなら、エラー扱い
                return new ContainerInfo() { Status = ContainerStatus.Forbidden };
            }

            // 上書き不可の環境変数
            var notEditableEnvList = new Dictionary<string, string>()
            {
                { "TRAINING_ID", trainingHistory.Id.ToString() },
                { "KQI_SERVER", containerOptions.WebServerUrl },
                { "KQI_TOKEN", loginLogic.GenerateToken().AccessToken },
                { "http_proxy", containerOptions.Proxy },
                { "https_proxy", containerOptions.Proxy },
                { "no_proxy", containerOptions.NoProxy },
                { "HTTP_PROXY", containerOptions.Proxy },
                { "HTTPS_PROXY", containerOptions.Proxy },
                { "NO_PROXY", containerOptions.NoProxy },
                { "PYTHONUNBUFFERED", "true" }, // python実行時の標準出力・エラーのバッファリングをなくす
                { "LC_ALL", "C.UTF-8"}, // python実行時のエラー回避
                { "LANG", "C.UTF-8"},  // python実行時のエラー回避
                { "EXPIRES_IN", expiresIn != 0 ? expiresIn.ToString() : "infinity"}  // コンテナ生存期間
            };

            string entryPoint = "/usr/local/bin/tensorboard --bind_all --logdir /kqi/output";
            List<NfsVolumeMountModel> NfsVolumeMounts = new List<NfsVolumeMountModel>()
            {
                // 結果が保存されているディレクトリ
                new NfsVolumeMountModel()
                {
                    Name = "nfs-output",
                    MountPath = "/kqi/output",
                    SubPath = trainingHistory.Id.ToString(),
                    Server = CurrentUserInfo.SelectedTenant.Storage.NfsServer,
                    ServerPath = CurrentUserInfo.SelectedTenant.TrainingContainerOutputNfsPath
                }
            };

            // 追加でマウントする学習がある場合
            if (selectedHistoryIds != null && selectedHistoryIds.Count() != 0)
            {
                entryPoint = "/usr/local/bin/tensorboard --bind_all --logdir_spec " + trainingHistory.Id + ":/kqi/output/" + trainingHistory.Id;
                NfsVolumeMounts = new List<NfsVolumeMountModel>()
                {
                    new NfsVolumeMountModel()
                    {
                        Name = "nfs-output-" + trainingHistory.Id,
                        MountPath = "/kqi/output/" + trainingHistory.Id,
                        SubPath = trainingHistory.Id.ToString(),
                        Server = CurrentUserInfo.SelectedTenant.Storage.NfsServer,
                        ServerPath = CurrentUserInfo.SelectedTenant.TrainingContainerOutputNfsPath
                    }
                };

                foreach (long id in selectedHistoryIds)
                {
                    entryPoint = entryPoint + "," + id + ":/kqi/output/" + id;
                    NfsVolumeMounts.Add(new NfsVolumeMountModel()
                    {
                        Name = "nfs-output-" + id,
                        MountPath = "/kqi/output/" + id,
                        SubPath = id.ToString(),
                        Server = CurrentUserInfo.SelectedTenant.Storage.NfsServer,
                        ServerPath = CurrentUserInfo.SelectedTenant.TrainingContainerOutputNfsPath
                    });
                };
            }

            //コンテナを起動するために必要な設定値をインスタンス化
            var inputModel = new RunContainerInputModel()
            {
                ID = trainingHistory.Id,
                TenantName = TenantName,
                LoginUser = CurrentUserInfo.Alias, //アカウントはエイリアスから指定
                Name = containerName,
                ContainerImage = "tensorflow/tensorflow:2.3.1",    // tensorboardで利用するイメージはtensorflow/tensorflow:2.3.1で固定
                ScriptType = "tensorboard",
                Cpu = 1,
                Memory = 1, //メモリは1GBで仮決め
                Gpu = 0,
                KqiImage = "kamonohashi/cli:" + versionLogic.GetVersion(),
                KqiToken = loginLogic.GenerateToken().AccessToken,
                NfsVolumeMounts = NfsVolumeMounts,

                PrepareAndFinishContainerEnvList = notEditableEnvList, // 上書き不可の環境変数を設定
                MainContainerEnvList = notEditableEnvList, // 上書き不可の環境変数を設定

                ConstraintList = new Dictionary<string, List<string>>() {
                    { TenantName, new List<string> { "true" } }, // tenantNameの許可がされているサーバでのみ実行
                    { containerOptions.ContainerLabelTensorBoardEnabled, new List<string> { "true" } } // tensorboardの実行が許可されているサーバでのみ実行
                },
                PortMappings = new PortMappingModel[]
                {
                    new PortMappingModel() { Protocol = "TCP", Port = 6006, TargetPort = 6006, Name = "tensorboard" }
                },
                ClusterManagerToken = token,
                IsNodePort = true, //ランダムポート指定。アクセス先ポートが動的に決まるようになる。
                EntryPoint = entryPoint
            };

            var outModel = await clusterManagementService.RunContainerAsync(inputModel);

            if (outModel.IsSuccess == false)
            {
                return new ContainerInfo() { Status = ContainerStatus.Failed };
            }
            var port = outModel.Value.PortMappings.Find(p => p.Name == "tensorboard");
            return new ContainerInfo()
            {
                Name = containerName,
                Status = outModel.Value.Status,
                Host = outModel.Value.Host,
                Port = port.NodePort,
                Configuration = outModel.Value.Configuration
            };
        }

        /// <summary>
        /// 指定したTensorBoardコンテナのステータスをクラスタ管理サービスに問い合わせ、結果でDBを更新する。
        /// </summary>
        /// <remark>
        /// TensorBoardコンテナの場合、以下の理由から、エラーが発生した場合は即DBからも削除してしまう。
        /// ・履歴管理をする必要がない
        /// ・名前に時刻が入っているので、もしコンテナが残っていても次回起動には支障がない。
        /// </remark>
        public async Task<ContainerStatus> SyncContainerStatusAsync(TensorBoardContainer container, bool force)
        {
            ContainerStatus result;
            if (string.IsNullOrEmpty(container.Host))
            {
                //ホストが決まっていない＝リソースに空きがなくて、待っている状態

                var info = await GetContainerEndpointInfoAsync(container.Name, CurrentUserInfo.SelectedTenant.Name, false);
                result = info.Status;
                var endpoint = info.EndPoints?.FirstOrDefault(e => e.Key == "tensorboard");
                if (endpoint != null)
                {
                    //ノードが立ったので、ポート情報を更新する
                    //どんな状態のインスタンスが引数で与えられるかわからないので、改めて取得して更新
                    var nextStatusContainer = await tensorBoardContainerRepository.GetByIdAsync(container.Id);
                    nextStatusContainer.Host = endpoint.Host;
                    nextStatusContainer.PortNo = endpoint.Port;
                    nextStatusContainer.Status = result.Name;
                    tensorBoardContainerRepository.Update(nextStatusContainer);
                    unitOfWork.Commit();

                    return info.Status;
                }
                //まだホストが決まっていない場合は、後段処理を実行（対象コンテナがないかもしれないから）
            }
            else
            {
                result = await GetContainerStatusAsync(container.Name, container.Tenant.Name, force);
            }


            if (result.Exist() == false)
            {
                //コンテナがすでに停止しているので、ログを出した後でDBから対象レコードを削除
                LogInformation($"ステータス {result.Name} のTensorBoardコンテナ {container.Id} {container.Name} を削除します。");
                tensorBoardContainerRepository.Delete(container, force);
                unitOfWork.Commit();
            }
            else
            {
                bool updateResult = tensorBoardContainerRepository.UpdateStatus(container.Id, result.Name, true);
                if (updateResult == false)
                {
                    //削除対象がすでに消えていた場合
                    return ContainerStatus.None;
                }
                unitOfWork.Commit();
            }
            return result;
        }
        #endregion

        #region Notebookコンテナ管理

        /// <summary>
        /// 新規にノートブック用コンテナを作成する。
        /// </summary>
        /// <param name="notebookHistory">対象のノートブック履歴</param>
        /// <returns>作成したコンテナのステータス</returns>
        public async Task<Result<ContainerInfo, string>> RunNotebookContainerAsync(NotebookHistory notebookHistory)
        {
            string token = await GetUserAccessTokenAsync();
            if (token == null)
            {
                //トークンがない場合、結果はnull
                return Result<ContainerInfo, string>.CreateErrorResult("Access denied. Failed to get token to access the cluster management system.");
            }

            var registryMap = new UserTenantRegistryMap();
            if (notebookHistory.ContainerRegistryId.HasValue)
            {
                registryMap = registryLogic.GetCurrentRegistryMap(notebookHistory.ContainerRegistryId.Value);
            }

            var nodes = GetAccessibleNode();
            if (nodes == null || nodes.Count == 0)
            {
                //デプロイ可能なノードがゼロなら、エラー扱い
                return Result<ContainerInfo, string>.CreateErrorResult("Access denied.　There is no node this tenant can use.");
            }

            // 上書き可の環境変数
            var editableEnvList = new Dictionary<string, string>()
            {
                { "http_proxy", containerOptions.Proxy },
                { "https_proxy", containerOptions.Proxy },
                { "no_proxy", containerOptions.NoProxy },
                { "HTTP_PROXY", containerOptions.Proxy },
                { "HTTPS_PROXY", containerOptions.Proxy },
                { "NO_PROXY", containerOptions.NoProxy },
                { "COLUMNS", containerOptions.ShellColumns }
            };

            // 上書き不可の環境変数
            var notEditableEnvList = new Dictionary<string, string>()
            {
                { "NOTEBOOK_ID", notebookHistory.Id.ToString()},
                { "COMMIT_ID", notebookHistory.ModelCommitId},
                { "KQI_SERVER", containerOptions.WebServerUrl },
                { "KQI_TOKEN", loginLogic.GenerateToken().AccessToken },
                { "PYTHONUNBUFFERED", "true" }, // python実行時の標準出力・エラーのバッファリングをなくす
                { "LC_ALL", "C.UTF-8"},  // python実行時のエラー回避
                { "LANG", "C.UTF-8"},  // python実行時のエラー回避
                { "EXPIRES_IN", notebookHistory.ExpiresIn != 0 ? notebookHistory.ExpiresIn.ToString() : "infinity"},  // コンテナ生存期間
                { "LOCAL_DATASET", notebookHistory.LocalDataSet.ToString() },  // ローカルにデータをコピーするか否か
                { "JUPYTERLAB_VERSION", notebookHistory.JupyterLabVersion },
                { "KQI_VERSION", versionLogic.GetVersion() }  // KAMONOHASHIのバージョン情報
            };

            //コンテナを起動するために必要な設定値をインスタンス化
            var inputModel = new RunContainerInputModel()
            {
                ID = notebookHistory.Id,
                TenantName = TenantName,
                LoginUser = CurrentUserInfo.Alias, //アカウントはエイリアスから指定
                Name = notebookHistory.Key,
                ContainerImage = notebookHistory.ContainerRegistryId.HasValue ? registryMap.Registry.GetImagePath(notebookHistory.ContainerImage, notebookHistory.ContainerTag) : $"{notebookHistory.ContainerImage}:{notebookHistory.ContainerTag}",
                ScriptType = "notebook",
                Cpu = notebookHistory.Cpu,
                Memory = notebookHistory.Memory,
                Gpu = notebookHistory.Gpu,
                KqiImage = "kamonohashi/cli:" + versionLogic.GetVersion(),
                KqiToken = loginLogic.GenerateToken().AccessToken,
                LogPath = "/kqi/attach/notebook_stdout_stderr_${NOTEBOOK_ID}.log",
                NfsVolumeMounts = new List<NfsVolumeMountModel>()
                {
                    // 結果保存するディレクトリ
                    new NfsVolumeMountModel()
                    {
                        Name = "nfs-output",
                        MountPath = "/kqi/output",
                        SubPath = notebookHistory.Id.ToString(),
                        Server = CurrentUserInfo.SelectedTenant.Storage.NfsServer,
                        ServerPath = CurrentUserInfo.SelectedTenant.NotebookContainerOutputNfsPath,
                        ReadOnly = false
                    },
                    // 添付ファイルを保存するディレクトリ
                    new NfsVolumeMountModel()
                    {
                        Name = "nfs-attach",
                        MountPath = "/kqi/attach",
                        SubPath = notebookHistory.Id.ToString(),
                        Server = CurrentUserInfo.SelectedTenant.Storage.NfsServer,
                        ServerPath = CurrentUserInfo.SelectedTenant.NotebookContainerAttachedNfsPath,
                        ReadOnly = false
                    },
                    // データをマウントするディレクトリ
                    // テナントのDataディレクトリを/kqi/rawにマウントする
                    new NfsVolumeMountModel()
                    {
                        Name = "nfs-data",
                        MountPath = "/kqi/raw",
                        Server = CurrentUserInfo.SelectedTenant.Storage.NfsServer,
                        ServerPath = CurrentUserInfo.SelectedTenant.DataNfsPath,
                        ReadOnly = true
                    }
                },
                ContainerSharedPath = new Dictionary<string, string>()
                {
                    { "tmp", "/kqi/tmp/" },
                    { "input", "/kqi/input/" },
                    { "git", "/kqi/git/" },
                    { "parent", "/kqi/parent/" },
                    { "inference", "/kqi/inference/" }
                },

                PrepareAndFinishContainerEnvList = editableEnvList, // 上書き可の環境変数を設定
                MainContainerEnvList = editableEnvList, // 上書き可の環境変数を設定


                PortMappings = new PortMappingModel[]
                {
                    new PortMappingModel() { Protocol = "TCP", Port = 8888, TargetPort = 8888, Name = "notebook" },
                },
                ClusterManagerToken = token,
                RegistryTokenName = notebookHistory.ContainerRegistryId.HasValue ? registryMap.RegistryTokenKey : null,
                IsNodePort = true,

                // スクリプトの実行をしない場合ヌルコマンドを挿入
                EntryPoint = string.IsNullOrEmpty(notebookHistory.EntryPoint) ? ":" : notebookHistory.EntryPoint
            };

            // データセットの未指定も許可するため、その判定
            if (notebookHistory.DataSetId != null)
            {
                notEditableEnvList.Add("DATASET_ID", notebookHistory.DataSetId.ToString());
            }
            else
            {
                notEditableEnvList.Add("DATASET_ID", "");
            }

            // 親学習を指定した場合は親学習の出力結果を/kqi/parentにマウント
            if (notebookHistory.ParentTrainingMaps != null)
            {
                foreach (var parentTrainingMap in notebookHistory.ParentTrainingMaps)
                {
                    inputModel.NfsVolumeMounts.Add(new NfsVolumeMountModel()
                    {
                        Name = "nfs-parent-" + parentTrainingMap.ParentId.ToString(),
                        MountPath = "/kqi/parent/" + parentTrainingMap.ParentId.ToString(),
                        SubPath = parentTrainingMap.ParentId.ToString(),
                        Server = CurrentUserInfo.SelectedTenant.Storage.NfsServer,
                        ServerPath = CurrentUserInfo.SelectedTenant.TrainingContainerOutputNfsPath,
                        ReadOnly = true
                    });
                }
            }

            // 親推論を指定した場合は親推論の出力結果を/kqi/inferenceにマウント
            if (notebookHistory.ParentInferenceMaps != null)
            {
                foreach (var parentInferenceMap in notebookHistory.ParentInferenceMaps)
                {
                    inputModel.NfsVolumeMounts.Add(new NfsVolumeMountModel()
                    {
                        Name = "nfs-inference-" + parentInferenceMap.ParentId.ToString(),
                        MountPath = "/kqi/inference/" + parentInferenceMap.ParentId.ToString(),
                        SubPath = parentInferenceMap.ParentId.ToString(),
                        Server = CurrentUserInfo.SelectedTenant.Storage.NfsServer,
                        ServerPath = CurrentUserInfo.SelectedTenant.InferenceContainerOutputNfsPath,
                        ReadOnly = true
                    });
                }
            }

            // Gitの未指定も許可するため、その判定
            if (notebookHistory.ModelGitId != null)
            {
                long gitId = notebookHistory.ModelGitId == -1 ?
                    CurrentUserInfo.SelectedTenant.DefaultGitId.Value : notebookHistory.ModelGitId.Value;

                var gitEndpointResult = await gitLogic.GetPullUrlAsync(gitId, notebookHistory.ModelRepository, notebookHistory.ModelRepositoryOwner);

                if (!gitEndpointResult.IsSuccess)
                {
                    return Result<ContainerInfo, string>.CreateErrorResult(gitEndpointResult.Error);
                }

                if (gitEndpointResult.Value != null)
                {
                    var gitEndpoint = gitEndpointResult.Value;
                    notEditableEnvList.Add("MODEL_REPOSITORY", gitEndpoint.FullUrl);
                    notEditableEnvList.Add("MODEL_REPOSITORY_URL", gitEndpoint.Url);
                    notEditableEnvList.Add("MODEL_REPOSITORY_TOKEN", gitEndpoint.Token);
                }
            }

            if (string.IsNullOrEmpty(notebookHistory.Options) == false)
            {
                // ユーザの任意追加環境変数をマージする
                AddEnvListToInputModel(notebookHistory.GetOptionDic(), inputModel.MainContainerEnvList);
            }

            // 上書き不可の追加環境変数をマージする
            AddEnvListToInputModel(notEditableEnvList, inputModel.PrepareAndFinishContainerEnvList);
            AddEnvListToInputModel(notEditableEnvList, inputModel.MainContainerEnvList);


            //使用できるノードを制約に追加
            inputModel.ConstraintList = new Dictionary<string, List<string>>()
            {
                { TenantName, new List<string> { "true" } }, // tenantNameの許可がされているサーバでのみ実行
                { containerOptions.ContainerLabelNotebookEnabled, new List<string> { "true" } } // notebookの実行が許可されているサーバでのみ実行
            };

            if (string.IsNullOrEmpty(notebookHistory.Partition) == false)
            {
                // パーティション指定があれば追加
                inputModel.ConstraintList.Add(containerOptions.ContainerLabelPartition, new List<string> { notebookHistory.Partition });
            }

            var outModel = await clusterManagementService.RunContainerAsync(inputModel);
            if (outModel.IsSuccess == false)
            {
                return Result<ContainerInfo, string>.CreateErrorResult(outModel.Error);
            }
            var port = outModel.Value.PortMappings.Find(p => p.Name == "notebook");
            return Result<ContainerInfo, string>.CreateResult(new ContainerInfo()
            {
                Name = outModel.Value.Name,
                Status = outModel.Value.Status,
                Host = outModel.Value.Host,
                Port = port.NodePort,
                Configuration = outModel.Value.Configuration
            });
        }
        #endregion

        #region テナントデータ削除用コンテナ管理
        /// <summary>
        /// 新規にバケット(テナントデータ)削除用のコンテナを作成する。
        /// </summary>
        /// <param name="tenant">対象のテナント</param>
        /// <returns>作成したコンテナのステータス</returns>
        public async Task<ContainerInfo> RunDeletingTenantDataContainerAsync(Tenant tenant)
        {
            //コンテナ名は自動生成
            string containerName = $"delete-tenant-{tenant.Name}-{DateTime.Now.ToString("yyyyMMddHHmmssffffff")}";

            // KQI管理者権限用の認証トークンを取得する。
            string adminToken = await RegistKQIAdminNameSpaceAsync();
            if (adminToken == null)
            {
                //トークンがない場合、結果はnull
                return new ContainerInfo() { Status = ContainerStatus.Forbidden };
            }

            // アクセスレベルがPublicとPrivateのノードを取得
            var nodes = GetPublicAndPrivateNode();
            if (nodes == null || nodes.Count == 0)
            {
                //デプロイ可能なノードがゼロなら、エラー扱い
                return new ContainerInfo() { Status = ContainerStatus.Forbidden };
            }

            // 上書き不可の環境変数
            var notEditableEnvList = new Dictionary<string, string>()
            {
                { "KQI_TENANT_NAME", tenant.Name }, // 削除対象のテナント名(バケット名)
                { "KQI_SERVER", containerOptions.WebServerUrl },
                { "KQI_TOKEN", loginLogic.GenerateToken().AccessToken },
                { "http_proxy", containerOptions.Proxy },
                { "https_proxy", containerOptions.Proxy },
                { "no_proxy", containerOptions.NoProxy },
                { "HTTP_PROXY", containerOptions.Proxy },
                { "HTTPS_PROXY", containerOptions.Proxy },
                { "NO_PROXY", containerOptions.NoProxy },
                { "PYTHONUNBUFFERED", "true" }, // python実行時の標準出力・エラーのバッファリングをなくす
                { "LC_ALL", "C.UTF-8"}, // python実行時のエラー回避
                { "LANG", "C.UTF-8"}  // python実行時のエラー回避
            };

            //コンテナを起動するために必要な設定値をインスタンス化
            var inputModel = new RunContainerInputModel()
            {
                ID = tenant.Id,
                TenantName = containerOptions.KqiAdminNamespace, // Namespaceに使用される名前は、KqiAdminNamespace とする。
                LoginUser = CurrentUserInfo.Alias, //アカウントはエイリアスから指定
                Name = containerName,
                ContainerImage = "kamonohashi/cli:" + versionLogic.GetVersion(),
                ScriptType = "delete_tenant",
                Cpu = 1,
                Memory = 1, //メモリは仮決め
                Gpu = 0,
                KqiImage = "kamonohashi/cli:" + versionLogic.GetVersion(),
                KqiToken = loginLogic.GenerateToken().AccessToken,
                NfsVolumeMounts = new List<NfsVolumeMountModel>()
                {
                    // 結果が保存されているディレクトリ
                    new NfsVolumeMountModel()
                    {
                        Name = "nfs-tenant",
                        MountPath = "/kqi/tenant",
                        SubPath = "",
                        Server = tenant.Storage.NfsServer,
                        ServerPath = tenant.Storage.NfsRoot, // バケットすべてをマウントするので注意すること。
                        ReadOnly = false
                    }
                },
                ContainerSharedPath = new Dictionary<string, string>()
                {
                    { "tmp", "/kqi/tmp/" }
                },

                PrepareAndFinishContainerEnvList = notEditableEnvList, // 上書き不可の環境変数を設定
                MainContainerEnvList = notEditableEnvList, // 上書き不可の環境変数を設定

                ConstraintList = new Dictionary<string, List<string>>() {
                    // KQI管理者用名前空間の実行が許可されているサーバ（アクセスレベルが "Public" と "Private"）でのみ実行
                    { containerOptions.KqiAdminNamespace, new List<string> { "true" } }
                },
                ClusterManagerToken = adminToken,
                IsNodePort = true //ランダムポート指定。アクセス先ポートが動的に決まるようになる。
            };

            var outModel = await clusterManagementService.RunContainerAsync(inputModel);
            if (outModel.IsSuccess == false)
            {
                return new ContainerInfo() { Status = ContainerStatus.Failed };
            }
            return new ContainerInfo()
            {
                Name = containerName,
                Status = outModel.Value.Status,
                Host = outModel.Value.Host,
                Configuration = outModel.Value.Configuration
            };
        }
        #endregion

        /// <summary>
        /// 追加環境変数をマージする
        /// </summary>
        /// <param name="optionDic">追加対象の環境変数</param>
        /// <param name="envList">追加先の環境変数</param>
        private static void AddEnvListToInputModel(Dictionary<string, string> optionDic, Dictionary<string, string> envList)
        {
            if (optionDic != null)
            {
                foreach (var env in optionDic)
                {
                    // 設定済み環境変数に追加対象の環境変数をマージする

                    string value = env.Value ?? ""; //nullは空文字と見なす

                    envList[env.Key] = value; // あれば新しい値で上書き、なければ追加
                }
            }
        }


        #endregion

        #region クラスタ管理

        /// <summary>
        /// 全ノード情報を取得する。
        /// 取得失敗した場合はnullが返る。
        /// </summary>
        public async Task<IEnumerable<NodeInfo>> GetAllNodesAsync()
        {
            var registeredNodeNames = nodeRepository.GetAll().Select(n => n.Name).ToList();
            return await clusterManagementService.GetAllNodesAsync(registeredNodeNames);
        }

        /// <summary>
        /// ノード単位のパーティションリストを取得する
        /// </summary>
        public async Task<Result<Dictionary<string, string>, string>> GetNodePartitionMapAsync()
        {
            string labelPartition = containerOptions.ContainerLabelPartition;
            var registeredNodeNames = nodeRepository.GetAll().Select(n => n.Name).ToList();
            return await clusterManagementService.GetNodeLabelMapAsync(labelPartition, registeredNodeNames);
        }

        /// <summary>
        /// パーティションを更新する
        /// </summary>
        /// <param name="nodeName">ノード名</param>
        /// <param name="labelValue">ノード値</param>
        /// <returns>更新結果、更新できた場合、true</returns>
        public async Task<bool> UpdatePartitionLabelAsync(string nodeName, string labelValue)
        {
            return await this.clusterManagementService.SetNodeLabelAsync(nodeName, containerOptions.ContainerLabelPartition, labelValue);
        }

        /// <summary>
        /// TensorBoardの実行可否設定を更新する
        /// </summary>
        /// <param name="nodeName">ノード名</param>
        /// <param name="enabled">実行可否</param>
        /// <returns>更新結果、更新できた場合、true</returns>
        public async Task<bool> UpdateTensorBoardEnabledLabelAsync(string nodeName, bool enabled)
        {
            string value = enabled ? "true" : "";
            return await this.clusterManagementService.SetNodeLabelAsync(nodeName, containerOptions.ContainerLabelTensorBoardEnabled, value);
        }

        /// <summary>
        /// Notebookの実行可否設定を更新する
        /// </summary>
        /// <param name="nodeName">ノード名</param>
        /// <param name="enabled">実行可否</param>
        /// <returns>更新結果、更新できた場合、true</returns>
        public async Task<bool> UpdateNotebookEnabledLabelAsync(string nodeName, bool enabled)
        {
            string value = enabled ? "true" : "";
            return await this.clusterManagementService.SetNodeLabelAsync(nodeName, containerOptions.ContainerLabelNotebookEnabled, value);
        }

        /// <summary>
        /// テナントの実行可否設定を更新する
        /// </summary>
        /// <param name="nodeName">ノード名</param>
        /// <param name="tenantName">テナント名</param>
        /// <param name="enabled">実行可否</param>
        /// <returns>更新結果、更新できた場合、true</returns>
        public async Task<bool> UpdateTenantEnabledLabelAsync(string nodeName, string tenantName, bool enabled)
        {
            string value = enabled ? "true" : "";
            return await this.clusterManagementService.SetNodeLabelAsync(nodeName, tenantName, value);
        }

        /// <summary>
        /// 指定されたテナントのクォータ設定をクラスタに反映させる。
        /// </summary>
        /// <param name="tenant">テナント</param>
        /// <returns>更新結果、更新できた場合、true</returns>
        public async Task<bool> SetQuotaAsync(Tenant tenant)
        {
            return await this.clusterManagementService.SetQuotaAsync(
                tenant.Name,
                tenant.LimitCpu == null ? 0 : tenant.LimitCpu.Value,
                tenant.LimitMemory == null ? 0 : tenant.LimitMemory.Value,
                tenant.LimitGpu == null ? 0 : tenant.LimitGpu.Value);
        }

        /// <summary>
        /// 実行要求の各リソース数がテナントのクォータ設定を超過しているかチェックする。
        /// </summary>
        /// <param name="tenant">テナント</param>
        /// <param name="cpu">要求CPUコア数</param>
        /// <param name="memory">要求メモリ容量（GB）</param>
        /// <param name="gpu">要求GPU数</param>
        /// <returns>超過の場合エラーメッセージ、問題ない場合null</returns>
        public string CheckQuota(Tenant tenant, int cpu, int memory, int gpu)
        {
            // エラーメッセージ
            string errorMessage = null;

            // CPUコア数
            if (tenant.LimitCpu != null && cpu >= tenant.LimitCpu)
            {
                errorMessage = "The request of CPU exceeds the upper limit.";
            }
            // メモリ容量
            else if (tenant.LimitMemory != null && memory >= tenant.LimitMemory)
            {
                errorMessage = "The request of Memory exceeds the upper limit.";
            }
            // GPU数
            else if (tenant.LimitGpu != null && gpu > tenant.LimitGpu)
            {
                errorMessage = "The request of GPU exceeds the upper limit.";
            }
            return errorMessage;
        }

        #endregion

        #region 権限管理

        /// <summary>
        /// クラスタ管理サービス上で、指定したユーザ＆テナントにコンテナレジストリを登録する。
        /// idempotentを担保。
        /// </summary>
        public async Task<bool> RegistRegistryToTenantAsync(string selectedTenantName, UserTenantRegistryMap userRegistryMap)
        {
            if (userRegistryMap == null)
            {
                return false;
            }
            //初回登録時など、まだパスワードが設定されていなかったら、登録はしない。
            if (string.IsNullOrEmpty(userRegistryMap.RegistryPassword))
            {
                return true; //正常系扱い
            }
            string dockerCfg = registryLogic.GetDockerCfgAuthString(userRegistryMap);
            if (dockerCfg == null)
            {
                return false;
            }
            var inModel = new RegistRegistryTokenInputModel()
            {
                TenantName = selectedTenantName,
                RegistryTokenKey = userRegistryMap.RegistryTokenKey,
                DockerCfgAuthString = dockerCfg,
                Url = userRegistryMap.Registry.RegistryUrl
            };
            return await clusterManagementService.RegistRegistryTokenyAsync(inModel);
        }

        /// <summary>
        /// クラスタ管理サービス上で、指定したユーザ＆テナントにコンテナレジストリを登録する。
        /// idempotentを担保。
        /// </summary>
        public async Task<bool> RegistRegistryToTenantAsync(string tokenKey, string url, Registry registry, string selectedTenantName, string userName, string password)
        {
            string dockerCfg = registryLogic.GetDockerCfgAuthString(registry, userName, password);
            if (dockerCfg == null)
            {
                return false;
            }
            var inModel = new RegistRegistryTokenInputModel()
            {
                TenantName = selectedTenantName,
                RegistryTokenKey = tokenKey,
                DockerCfgAuthString = dockerCfg,
                Url = url,
            };
            return await clusterManagementService.RegistRegistryTokenyAsync(inModel);
        }


        /// <summary>
        /// 指定したテナントを作成する。
        /// 既にある場合は何もしない。
        /// </summary>
        public async Task<bool> RegistTenantAsync(string tenantName)
        {
            return await clusterManagementService.RegistTenantAsync(tenantName);
        }

        /// <summary>
        /// KQI管理者用の名前空間にユーザを登録し、認証トークンを取得する。
        /// 既にある場合は何もしない。
        /// </summary>
        private async Task<string> RegistKQIAdminNameSpaceAsync()
        {
            // KQI管理者用名前空間を作成する
            if ((await clusterManagementService.RegistTenantAsync(containerOptions.KqiAdminNamespace)) == false)
            {
                return null;
            }

            // ユーザ個人のクラスタ管理サービスへの認証トークンが存在するかチェックする
            string userToken = await GetUserAccessTokenAsync();
            if (userToken == null)
            {
                return null;
            }

            // KQI管理者用名前空間にユーザを登録と認証トークンを取得する
            return await clusterManagementService.RegistUserAsync(containerOptions.KqiAdminNamespace, CurrentUserInfo.Alias);
        }

        /// <summary>
        /// ログイン中のユーザ＆テナントに対する、クラスタ管理サービスにアクセスするためのトークンを取得する。
        /// 存在しない場合、新規に作成する。
        /// </summary>
        public async Task<string> GetUserAccessTokenAsync()
        {
            string token = userRepository.GetClusterToken(CurrentUserInfo.Id, CurrentUserInfo.SelectedTenant.Id);

            if (token == null)
            {
                //トークンがない場合、新規に作成する
                //作成時の名前はUserNameではなくAliasを使う
                if (string.IsNullOrEmpty(CurrentUserInfo.Alias))
                {
                    //Aliasがない場合は、乱数で作成する
                    string alias = Util.GenerateRandamString(10);

                    //DBに保存
                    var user = await userRepository.SetAliasAsync(CurrentUserInfo.Id, alias);
                    LogInformation($"Set alias {alias} to {CurrentUserInfo.Id}:{CurrentUserInfo.Name}");
                    unitOfWork.Commit(); //仮にクラスタトークンの生成に失敗しても、エイリアスは保存して、ロールバックはしない

                    CurrentUserInfo.Alias = alias;
                }
                token = await clusterManagementService.RegistUserAsync(TenantName, CurrentUserInfo.Alias);

                if (token == null)
                {
                    //トークン生成に失敗
                    return null;
                }

                //新規トークンをDBへ登録
                userRepository.SetClusterToken(CurrentUserInfo.Id, CurrentUserInfo.SelectedTenant.Id, token);
                unitOfWork.Commit();
            }

            return token;
        }

        /// <summary>
        /// 指定したテナントを抹消(削除)する。
        /// </summary>
        public async Task<bool> EraseTenantAsync(string tenantName)
        {
            return await clusterManagementService.EraseTenantAsync(tenantName);
        }

        /// <summary>
        /// 現在接続中のテナントが使用できるノード一覧を取得する
        /// </summary>
        private List<string> GetAccessibleNode()
        {
            return nodeRepository.GetAccessibleNodes(CurrentUserInfo.SelectedTenant.Id).Select(n => n.Name).ToList();
        }

        /// <summary>
        /// アクセスレベルがPublicとPrivateのノード一覧を取得する
        /// </summary>
        private List<string> GetPublicAndPrivateNode()
        {
            return nodeRepository.GetAll().Where(n => n.AccessLevel == NodeAccessLevel.Public || n.AccessLevel == NodeAccessLevel.Private).Select(n => n.Name).ToList();
        }
        #endregion

        #region WebSocket通信
        /// <summary>
        /// ブラウザとのWebSocket接続および、KubernetesとのWebSocket接続を確立する。
        /// そしてブラウザからのメッセージを待機し、メッセージを受信した際にはその内容をKubernetesに送信する。
        /// </summary>
        public async Task ConnectKubernetesWebSocketAsync(HttpContext context)
        {
            // ブラウザとのWebSocket接続を確立
            WebSocket browserWebSocket = await context.WebSockets.AcceptWebSocketAsync();


            // リクエストから、job名、tenant名を取得
            string jobName = context.Request.Query["jobName"];
            string tenantName = context.Request.Query["tenantName"];

            var containerOptions = CommonDiLogic.DynamicDi<IOptions<ContainerManageOptions>>();
            var token = containerOptions.Value.ResourceManageKey;   // 全テナントにアクセス可能な状態。CurrentUserInfoがnullであるため(Claim情報が無いため)、GetUserAccessTokenAsync()が使えない

            // KubernetesとのWebSocket接続を確立
            var kubernetesService = CommonDiLogic.DynamicDi<Services.Interfaces.IClusterManagementService>();
            var result = await kubernetesService.ConnectWebSocketAsync(jobName, tenantName, token);

            // 確立に失敗した場合は、ブラウザとの接続を切断
            if (result.Error != null)
            {
                var buff = new List<byte>(System.Text.Encoding.ASCII.GetBytes("\"" + jobName + "\" not found.\r\nConnection Closed."));
                await browserWebSocket.SendAsync(buff.ToArray(), WebSocketMessageType.Text, true, CancellationToken.None);

                await browserWebSocket.CloseOutputAsync(WebSocketCloseStatus.InternalServerError, "Kubernetes error", CancellationToken.None); ;
                return;
            }
            ClientWebSocket kubernetesWebSocket = result.Value;

            // Kubernetesの情報を中継する処理を、別スレッドで起動。
            var task = CommunicateKubernetesAsync(kubernetesWebSocket, browserWebSocket);

            // ブラウザからの入力を中継
            while (browserWebSocket.State == WebSocketState.Open)
            {
                try
                {
                    // ブラウザからメッセージ待受(通常入力時は、ブラウザ上コンソールから1文字単位で送られてくる。ペーストした場合は一度に複数文字送られてくる)
                    var buff = new ArraySegment<byte>(new byte[1024]);
                    var ret = await browserWebSocket.ReceiveAsync(buff, System.Threading.CancellationToken.None);

                    var sendK8sBuffer = new List<byte>();
                    sendK8sBuffer.Insert(0, 0); // stdin prefix(0x00)を追加

                    for (int i = 0; i < ret.Count; i++)
                    {
                        sendK8sBuffer.Add(buff[i]);
                    }
                    await kubernetesWebSocket.SendAsync(sendK8sBuffer.ToArray(), WebSocketMessageType.Binary, true, CancellationToken.None);
                }
                catch
                {
                    // ブラウザが異常終了した場合、Kubernetes側との接続が切れた場合
                    browserWebSocket.Dispose();
                    kubernetesWebSocket.Dispose();
                    return;
                }
            }

            // ブラウザが、切断要求を行った場合、
            await browserWebSocket.CloseOutputAsync(browserWebSocket.CloseStatus.Value, browserWebSocket.CloseStatusDescription, CancellationToken.None);
            await kubernetesWebSocket.CloseOutputAsync(browserWebSocket.CloseStatus.Value, browserWebSocket.CloseStatusDescription, CancellationToken.None);
            browserWebSocket.Dispose();
            kubernetesWebSocket.Dispose();
            return;
        }

        /// <summary>
        /// Kubernetesからのメッセージを待機し、メッセージを受信した際にはその内容をブラウザに送信する。
        /// </summary>
        private static async Task CommunicateKubernetesAsync(ClientWebSocket kubernetesWebSocket, WebSocket browserWebSocket)
        {
            while (kubernetesWebSocket.State == WebSocketState.Open)
            {
                //Kubernetesからメッセージ待受。メッセージを受信した際には、Browser側に送信
                var receivedBuffer = new ArraySegment<byte>(new byte[1024]);
                await kubernetesWebSocket.ReceiveAsync(receivedBuffer, CancellationToken.None);
                var sendBuffer = new List<byte>();
                for (int i = 1; i != receivedBuffer.Count; i++) // stdout prefix(0x01)を読み飛ばし
                {
                    byte b = receivedBuffer[i];
                    if (b != 0)  // 文字情報のみ抜き出し
                        sendBuffer.Add(b);
                }
                await browserWebSocket.SendAsync(sendBuffer.ToArray(), WebSocketMessageType.Text, true, CancellationToken.None);
            }

            // Kubernetesとの接続が切れた場合(ジョブが終了した場合等)
            await kubernetesWebSocket.CloseOutputAsync(kubernetesWebSocket.CloseStatus.Value, kubernetesWebSocket.CloseStatusDescription, CancellationToken.None);
            await browserWebSocket.CloseOutputAsync(kubernetesWebSocket.CloseStatus.Value, kubernetesWebSocket.CloseStatusDescription, CancellationToken.None);
        }
        #endregion
    }
}