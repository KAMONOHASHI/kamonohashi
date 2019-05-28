using Nssol.Platypus.Infrastructure.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Nssol.Platypus.Models
{
    /// <summary>
    /// Dockerレジストリ
    /// </summary>
    [Table("Registries")]
    public class Registry : ModelBase
    {
        /// <summary>
        /// 識別名。一意制約あり。
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// サーバホスト名
        /// </summary>
        [Required]
        public string Host { get; set; }
        /// <summary>
        /// サーバポート番号
        /// </summary>
        [Required]
        public int PortNo { get; set; }

        /// <summary>
        /// レジストリサービス種別
        /// </summary>
        [Required]
        public RegistryServiceType ServiceType {get;set;}

        /// <summary>
        /// プロジェクト名。
        /// GitLabの場合、今はここに"Owner/Repos"を入れる。
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 接続用パスワード。
        /// GitLabの場合、ここにトークンを入れる
        /// </summary>
        [Obsolete]
        public string Password { get; set; }
        /// <summary>
        /// API実行に使うURL。
        /// WebアプリケーションからREST APIで通信する場合はこちらを使う。
        /// docker pullの場合とポート番号が異なる場合があるため、<see cref="Host"/>+<see cref="PortNo"/>の組み合わせとは区別している
        /// </summary>
        public string ApiUrl { get; set; }
        /// <summary>
        /// レジストリUrl。
        /// ColusterManagerに渡すURLとして使用する。
        /// プロトコルが異なる場合があるため、<see cref="ApiUrl"/>とは区別している。
        /// </summary>
        [Required]
        public string RegistryUrl { get; set; }

        /// <summary>
        /// レジストリパス
        /// </summary>
        [NotMapped]
        public string RegistryPath {
            get
            {
                return $"{Host}:{PortNo}";
            }
        }

        /// <summary>
        /// クラスタ管理サービスに登録される、このリポジトリ用の認証トークン名
        /// </summary>
        [NotMapped]
        public string TokenKey
        {
            get
            {
                return $"registry-{Name}";
            }
        }

        /// <summary>
        /// このRegistry情報の概略を返す
        /// </summary>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// イメージのパス表現を取得する
        /// </summary>
        public string GetImagePath(string imageName, string tag)
        {
            if (ServiceType == RegistryServiceType.DockerHub)
            {
                //DockerHubの場合は前部分を省略
                return $"{imageName}:{tag}";
            }
            else
            {
                return $"{RegistryPath}/{imageName}:{tag}";
            }
        }
    }
}
