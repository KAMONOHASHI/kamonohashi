using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ServiceModels.KubernetesModels
{
    public class GetEventsOutputModel
    {
        public List<ItemModel> Items { get; set; }

        public class ItemModel
        {
            public MetadataModel Metadata { get; set; }

            public InvolvedObjectModel InvolvedObject { get; set; }

            public string Message { get; set; }

            public string Reason { get; set; }

            public long? Count { get; set; }

            public string Type { get; set; }

            /// <summary>
            /// 初回登録日時
            /// </summary>
            public string FirstTimestamp { get; set; }

            /// <summary>
            /// 最終更新日時
            /// </summary>
            public string LastTimestamp { get; set; }
        }

        public class MetadataModel
        {
            public string Uid { get; set; }

            public string Namespace { get; set; }

            public string Name { get; set; }
        }

        public class InvolvedObjectModel
        {
            /// <summary>
            /// 対象オブジェクト種別。
            /// e.g. Job, Pod
            /// </summary>
            public string Kind { get; set; }
            /// <summary>
            /// 対象オブジェクト名
            /// </summary>
            public string Name { get; set; }

            public string ContainerName
            {
                get
                {
                    if(Kind == "Pod")
                    {
                        //Podの場合はコンテナ名-(ランダムな5文字)になる。
                        //既に無くなったものも含め、関連する情報はすべて回収したいので、今は上記フォーマット決め打ちでコンテナ名を判定する
                        return Name.Substring(0, Name.Length - 6);
                    }
                    return Name;
                }
            }
        }
    }
}