using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.ServiceModels.ClusterManagementModels
{
    /// <summary>
    /// クラスタ管理サービスで新規にコンテナを起動するための入力モデルクラス。
    /// </summary>
    public class RunContainerInputModel
    {
        /// <summary>
        /// テナント名
        /// </summary>
        public string TenantName { get; set; }

        /// <summary>
        /// ログインユーザ。
        /// SSHする場合はSSHユーザ名になる。
        /// </summary>
        public string LoginUser { get; set; }

        /// <summary>
        /// ID。nfsマウント時に作るサブディレクトリ等、一意性が欲しい箇所で使用
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// コンテナ名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// コンテナイメージパス
        /// リポジトリパス/イメージ名の形。
        /// </summary>
        public string ContainerImage { get; set; }

        /// <summary>
        /// CPU利用数（最大値）
        /// </summary>
        public int Cpu { get; set; }

        /// <summary>
        /// メモリ容量（MB）
        /// </summary>
        public int Memory { get; set; }

        /// <summary>
        /// GPU数
        /// </summary>
        public int Gpu { get; set; }

        /// <summary>
        /// コンテナ内の環境変数にセットする変数名と値（複数可）のペア
        /// </summary>
        public Dictionary<string, string> EnvList { get; set; }

        /// <summary>
        /// 制約のリスト
        /// 同一の設定がされているノードでしか起動しない。
        /// </summary>
        public Dictionary<string, List<string>> ConstraintList { get; set; }

        /// <summary>
        /// 最初に実行されるコマンド。
        /// Templateによっては実行コマンドが既定されているものがあるので、使われないこともある。
        /// </summary>
        public string EntryPoint { get; set; }
        
        /// <summary>
        /// NFSマウント情報
        /// </summary>
        public List<NfsVolumeMountModel> NfsVolumeMounts { get; set; }
        /// <summary>
        /// prepare,main,finishのコンテナ間で共有するディレクトリの共有名(なんでもよい)とパス
        /// </summary>
        public Dictionary<string, string> ContainerSharedPath { get; set; }
        /// <summary>
        /// ホストのポートとコンテナのポートのマッピング情報。
        /// </summary>
        public PortMappingModel[] PortMappings { get; set; }

        /// <summary>
        /// コンテナを起動するための認証トークン
        /// </summary>
        public string ClusterManagerToken { get; set; }

        /// <summary>
        /// KAMONOHASHI認証トークン
        /// </summary>
        public string KqiToken { get; set; }
        /// <summary>
        /// イメージレジストリからイメージを取得するための認証トークン
        /// </summary>
        public string RegistryTokenName { get; set; }

        /// <summary>
        /// コンテナのログファイルのパス
        /// </summary>
        public string LogPath { get; set; }
        /// <summary>
        /// コンテナ起動時に実行するスクリプトの種類。
        /// 値に応じて、 /kqi/scripts/{ScriptType}/ の内容が書き換えられる。
        /// </summary>
        public string ScriptType { get; set; }

        /// <summary>
        /// コンテナ起動時に実行するコマンド。
        /// </summary>
        public string Cmd { get; set; }

        /// <summary>
        /// 外からコンテナに接続する際、ノードへ直接アクセスするか。
        /// falseの場合はクラスタ管理マスタ経由でアクセスする。
        /// trueの場合、<see cref="PortMappingModel.NodePort"/> で指定されたノードへ直接アクセスする。
        /// </summary>
        public bool IsNodePort { get; set; }

        /// <summary>
        /// KqiCLIイメージ名
        /// </summary>
        public string KqiImage { get; set; }
    }
}
