﻿using Nssol.Platypus.Models.TenantModels;

namespace Nssol.Platypus.ApiModels.TrainingApiModels
{
    /// <summary>
    /// 検索履歴出力モデル
    /// </summary>
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
                SearchDetail.Name = history.TrainingName;
                SearchDetail.NameOr = history.TrainingNameOr;
            }
            else
            {
                SearchDetail.NameOr = true;
            }
            if (string.IsNullOrEmpty(history.ParentName) == false)
            {
                SearchDetail.ParentName = history.ParentName;
                SearchDetail.ParentNameOr = history.ParentNameOr;
            }
            else
            {
                SearchDetail.ParentNameOr = true;
            }
            if (string.IsNullOrEmpty(history.StartedBy) == false)
            {
                SearchDetail.StartedBy = history.StartedBy;
                SearchDetail.StartedByOr = history.StartedByOr;
            }
            else
            {
                SearchDetail.StartedByOr = true;
            }
            if (string.IsNullOrEmpty(history.DataSet) == false)
            {
                SearchDetail.DataSet = history.DataSet;
                SearchDetail.DataSetOr = history.DataSetOr;
            }
            else
            {
                SearchDetail.DataSetOr = true;
            }
            if (string.IsNullOrEmpty(history.EntryPoint) == false)
            {
                SearchDetail.EntryPoint = history.EntryPoint;
                SearchDetail.EntryPointOr = history.EntryPointOr;
            }
            else
            {
                SearchDetail.EntryPointOr = true;
            }
            if (string.IsNullOrEmpty(history.Memo) == false)
            {
                SearchDetail.Memo = history.Memo;
                SearchDetail.MemoOr = history.MemoOr;
            }
            else
            {
                SearchDetail.MemoOr = true;
            }
            if (string.IsNullOrEmpty(history.Tags) == false)
            {
                SearchDetail.Tags = history.Tags;
                SearchDetail.TagsOr = history.TagsOr;
            }
            else
            {
                SearchDetail.TagsOr = true;
            }
            if (string.IsNullOrEmpty(history.Status) == false)
            {
                SearchDetail.Status = history.Status;
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
