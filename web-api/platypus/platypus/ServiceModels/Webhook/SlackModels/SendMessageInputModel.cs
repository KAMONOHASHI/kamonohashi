using Nssol.Platypus.Models;
using System.Text.RegularExpressions;

namespace Nssol.Platypus.ServiceModels.Webhook.SlackModels
{
    /// <summary>
    /// Slackのメッセージ送信用モデル
    /// </summary>
    public class SendMessageInputModel
    {
        /// <summary>
        /// メッセージブロックの縁の色
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// メッセージのタイトル
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// メンション
        /// </summary>
        public string Mention { get; set; }

        /// <summary>
        /// メッセージ文
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 対象ジョブの履歴ID
        /// </summary>
        public long Id { get; set; }

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
        public string BaseUrl { get { return GetBaseUrl(); } }

        /// <summary>
        ///  メッセージ送信先ApiPath
        /// </summary>
        public string ApiPath { get { return GetApiPath(); } }
        
        /// <summary>
        /// 送信先URLからベースURLを取得する
        /// </summary>
        private string GetBaseUrl()
        {
            return Regex.Match(WebhookUrl, @"https://.*?/").Value;
        }

        /// <summary>
        /// 送信先URLからAPIパスを取得する
        /// </summary>
        private string GetApiPath()
        {
            return Regex.Replace(WebhookUrl, @"https://.*?/", "/");
        }
    }
}
