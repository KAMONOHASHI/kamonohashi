using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Options;
using Nssol.Platypus.Models;
using System.Collections.Generic;

namespace Nssol.Platypus.ApiModels.ResourceApiModels
{
    /// <summary>
    /// テナント単位のリソース情報出力モデル
    /// </summary>
    public class TenantResourceOutputModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="tenant">テナント情報</param>
        public TenantResourceOutputModel(Tenant tenant)
        {
            Id = tenant.Id;
            Name = tenant.DisplayName;
            AllocatableCpu = tenant.LimitCpu == null ? 0 : tenant.LimitCpu.Value;
            AllocatableMemory = tenant.LimitMemory == null ? 0 : tenant.LimitMemory.Value;
            AllocatableGpu = tenant.LimitGpu == null ? 0 : tenant.LimitGpu.Value;

            ContainerResourceList = new List<ContainerDetailsOutputModel>();
        }

        /// <summary>
        /// DBにないテナントに対応する出力モデルを作成する
        /// </summary>
        public TenantResourceOutputModel(string tenantName, ContainerManageOptions containerManageOptions)
        {
            if (tenantName == containerManageOptions.KqiAdminNamespace)
            {
                // KQI管理者用。(テナント実体は存在しないが、k8sの名前空間として使用している)
                Id = 0;
                Name = containerManageOptions.KqiAdminNamespace;
                DisplayName = containerManageOptions.KqiAdminNamespace;
            }
            else
            {
                // 正体不明テナント
                Id = -1;
                Name = tenantName;
                DisplayName = "Unknown:" + tenantName;
            }

            ContainerResourceList = new List<ContainerDetailsOutputModel>();
        }

        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// テナント表示名
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 割り当て可能なCpu
        /// </summary>
        public float AllocatableCpu { get; set; }

        /// <summary>
        /// 割り当て可能なメモリ(単位：GB)
        /// </summary>
        public float AllocatableMemory { get; set; }

        /// <summary>
        /// 割り当て可能なGPU
        /// </summary>
        public float AllocatableGpu { get; set; }

        /// <summary>
        /// 割り当て済みCpu
        /// </summary>
        public float AssignedCpu { get; set; }

        /// <summary>
        /// 割り当て済みメモリ(単位：GB)
        /// </summary>
        public float AssignedMemory { get; set; }

        /// <summary>
        /// 割り当て済みGPU
        /// </summary>
        public float AssignedGpu { get; set; }

        /// <summary>
        /// Cpu情報取得
        /// </summary>
        public string CpuInfo
        {
            get
            {
                return $"{AssignedCpu} /" + (AllocatableCpu == 0 ? "Infinity" : AllocatableCpu.ToString("0.0"));
            }
        }

        /// <summary>
        /// メモリ情報取得
        /// </summary>
        public string MemoryInfo
        {
            get
            {
                return $"{AssignedMemory} GB /" + (AllocatableMemory == 0 ? "Infinity" : AllocatableMemory.ToString("0.0 GB"));
            }
        }

        /// <summary>
        /// Gpu情報取得
        /// </summary>
        public string GpuInfo
        {
            get
            {
                return $"{AssignedGpu} /" + (AllocatableGpu == 0 ? "Infinity" : AllocatableGpu.ToString("0.0"));
            }
        }

        /// <summary>
        /// コンテナリソースのリスト
        /// </summary>
        public List<ContainerDetailsOutputModel> ContainerResourceList { get; set; }
        
        public void Add(ContainerDetailsOutputModel model)
        {
            model.TenantId = Id;

            AssignedCpu = Util.SumOfFloat(AssignedCpu, model.Cpu);
            AssignedMemory = Util.SumOfFloat(AssignedMemory, model.Memory);
            AssignedGpu += model.Gpu;
            ContainerResourceList.Add(model);
        }
    }
}
