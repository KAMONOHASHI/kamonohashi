using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// ロールリポジトリインターフェース。
    /// ロール情報はキャッシュされており、外から通常通りのアクセスをされると困るので、<see cref="IRepository{Role}"/>を継承しない。
    /// </summary>
    public interface IRoleRepository
    {
        /// <summary>
        /// キャッシュを破棄する
        /// </summary>
        void Refresh();

        /// <summary>
        /// Idからロールを取得する。
        /// 対応するロールが見つからない場合はNULLを返す。
        /// </summary>
        Task<Role> GetRoleAsync(long id);

        /// <summary>
        /// 全ロールを取得する
        /// </summary>
        Task<IEnumerable<Role>> GetAllRolesAsync();

        /// <summary>
        /// ロールを新規に追加する
        /// </summary>
        void Add(Role role, IUnitOfWork unitOfWork);
        
        /// <summary>
        /// 更新用のロール情報を取得する。
        /// 普段はキャッシュからデータをとるが、それだとEntityFrameworkがキャッシュされたオブジェクトのIDを見失って、編集ではなく新規追加になる恐れがある。
        /// </summary>
        Task<Role> GetRoleForUpdateAsync(long id);

        /// <summary>
        /// ロール情報を更新する。
        /// 引数のRoleはキャッシュからではなく、<see cref="GetRoleForUpdateAsync(long)"/>で直接DBから取得したものを使うこと。
        /// </summary>
        void Update(Role role, IUnitOfWork unitOfWork);

        /// <summary>
        /// ロール情報を削除する
        /// </summary>
        Task DeleteAsync(long id, IUnitOfWork unitOfWork);

        /// <summary>
        /// 指定したユーザが持つロールを、Admin・テナント共に取得する。
        /// ユーザIDの存在チェックは行わない。
        /// </summary>
        IEnumerable<Role> GetRoles(long userId);

        /// <summary>
        /// 指定したテナントが持つカスタムロールを取得する。
        /// テナントIDの存在チェックは行わない。
        /// </summary>
        Task<IEnumerable<Role>> GetCustomRolesAsync(long tenantId);

        /// <summary>
        /// 指定したユーザが持つシステムロールを取得する。
        /// ユーザIDの存在チェックは行わない。
        /// </summary>
        IEnumerable<Role> GetSystemRoles(long userId);

        /// <summary>
        /// 指定したユーザ持つテナントロールを、(テナントID、ロールのリスト）のディクショナリ形式で 取得する。
        /// ユーザIDの存在チェックは行わない。
        /// </summary>
        Dictionary<long, List<Role>> GetTenantRolesDictionary(long userId);

        /// <summary>
        /// テナント横断で使用可能なテナントロールを取得する。
        /// </summary>
        Task<IEnumerable<Role>> GetCommonTenantRolesAsync();

        /// <summary>
        /// 指定したユーザが特定のテナントで持つテナントロールを取得する。
        /// ユーザID, テナントIDの存在チェックは行わない。
        /// </summary>
        IEnumerable<Role> GetTenantRoles(long userId, long tenantId);

        /// <summary>
        /// 指定したユーザにロールを付与する。
        /// <paramref name="role"/>がシステムロールの場合、<paramref name="userTenantMap"/>はNULLになって、テナントに関係なくそのロールが必ず付与される。
        /// <paramref name="role"/>がテナントロールの場合、<paramref name="userTenantMap"/>は非NULLになる。
        /// その場合、<paramref name="user"/>と<paramref name="userTenantMap"/>のUserIdは一致している必要があり、
        /// <paramref name="role"/>のテナントIDが非NULLなら<paramref name="userTenantMap"/>のテナントIDと一致している必要がある。
        /// </summary>
        /// <param name="user">対象ユーザ</param>
        /// <param name="role">対象ロール</param>
        /// <param name="userTenantMap">テナントマップ</param>
        /// <param name="isCreate">ユーザが新規作成の状態(=ID未割当)ならtrue</param>
        void AttachRole(User user, Role role, UserTenantMap userTenantMap, bool isCreate);

        /// <summary>
        /// 指定したユーザから、すべてのシステムロールを外す。
        /// ユーザIDの存在チェックは行わない。
        /// </summary>
        void DetachSystemRole(long userId);


        /// <summary>
        /// 指定したロールに、指定したメニューへのアクセス権限があるか、確認する
        /// </summary>
        /// <returns></returns>
        Task<bool> AuthorizeAsync(long roleId, MenuCode menuCode);
    }
}
