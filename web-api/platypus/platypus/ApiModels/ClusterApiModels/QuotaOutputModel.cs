using Nssol.Platypus.Models;

namespace Nssol.Platypus.ApiModels.ClusterApiModels
{
    /// <summary>
    /// クォータ設定情報出力モデル
    /// </summary>
    public class QuotaOutputModel : QuotaInputModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="tenant">テナント情報</param>
        public QuotaOutputModel(Tenant tenant)
        {
            this.TenantId = tenant.Id;
            this.TenantName = tenant.DisplayName;
            this.Cpu = Convert(tenant.LimitCpu);
            this.Memory = Convert(tenant.LimitMemory);
            this.Gpu = Convert(tenant.LimitGpu);
        }

        /// <summary>
        /// テナント表示名
        /// </summary>
        public string TenantName { get; set; }

        /// <summary>
        /// null許容型intをint型に変換
        /// </summary>
        /// <param name="d">null許容型int</param>
        /// <returns>int型</returns>
        private int Convert(int? d)
        {
            if (d.HasValue)
            {
                return d.Value;
            }
            return 0;
        }
    }
}
