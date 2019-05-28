using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ApiModels.TenantApiModels
{
    public class DeleteOutputModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DeleteOutputModel()
        {
            UpdateUserIdList = new List<long>();
        }

        /// <summary>
        /// テナント削除の際に影響を及ぼした(更新された)ユーザIDを格納するリスト
        /// </summary>
        public List<long> UpdateUserIdList { get; set; }

        /// <summary>
        /// Storage のバケット削除(minio)に関するWarnメッセージ (Warnがなければ null で返却)
        /// </summary>
        public string StorageWarnMsg { get; set; }

        /// <summary>
        /// Kubernetes に関するWarnメッセージ (Warnがなければ null で返却)
        /// </summary>
        public string KubernetesWarnMsg { get; set; }
    }
}
