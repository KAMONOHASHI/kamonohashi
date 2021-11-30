using System;
using System.Text;

namespace Nssol.Platypus.ServiceModels.Git
{
    /// <summary>
    /// コミット情報
    /// </summary>
    public class CommitModel
    {
        /// <summary>
        /// コミットID
        /// </summary>
        public string CommitId { get; set; }
        /// <summary>
        /// コミッター名
        /// </summary>
        public string CommitterName { get; set; }
        /// <summary>
        /// コミット日時
        /// </summary>
        public string CommitAt { get; set; }
        /// <summary>
        /// コミットコメント
        /// </summary>
        public string Comment { get; set; }
        /// <summary>
        /// 文字列表現。
        /// </summary>
        public string Display
        {
            get
            {
                StringBuilder buf = new StringBuilder();
                buf.Append(this.CommitterName ?? string.Empty);
                buf.Append(" ");
                buf.Append(this.CommitAt ?? string.Empty);
                buf.Append(Environment.NewLine);
                buf.Append(this.Comment ?? string.Empty);
                return buf.ToString();
            }
        }
    }
}
