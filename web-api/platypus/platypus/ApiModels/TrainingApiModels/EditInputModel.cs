namespace Nssol.Platypus.ApiModels.TrainingApiModels
{
    public class EditInputModel
    {
        /// <summary>
        /// 識別名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// メモ
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// お気に入り
        /// </summary>
        public bool? Favorite { get; set; }
    }
}
