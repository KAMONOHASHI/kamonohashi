using System.Collections.Generic;

namespace Nssol.Platypus.ServiceModels.KubernetesModels
{
    /// <summary>
    /// サービスアカウントの存在確認をした時の返り値
    /// </summary>
    public class GetServiceAccountOutputModel
    {
        public List<SecretModel> Secrets { get; set; }

        public class SecretModel
        {
            public string Name { get; set; }
        }
    }
}
