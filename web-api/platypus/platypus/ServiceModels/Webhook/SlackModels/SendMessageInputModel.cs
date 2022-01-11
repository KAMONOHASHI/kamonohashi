using System.Text.RegularExpressions;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models;

namespace Nssol.Platypus.ServiceModels.Webhook.SlackModels
{
    public class SendMessageInputModel
    {
        /// <summary>
        /// メッセージブロックの淵の色
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// メッセージのタイトル
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 対象ジョブの履歴ID
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 対象ジョブの名前
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 対象ジョブのテナント
        /// </summary>
        public Tenant Tenant { get; set; }

        /// <summary>
        /// 対象ジョブの作成者名
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 対象ジョブのステータス
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 対象ジョブのURL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// メッセージ送信先URL
        /// </summary>
        public string WebhookUrl { get; set; }

        /// <summary>
        ///  メッセージ送信先ベースURL
        /// </summary>
        public string BaseUrl { get { return getBaseUrl(); } }

        /// <summary>
        ///  メッセージ送信先ApiPath
        /// </summary>
        public string ApiPath { get { return getApiPath(); } }
        
        private string getBaseUrl()
        {
            return Regex.Match(this.WebhookUrl, @"https://.*?/").Value;
        }

        private string getApiPath()
        {
            return Regex.Replace(this.WebhookUrl, @"https://.*?/", "/");
        }
    }
}
