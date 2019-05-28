using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.Models.TenantModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.DataAccess.Repositories.Interfaces.TenantRepositories;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models.CustomModels;

namespace Nssol.Platypus.DataAccess.Repositories.TenantRepositories
{
    /// <summary>
    /// データテーブルにアクセスするためのリポジトリクラス
    /// </summary>
    /// <seealso cref="Interfaces.TenantRepositories.IDataRepository" />
    public class DataRepository : RepositoryForTenantBase<Data>, IDataRepository
    {
        private readonly DbQuery<DataIndex> dbDataIndex;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DataRepository(CommonDbContext context, Microsoft.AspNetCore.Http.IHttpContextAccessor accessor)
            : base(context, accessor)
        {
            dbDataIndex = context.DataIndex;
        }

        /// <summary>
        /// 全データをタグ情報付きで取得する。
        /// ソート順はIDの逆順。
        /// </summary>
        public IQueryable<Data> GetAllIncludeTag()
        {
            return GetAll().Include(d => d.TagMaps).ThenInclude(tm => tm.Tag)
                                             .OrderByDescending(t => t.Id);
        }

        /// <summary>
        /// DataのIndex情報をViewから取得する
        /// </summary>
        public IQueryable<DataIndex> GetDataIndex()
        {
            //普通に検索すると非常に重いので、生SQL文をそのまま発行させる
            return dbDataIndex.FromSql(
                "select d.\"Id\", d.\"DisplayId\", d.\"Name\", d.\"CreatedAt\", d.\"CreatedBy\", d.\"ModifiedAt\", d.\"ModifiedBy\", d.\"Memo\", tag.\"Tag\", d.\"TenantId\", parent.\"Id\" as \"ParentDataId\" , parent.\"Name\" as \"ParentDataName\" " +
                "from \"Data\" d " +
                "left join \"Data\" parent on d.\"ParentDataId\" = parent.\"Id\" " +
                "left join " +
                "(select map.\"DataId\", array_to_string(array_agg(t.\"Name\"), ',') as \"Tag\" " +
                "from \"DataTagMaps\" as map " +
                "left join \"Tags\" t on t.\"Id\" = map.\"TagId\" " +
                "where map.\"TenantId\" = {0} " + 
                "group by map.\"DataId\" " +
                "order by map.\"DataId\" " +
                ") tag on d.\"Id\" = tag.\"DataId\" " +
                "where d.\"TenantId\" = {0} ", CurrentTenantId); // こう書くと文字列の結合ではなくSQLのパラメタにして渡してくれる
        }

        /// <summary>
        /// 指定したデータIDのデータ（データファイル、タグの外部参照を含む）を取得します。
        /// </summary>
        public async Task<Data> GetDataIncludeAllAsync(long id)
        {
            return await GetAll().Include(d => d.TagMaps).ThenInclude(tm => tm.Tag)
                .Include(d => d.DataProperties).ThenInclude(dp => dp.DataFile)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        /// <summary>
        /// 指定したデータに新規ファイルを追加する
        /// </summary>
        public DataProperty AddFile(Data data, string fileName, string storedPath)
        {
            //ファイルの場合、プロパティのキーにはファイル名を指定する
            var property = AddFile(data, fileName, storedPath, fileName);
            return property;
        }

        /// <summary>
        /// 指定したデータに新規ファイルを追加する
        /// </summary>
        private DataProperty AddFile(Data data, string fileName, string storedPath, string key)
        {
            DataFile file = new DataFile()
            {
                FileName = fileName,
                StoredPath = storedPath
            };
            DataProperty property = new DataProperty()
            {
                Data = data,
                DataFile = file,
                Key = key
            };

            AddModel<DataFile>(file);
            AddModel<DataProperty>(property);

            if (data.DataProperties != null)
            {
                data.DataProperties.Add(property);
            }

            return property;
        }

        /// <summary>
        /// 既存プロパティを取得する
        /// </summary>
        public DataProperty GetDataProperty(long dataId, string key)
        {
            var dataProperty = GetModelAll<DataProperty>().Include(d => d.DataFile).FirstOrDefault(d => d.DataId == dataId && d.DataFile.FileName == key);
            return dataProperty;
        }

        /// <summary>
        /// 既存プロパティを全て取得する
        /// </summary>
        public IEnumerable<DataProperty> GetAllDataProperty(long dataId)
        {
            var dataProperties = GetModelAll<DataProperty>().Include(d => d.DataFile).Where(d => d.DataId == dataId).OrderByDescending(t => t.DataFileId);
            return dataProperties;
        }

        /// <summary>
        /// データを削除する。
        /// 紐づいているファイルもすべて削除する。
        /// </summary>
        public void DeleteData(Data data)
        {
            foreach(var file in data.DataProperties)
            {
                if (file.DataFile != null)
                {
                    DeleteModel<DataFile>(file.DataFile);
                }
                DeleteModel<DataProperty>(file);
            }

            Delete(data);
        }
    }
}
