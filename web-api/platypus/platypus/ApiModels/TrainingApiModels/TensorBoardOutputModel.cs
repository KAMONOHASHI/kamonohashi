using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Models.TenantModels;
using System;
using System.Globalization;

namespace Nssol.Platypus.ApiModels.TrainingApiModels
{
    /// <summary>
    /// TensorBoard情報出力モデル
    /// </summary>
    public class TensorBoardOutputModel
    {
        public TensorBoardOutputModel(TensorBoardContainer container, ContainerStatus status, string endpoint = null)
        {
            Status = status.Name;
            StatusType = status.StatusType;

            if (container != null)
            {
                Name = container.Name;
                if (status.Exist() && string.IsNullOrEmpty(container.Host) == false)
                {
                    //リバプロなどでノードのホスト名ではなく、共通のエンドポイントを使える場合は、そっちを使う
                    string host = string.IsNullOrEmpty(endpoint) ? container.Host : endpoint;
                    Url = new UriBuilder("http", host, (int)container.PortNo).ToString();
                }

                // 残り生存時間を計算する
                if (container.ExpiresIn.HasValue && container.ExpiresIn.Value != 0)
                {
                    long elapsedTicks = container.StartedAt.AddSeconds(container.ExpiresIn.Value).Ticks - DateTime.Now.Ticks;
                    TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);
                    if (elapsedSpan.Ticks < 0)
                    {
                        RemainingTime = "0d 0h 0m";
                    }
                    else
                    {
                        RemainingTime = elapsedSpan.ToString(@"%d'd '%h'h '%m'm'", CultureInfo.InvariantCulture);
                    }
                }
                else
                {
                    // 生存時間が無期限の場合はnullを返す
                    RemainingTime = null;
                }
            }
        }

        /// <summary>
        /// コンテナ名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ステータス
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// ステータス詳細
        /// </summary>
        public string StatusType { get; set; }

        /// <summary>
        /// アクセスURL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// コンテナの残存時間(%d d %h h %m m)
        /// </summary>
        public string RemainingTime { get; set; }
    }
}
