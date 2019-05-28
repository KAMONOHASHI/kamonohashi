using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models.TenantModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ApiModels.TrainingApiModels
{
    public class TensorBoardOutputModel
    {
        public TensorBoardOutputModel(TensorBoardContainer container, ContainerStatus status, string endpoint = null)
        {
            Status = status.Name;
            StatusType = status.StatusType;

            if (container != null)
            {
                Name = container.Name;
                if (status.Exist() && string.IsNullOrEmpty(container.Host) == false)
                {
                    //リバプロなどでノードのホスト名ではなく、共通のエンドポイントを使える場合は、そっちを使う
                    string host = string.IsNullOrEmpty(endpoint) ? container.Host : endpoint;
                    Url = new UriBuilder("http", host, (int)container.PortNo).ToString();
                }
            }
        }

        /// <summary>
        /// コンテナ名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// ステータス
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// ステータス詳細
        /// </summary>
        public string StatusType { get; set; }
        /// <summary>
        /// アクセスURL
        /// </summary>
        public string Url { get; set; }
    }
}
