using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;
using Nssol.Platypus.Models;

namespace Nssol.Platypus.ApiModels.EksApiModels
{
    public class EksOutputModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EksOutputModel(Eks eks, IEnumerable<Tenant> tenants)
        {
            Id = eks.Id;
            Name = eks.Name;
            Token = eks.Token;
            HostName = eks.HostName;
            PortNumber = int.Parse(eks.PortNumber);

            var AnableTenantsList = new List<AnableTenant>();
            foreach (var tenant in tenants)
            {
                AnableTenantsList.Add(new AnableTenant()
                {
                    Name = tenant.Name,
                    Id = tenant.Id,
                    DisplayName = tenant.DisplayName
                });
            }
            AnableTenants = AnableTenantsList;
        }

        /// <summary>
        /// ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 登録名 (システム上一意であることが必要)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// EKSへのアクセストークン
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// EKSのAPIエンドポイント
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// EKSのAPIエンドポイントのポート番号
        /// </summary>
        public long PortNumber { get; set; }

        /// <summary>
        /// 最低限の接続先テナント情報を返すクラス
        /// </summary>
        public class AnableTenant
        {
            /// <summary>
            /// テナントID
            /// </summary>
            public long Id { get; set; }

            /// <summary>
            /// テナント名
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// テナント表示名
            /// </summary>
            public string DisplayName { get; set; }
        }

        /// <summary>
        /// 接続先テナント
        /// </summary>
        public IEnumerable<AnableTenant> AnableTenants { get; set; }
    }
}
