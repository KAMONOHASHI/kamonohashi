using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nssol.Platypus.ApiModels.UserApiModels
{
    public class CreateInputModel
    {
        /// <summary>
        /// 名前
        /// </summary>
        [Required]
        [Controllers.Util.CustomValidation(Controllers.Util.CustomValidationType.Email)]
        public string Name { get; set; }

        /// <summary>
        /// 表示名
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// パスワード
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// 最初に付与するシステムロールID
        /// </summary>
        public IEnumerable<long> SystemRoles { get; set; }

        /// <summary>
        /// 最初に所属させるテナント
        /// </summary>
        [Required]
        public List<TenantInfoModel> Tenants { get; set; }

        public class TenantInfoModel
        {
            [Required]
            public long? Id { get; set; }

            public Boolean Default { get; set; }

            public IEnumerable<long> Roles { get; set; }
        }
    }
}
