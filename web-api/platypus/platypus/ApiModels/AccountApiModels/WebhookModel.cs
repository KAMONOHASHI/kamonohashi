namespace Nssol.Platypus.ApiModels.AccountApiModels
{
    /// <summary>
    /// Webhook用モデル
    /// </summary>
    public class WebhookModel
    {
        /// <summary>
        /// Slackの送信先URL
        /// </summary>
        public string SlackUrl { get; set; }

        /// <summary>
        /// Slackメッセージのメンション
        /// </summary>
        public string Mention { get; set; }
    }
}
