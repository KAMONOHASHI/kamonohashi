using Nssol.Platypus.DataAccess.Core;
using Nssol.Platypus.DataAccess.Repositories.Interfaces;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Types;
using Nssol.Platypus.Logic.Interfaces;
using Nssol.Platypus.Models;
using Nssol.Platypus.Models.TenantModels;
using System.Threading.Tasks;

namespace Nssol.Platypus.Logic
{
    /// <summary>
    /// テンプレートロジッククラス
    /// </summary>
    /// <seealso cref="Nssol.Platypus.Logic.Interfaces.ITemplateLogic" />
    public class TemplateLogic : PlatypusLogicBase, ITemplateLogic
    {
        private readonly ITemplateRepository templateRepository;
        private IDataLogic dataLogic;
        private IStorageLogic storageLogic;
        private readonly IClusterManagementLogic clusterManagementLogic;
        private IUnitOfWork unitOfWork;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TemplateLogic(
            IDataLogic dataLogic,
            IStorageLogic storageLogic,
            IClusterManagementLogic clusterManagementLogic,
            IUnitOfWork unitOfWork,
            ICommonDiLogic commonDiLogic) : base(commonDiLogic)
        {
            this.dataLogic = dataLogic;
            this.storageLogic = storageLogic;
            this.clusterManagementLogic = clusterManagementLogic;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        ///　現在接続中のテナントが使用できるテンプレート一覧を取得する
        /// 
        /// </summary>
        //public async List<string> GetAccessibleTemplates()
        //{
        //    return templateRepository.GetAccessibleTemplates(CurrentUserInfo.SelectedTenant.Id).Select(n => n.Name).ToList();
        //}
    }       
}
