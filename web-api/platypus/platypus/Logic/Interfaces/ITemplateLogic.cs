using Microsoft.AspNetCore.Http;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Infos;
using Nssol.Platypus.Infrastructure.Types;
using Nssol.Platypus.Models;
using Nssol.Platypus.Models.TenantModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Logic.Interfaces
{
    /// <summary>
    /// 前処理ロジックインターフェイス
    /// </summary>
    public interface ITemplateLogic
    {
        /// <summary>
        /// 現在のアクセスユーザが利用可能なテンプレート一覧を取得する
        /// </summary>
        //List<string> GetAccessibleTemplates();
        
    }
}
