using Nssol.Platypus.Models;

namespace Nssol.Platypus.ApiModels.GitApiModels
{
    public class DetailsOutputModel : IndexOutputModel
    {
        public DetailsOutputModel(Git git) : base(git)
        {
            IsNotEditable = git.IsNotEditable;
        }

        /// <summary>
        /// 編集不可
        /// </summary>
        /// <remarks>
        /// true：編集不可　false：編集可
        /// </remarks>
        public bool IsNotEditable { get; set; }
    }
}
