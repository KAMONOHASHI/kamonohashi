using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ServiceModels.ClusterManagementModels
{
    /// <summary>
    /// 接続先のクラスタ管理サービス(k8s, eks等)の情報を格納するモデル
    /// </summary>
    public class KubernetesEndpointModel
    {
        /// <summary>
        /// 接続先クラスタのID (DBに格納されているEKSの場合に使用する)
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 接続先クラスタ名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// APIのホスト名
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// ポート番号
        /// </summary>
        public string PortNumber { get; set; }

        /// <summary>
        /// トークン (EKSのsaのトークン等)
        /// </summary>
        
        public string Token { get; set; }

        /// <summary>
        /// コンテナ管理サービスのベースURL
        /// ポート番号が80の場合はホスト名を、それ以外の場合はホスト名とポート番号を組み合わせた文字列を返却する
        /// </summary>
        public string ContainerServiceBaseUrl
        {
            get
            {
                if ("80".Equals(PortNumber))
                {
                    return HostName;
                }
                else
                {
                    return $"{HostName}:{PortNumber}";
                }
            }
        }
    }
}
