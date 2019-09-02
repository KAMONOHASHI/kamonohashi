using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Infrastructure
{
    /// <summary>
    /// メニューコードの一覧
    /// </summary>
    public enum MenuCode
    {
        /// <summary>
        /// エラー用
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// アカウント管理
        /// </summary>
        Account,
        /// <summary>
        /// ログイン
        /// </summary>
        Login,
        /// <summary>
        /// ダッシュボード
        /// </summary>
        DashBoard,
        /// <summary>
        /// データ管理
        /// </summary>
        Data,
        /// <summary>
        /// データセット管理
        /// </summary>
        DataSet,
        /// <summary>
        /// ノートブック管理
        /// </summary>
        Notebook,
        /// <summary>
        /// 前処理管理
        /// </summary>
        Preprocess,
        /// <summary>
        /// 前処理履歴管理
        /// </summary>
        PreprocessHistory,
        /// <summary>
        /// 学習管理
        /// </summary>
        Training,
        /// <summary>
        /// 推論ジョブ管理
        /// </summary>
        Inference,

        /// <summary>
        /// テナント用テナント管理
        /// </summary>
        TenantSetting,
        /// <summary>
        /// テナント用ロール管理
        /// </summary>
        TenantRole,
        /// <summary>
        /// テナント用ユーザ管理
        /// </summary>
        TenantUser,
        /// <summary>
        /// テナント用メニュー管理
        /// </summary>
        TenantMenu,
        /// <summary>
        /// テナント用リソース管理
        /// </summary>
        TenantResource,

        /// <summary>
        /// テナント管理
        /// </summary>
        Tenant,
        /// <summary>
        /// Git管理
        /// </summary>
        Git,
        /// <summary>
        /// レジストリ管理
        /// </summary>
        Registry,
        /// <summary>
        /// ストレージ管理
        /// </summary>
        Storage,
        /// <summary>
        /// ロール管理
        /// </summary>
        Role,
        /// <summary>
        /// クオータ管理
        /// </summary>
        Quota,
        /// <summary>
        /// ユーザ管理
        /// </summary>
        User,
        /// <summary>
        /// ノード管理
        /// </summary>
        Node,
        /// <summary>
        /// メニュー管理
        /// </summary>
        Menu,
        /// <summary>
        /// リソース管理
        /// </summary>
        Resource
    }
}
