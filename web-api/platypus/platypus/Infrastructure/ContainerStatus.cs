using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Infrastructure
{
    /// <summary>
    /// コンテナのステータス詳細。
    /// </summary>
    /// <remarks>
    /// ステータスには大きく分けて以下の二種類がある。
    /// ・KAMONOHASHI内で定義したステータス
    /// ・コンテナ管理サービスから取得されるステータス。
    /// </remarks>
    public class ContainerStatus
    {
        #region 各ステータス
        /// <summary>
        /// コンテナが存在しない状態。
        /// k8sの場合：Jobが存在しない。
        /// </summary>
        public static ContainerStatus None = new ContainerStatus("KQI-None", ContainerStatusType.None, "None");
        /// <summary>
        /// コンテナは存在せず、ローカルで実行中の状態。
        /// コンテナ実行中と同様にDB側の情報変更は可能。
        /// </summary>
        public static ContainerStatus Opened = new ContainerStatus("KQI-Opened", ContainerStatusType.None, "Opened");
        /// <summary>
        /// コンテナが立った状態。実行中。
        /// この状況の時はコンテナ管理サービス側のステータスが引けるはずなので、このメンバは使われない想定。
        /// </summary>
        public static ContainerStatus Running = new ContainerStatus("KQI-Running", ContainerStatusType.Running, "Running");
        /// <summary>
        /// コンテナが正常終了した。
        /// </summary>
        public static ContainerStatus Completed = new ContainerStatus("KQI-Completed", ContainerStatusType.Closed, "Completed");
        /// <summary>
        /// コンテナを手動で削除した。
        /// </summary>
        public static ContainerStatus Killed = new ContainerStatus("KQI-Killed", ContainerStatusType.Failed, "Killed");
        /// <summary>
        /// コンテナ管理サービスに問い合わせられず、分からない状態。
        /// REST APIでエラーが発生したなど。
        /// </summary>
        public static ContainerStatus Failed = new ContainerStatus("KQI-Failed", ContainerStatusType.Failed, "Failed");
        /// <summary>
        /// コンテナ管理サービスの問い合わせ結果が不正で判断できない。
        /// REST APIのレスポンス内容がパースできない状態。
        /// </summary>
        public static ContainerStatus Invalid = new ContainerStatus("KQI-Invalid", ContainerStatusType.Failed, "Invalid");
        /// <summary>
        /// 権限不足。
        /// k8sの場合：アクセストークンを取得できない、利用可能なノードがない、など。テナントの初期化が不正とか。
        /// </summary>
        public static ContainerStatus Forbidden = new ContainerStatus("KQI-Forbidden", ContainerStatusType.Failed, "Forbidden");
        /// <summary>
        /// コンテナ管理サービスの問い合わせ結果が複数で判断できない。
        /// k8sの場合：Jobに複数のPodが紐づいている状態。想定外。
        /// </summary>
        public static ContainerStatus Multiple = new ContainerStatus("KQI-Multiple", ContainerStatusType.Error, "Multiple");
        /// <summary>
        /// コンテナ自体のデプロイは完了したが、中身がない状態。
        /// k8sの場合：Jobはあるけど、Podがない状態。
        /// エラーとして扱う。必要であれば、このステータスを付ける前に適宜リトライを行う事。
        /// </summary>
        public static ContainerStatus Empty = new ContainerStatus("KQI-Empty", ContainerStatusType.Error, "Empty");
        /// <summary>
        /// コンテナ管理サービスが明確なエラーを返している状態。
        /// 通常、コンテナ管理サービスのステータスが返ってきた場合は全て<see cref="ContainerStatusType.Running"/>とするが、流石にどう考えても失敗している場合は、<see cref="ContainerStatusType.Error"/>で扱う。
        /// </summary>
        public static ContainerStatus Error = new ContainerStatus("KQI-Error", ContainerStatusType.Error, "Error");

        #endregion

        /// <summary>
        /// 全てのKAMONOHASHI管理化のステータスのディクショナリ。
        /// DBには文字列が入っているので、それをオブジェクトに復元する際に使う。
        /// </summary>
        /// <remarks>
        /// Staticフィードの中でStaticオブジェクトを参照している。
        /// 初期化タイミングは宣言順のため、
        /// </remarks>
        private static Dictionary<string, ContainerStatus> AllStatus;

        #region 大分類の判定メソッド
        /// <summary>ステータスがErrorか(Failedは含まない)。</summary>
        public bool IsError()
        {
            return this.Type == ContainerStatusType.Error;
        }

        /// <summary>ステータスがRunningか＝コンテナが正常稼働中か</summary>
        public bool IsRunning()
        {
            return this.Type == ContainerStatusType.Running;
        }

        /// <summary>履歴が開放中か＝結果やステータスの変更を受け付けるか</summary>
        public bool IsOpened()
        {
            return this.Type == ContainerStatusType.Running || this == Opened;
        }

        /// <summary>
        /// ノード上に対応するコンテナが存在するか
        /// </summary>
        public bool Exist()
        {
            return this.Type == ContainerStatusType.Running ||
                this.Type == ContainerStatusType.Error;
        }

        /// <summary>
        /// 指定したステータスがErrorでない場合にtrueが返る。
        /// </summary>
        public static bool IsAvailable(string name)
        {
            if (AllStatus.ContainsKey(name) == false)
            {
                return true;
            }
            return AllStatus[name].Exist();
        }

        /// <summary>
        /// コンテナが正常系か
        /// </summary>
        /// <returns></returns>
        public bool Succeed()
        {
            return this.Type == ContainerStatusType.Running ||
                this.Type == ContainerStatusType.Closed;
        }

        /// <summary>
        /// ステータス名からインスタンスに変換する。
        /// </summary>
        public static ContainerStatus Convert(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                //古いデータはStatusが記録されていないことがある。そのデータは完了と見なす。
                return Completed;
            }
            if (AllStatus.ContainsKey(name) == false)
            {
                //コンテナ管理サービス側で生成されたステータスということ。正常扱い。
                return new ContainerStatus(name);
            }
            return AllStatus[name];
        }
        #endregion

        /// <summary>
        /// DBに格納されるキー。k8sで生成されるステータスも含むため、enumではなくstringで定義。
        /// KQI内部で定義されたステータスは、外部で生成されるステータスと重複しないように、"KQI-"がprefixとして付く。
        /// </summary>
        internal string Key { get; set; }

        /// <summary>
        /// ステータスの表示名。ユーザには<see cref="Key"/>は表示せず、こっちを見せる
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// KAMONOHASHI管理化のステータスであればtrue。
        /// </summary>
        internal bool IsDefined { get; private set; }

        /// <summary>
        /// ステータスの種類。
        /// Running: ジョブが正常に実行されている。
        /// Error: ジョブが異常な状態で実行されている。
        /// Closed: ジョブ実行が正常に完了し、実行結果が保存された。
        /// Failed: ジョブ実行が異常終了した。
        /// </summary>
        public string StatusType
        {
            get
            {
                return Type.ToString();
            }
        }

        /// <summary>
        /// ステータスの大分類
        /// </summary>
        private ContainerStatusType Type;

        /// <summary>
        /// staticコンストラクタ。
        /// staticフィールドの初期化後に呼ばれるので、staticプロパティを参照する処理はここで実行。
        /// </summary>
        static ContainerStatus()
        {
            AllStatus = new Dictionary<string, ContainerStatus>()
            {
                { None.Key, None },
                { Opened.Key, Opened },
                { Running.Key, Running },
                { Completed.Key, Completed },
                { Killed.Key, Killed },
                { Failed.Key, Failed },
                { Invalid.Key, Invalid },
                { Forbidden.Key, Forbidden },
                { Multiple.Key, Multiple },
                { Empty.Key, Empty },
                { Error.Key, Error },
            };
        }

        /// <summary>
        /// KAMONOHASHI管理化のステータス用コンストラクタ。
        /// 決まったstaticメンバしか利用しないようにprivate。
        /// </summary>
        private ContainerStatus(string key, ContainerStatusType type, string name)
        {
            this.Key = key;
            this.Type = type;
            this.Name = name;
            this.IsDefined = true;
        }

        /// <summary>
        /// KAMONOHASHI管理外のステータス用コンストラクタ。
        /// </summary>
        public ContainerStatus(string name) : base()
        {
            if(name == "Failed")
            {
                this.Key = Error.Key;
                this.Type = Error.Type;
                this.Name = Error.Name;
                this.IsDefined = Error.IsDefined;
            }
            else
            {
                this.Key = name;
                this.Type = ContainerStatusType.Running;
                this.Name = name;
                this.IsDefined = false;
            }
        }

        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// ステータスの大分類。
        /// 分類ごとに、ユーザの行動が変わるような差異を持つ。
        /// </summary>
        private enum ContainerStatusType
        {
            /// <summary>
            /// DB上のレコード、クラスタ上のコンテナ、共に存在しない。
            /// ユーザはコンテナを立てられる。
            /// </summary>
            None = 0,
            /// <summary>
            /// 稼働中。進行状況をユーザが確認できる。
            /// DB上のレコード、クラスタ上のコンテナ、共に存在する。
            /// クラスタ管理サービスが返すステータスは全てこの値にする。
            /// </summary>
            Running = 1,
            /// <summary>
            /// 停止済み。
            /// DB上のレコードは存在するが、クラスタ上のコンテナはない。
            /// ユーザが実行結果を参照できる。
            /// </summary>
            Closed = 2,
            /// <summary>
            /// 異常が発生している状態。ユーザは管理者に連絡する。
            /// DB上のレコードは存在するが、クラスタ上のコンテナはない（あるいは見つけられない）。
            /// </summary>
            Failed = 98,
            /// <summary>
            /// 異常が発生している状態。ユーザは管理者に連絡する。
            /// DB上のレコードは存在する。コンテナも異常な状態で存在している。。
            /// </summary>
            Error = 99,
        }
    }
}
