using System.Text.RegularExpressions;

namespace Nssol.Platypus.ApiModels.ExperimentApiModels
{
    public class AttachedFileOutputModel
    {
        public AttachedFileOutputModel()
        {
        }

        public AttachedFileOutputModel(long id, string fileName, long fileId)
        {
            this.Id = id;
            this.FileName = fileName;
            this.FileId = GetFileId(fileId, fileName);
        }

        /// <summary>
        /// 実験履歴ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 添付ファイルID
        /// </summary>
        public long FileId { get; set; }
        /// <summary>
        /// ファイルURL
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// ファイル名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 削除可能か
        /// </summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// ファイルIDを取得する
        /// </summary>
        /// <param name="fileId">ファイルID</param>
        /// <param name="fileName">ファイル名</param>
        private long GetFileId(long fileId, string fileName)
        {
            // ファイルIDが負の場合、ファイル名によってIDを設定する。
            if (fileId < 0)
            {
                // ジョブ結果ファイル
                if (Regex.IsMatch(fileName, @"(experiment)_output_\d+.zip"))
                {
                    return -1;
                }
                // ジョブ結果ログファイル
                else if (Regex.IsMatch(fileName, @"(experiment)_stdout_stderr_\d+.log"))
                {
                    return -2;
                }
                // それ以外のファイル
                else
                {
                    return -99;
                }
            }
            // ファイルIDが負以外の場合、そのまま
            return fileId;
        }
    }
}
