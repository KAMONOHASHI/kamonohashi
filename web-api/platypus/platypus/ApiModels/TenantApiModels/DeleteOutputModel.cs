namespace Nssol.Platypus.ApiModels.TenantApiModels
{
    public class DeleteOutputModel
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DeleteOutputModel()
        {
        }

        /// <summary>
        /// データ削除用コンテナに関するWarnメッセージ (Warnがなければ null で返却)
        /// </summary>
        public string ContainerWarnMsg { get; set; }
    }
}