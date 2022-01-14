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
        /// SlackメッセージのメンションID
        /// </summary>
        public string MentionId { get; set; }
    }
}
