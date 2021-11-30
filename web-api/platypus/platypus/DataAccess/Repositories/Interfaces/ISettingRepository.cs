using Microsoft.IdentityModel.Tokens;
using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.Models;

namespace Nssol.Platypus.DataAccess.Repositories.Interfaces
{
    public interface ISettingRepository : IRepository<Setting>
    {
        /// <summary>
        /// APIでトークンの生成に使う共通鍵を取得する
        /// </summary>
        SymmetricSecurityKey GetApiJwtSigningKey();
    }
}
