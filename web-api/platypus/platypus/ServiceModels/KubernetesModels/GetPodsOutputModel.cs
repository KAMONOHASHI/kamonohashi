using Newtonsoft.Json;
using Nssol.Platypus.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ServiceModels.KubernetesModels
{
    /// <summary>
    /// Jobのステータス確認をした結果
    /// </summary>
    /// <remarks>
    /// 結果のサンプルをこのファイルの末尾に記載（長いので頭には置きたくなかった）
    /// </remarks>
    public class GetPodsOutputModel
    {
        public List<ItemModel> Items { get; set; }

        public class ItemModel
        {
            public MetadataModel Metadata { get; set; }

            public StatusModel Status { get; set; }

            public SpecModel Spec { get; set; }


            public string ConditionNote
            {
                get
                {

                    return Status.Conditions?.FirstOrDefault(c => string.IsNullOrEmpty(c.Message) == false)?.Message;
                }
            }
        }

        public class MetadataModel
        {
            public string Namespace { get; set; }

            public string Name { get; set; }

            public string CreationTimestamp { get; set; }

            public LabelsModel Labels { get; set; }
        }

        public class LabelsModel
        {
            public string App { get; set; }
        }

        public class StatusModel
        {
            public string Phase { get; set; }

            public string HostIP { get; set; }

            public string StartTime { get; set; }

            public List<ConditionModel> Conditions { get; set; }

            public List<ContainerStatusModel> ContainerStatuses { get; set; }

            public bool isOOMKilled
            {
                get
                {
                    // コンテナが立っているか
                    if (ContainerStatuses != null)
                    {
                        // mainコンテナのステータスを取得する
                        var mainContainerStatus = ContainerStatuses.Where(container => container.name == "main").FirstOrDefault().state;
                        if (mainContainerStatus.terminated != null && mainContainerStatus.terminated.reason == "OOMKilled")
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }
        }

        public class ConditionModel
        {
            public string Type { get; set; }
            public string Status { get; set; }
            public string LastProbeTime { get; set; }
            public string LastTransitionTime { get; set; }
            public string Reason { get; set; }
            public string Message { get; set; }
        }

        public class ContainerStatusModel
        {
            public string name { get; set; }
            public StateModel state { get; set; }
        }

        public class StateModel
        {
            public TerminatedModel terminated { get; set; }
            public class TerminatedModel
            {
                public string exitCode { get; set; }
                public string reason { get; set; }
            }
        }

        public class SpecModel
        {
            public string NodeName { get; set; }

            public string ServiceAccountName { get; set; }

            public List<ContainerModel> Containers { get; set; }

            public class ContainerModel
            {
                public string Name { get; set; }
                public string Image { get; set; }
                public ResourcesModel Resources { get; set; }
            }

            public class ResourcesModel
            {
                public NodeResourceModel Requests { get; set; }
            }
        }
    }
}

