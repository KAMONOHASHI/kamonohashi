using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nssol.Platypus.Models.TenantModels
{
    /// <summary>
    /// 既存データからTensorBoardを表示するためのコンテナのモデル
    /// </summary>
    public class TensorBoardContainer : TenantModelBase
    {
        /// <summary>
        /// コンテナ名
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// ホスト
        /// </summary>
        /// <remarks>
        /// 立てた直後はホスト名が決まらないことがあるので、NULLを許可する
        /// </remarks>
        public string Host { get; set; }

        /// <summary>
        /// ポート番号
        /// </summary>
        /// <remarks>
        /// 立てた直後はホスト名が決まらないことがあるので、NULLを許可する
        /// </remarks>
        public int? PortNo { get; set; }

        /// <summary>
        /// 学習履歴ID。
        /// </summary>
        [Required]
        public long TrainingHistoryId { get; set; }
        /// <summary>
        /// ステータス
        /// </summary>
        [Required]
        public string Status { get; set; }
        /// <summary>
        /// 実行開始日時
        /// </summary>
        [Required]
        public DateTime StartedAt { get; set; }

        /// <summary>
        /// コンテナの生存期間(秒)
        /// </summary>
        [Required]
        public int? ExpiresIn { get; set; }

        /// <summary>
        /// 学習履歴
        /// </summary>
        [ForeignKey(nameof(TrainingHistoryId))]
        public virtual TrainingHistory TrainingHistory { get; set; }

        /// <summary>
        /// コンテナ情報の文字列表現を返す。
        /// </summary>
        public override string ToString()
        {
            return $"{Id}({Host}:{PortNo}):{Name}:{Status}";
        }

        /// <summary>
        /// マウントした学習履歴ID
        /// </summary>
        public string MountedTrainingHistoryIds { get; set; }

        /// <summary>
        /// マウントした学習履歴IDリスト
        /// </summary>
        [NotMapped]
        public List<long> MountedTrainingHistoryIdList { get; set; }

        /// <summary>
        /// マウントした学習履歴ID
        /// </summary>
        /// リストの情報をカンマ区切りで1つにまとめる
        public string GetMountedTrainingHistoryIds()
        {
            if ( MountedTrainingHistoryIdList == null || MountedTrainingHistoryIdList.Count < 1)
            {
                return null;
            }
            foreach (long selectedHistoryId in MountedTrainingHistoryIdList)
            {
                MountedTrainingHistoryIds = MountedTrainingHistoryIds + selectedHistoryId + ",";
            }
            MountedTrainingHistoryIds = MountedTrainingHistoryIds.TrimEnd(',');
            return MountedTrainingHistoryIds;
        }


        /// <summary>
        /// マウントした学習履歴IDのリスト表現
        /// </summary>
        /// <returns>マウントした学習履歴ID</returns>
        public List<long> GetMountedTrainingHistoryIdList()
        {
            if (MountedTrainingHistoryIds == null)
            {
                return new List<long>();
            }
            string[] historyIds = MountedTrainingHistoryIds.Split(',');
            List<long> mountedTrainingHistoryIds = new List<long>();
            foreach (string id in historyIds)
            {
                mountedTrainingHistoryIds.Add(long.Parse(id));
            }
            MountedTrainingHistoryIdList = mountedTrainingHistoryIds;
            return MountedTrainingHistoryIdList;
        }
    }
}
