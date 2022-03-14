using Nssol.Platypus.Models.TenantModels;
using System.Linq;


namespace Nssol.Platypus.ApiModels.TrainingApiModels
{
    public class SearchHistoryOutputModel : Components.OutputModelBase
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SearchHistoryOutputModel(TrainingSearchHistories history) : base(history)
        {
            Name = history.Name;
            Id = history.Id;
            SearchDetail = new SearchDetailInputModel();
            SearchDetail.IdUpper = history.IdUpper;
            SearchDetail.IdLower = history.IdLower;
            SearchDetail.StartedAtLower = history.StartedAtLower;
            SearchDetail.StartedAtUpper = history.StartedAtUpper;
            if (string.IsNullOrEmpty(history.TrainingName) == false)
            {
                SearchDetail.Name = history.TrainingName.Split(",").ToList();
                SearchDetail.NameOr = history.NameOr;
            }
            else
            {
                SearchDetail.NameOr = true;
            }
            if (string.IsNullOrEmpty(history.ParentName) == false)
            {
                SearchDetail.ParentName = history.ParentName.Split(",").ToList();
                SearchDetail.ParentNameOr = history.ParentNameOr;
            }
            else
            {
                SearchDetail.ParentNameOr = true;
            }
            if (string.IsNullOrEmpty(history.StartedBy) == false)
            {
                SearchDetail.StartedBy = history.StartedBy.Split(",").ToList();
                SearchDetail.StartedByOr = history.StartedByOr;
            }
            else
            {
                SearchDetail.StartedByOr = true;
            }
            if (string.IsNullOrEmpty(history.DataSet) == false)
            {
                SearchDetail.DataSet = history.DataSet.Split(",").ToList();
                SearchDetail.DataSetOr = history.DataSetOr;
            }
            else
            {
                SearchDetail.StartedByOr = true;
            }
            if (string.IsNullOrEmpty(history.EntryPoint) == false)
            {
                SearchDetail.EntryPoint = history.EntryPoint.Split(",").ToList();
                SearchDetail.EntryPointOr = history.EntryPointOr;
            }
            else
            {
                SearchDetail.EntryPointOr = true;
            }
            if (string.IsNullOrEmpty(history.Memo) == false)
            {
                SearchDetail.Memo = history.Memo.Split(",").ToList();
                SearchDetail.MemoOr = history.MemoOr;
            }
            else
            {
                SearchDetail.MemoOr = true;
            }
            if (string.IsNullOrEmpty(history.Tags) == false)
            {
                SearchDetail.Tags = history.Tags.Split(",").ToList();
                SearchDetail.TagsOr = history.TagsOr;
            }
            else
            {
                SearchDetail.TagsOr = true;
            }
            if (string.IsNullOrEmpty(history.Status) == false)
            {
                SearchDetail.Status = history.Status.Split(",").ToList();
                SearchDetail.StatusOr = history.StatusOr;
            }
            else
            {
                SearchDetail.StatusOr = true;
            }
        }

        /// <summary>
        /// 履歴の登録名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 履歴のid
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 検索の内容
        /// </summary>
        public SearchDetailInputModel SearchDetail { get; set; }
    }
}
