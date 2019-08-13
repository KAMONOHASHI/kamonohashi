using Nssol.Platypus.ApiModels.DataApiModels;
using Nssol.Platypus.Models.TenantModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Logic.Interfaces
{
    /// <summary>
    /// データに対する共通処理をまとめたインターフェース。
    /// <see cref="Controllers.spa.DataSetController"/>の中で、他のControllerからも使いたい処理ができた場合、ここで共通化する。
    /// </summary>
    public interface IDataLogic
    {
        /// <summary>
        /// 指定したIDの全ファイル情報を取得する。
        /// </summary>
        /// <param name="dataId">データID</param>
        /// <param name="withUrl">結果にダウンロード用のURLを含めるか</param>
        IEnumerable<DataFileOutputModel> GetDataFiles(long dataId, bool withUrl);

        /// <summary>
        /// 指定したIDの全ファイル情報を取得する。
        /// </summary>
        /// <param name="data">データ</param>
        /// <param name="withUrl">結果にダウンロード用のURLを含めるか</param>
        IEnumerable<DataFileOutputModel> GetDataFiles(Data data, bool withUrl);

        /// <summary>
        /// 指定されたIDのデータを削除する。
        /// まずDBのレコードを削除して、その後オブジェクトストレージから上記のデータを削除する。
        /// コミットは行わないので、呼び出し側で実行すること。（基本的には結果に関わらずコミットする）
        /// </summary>
        /// <param name="dataId">データID</param>
        Task<bool> DeleteDataAsync(long dataId);
    }
}
