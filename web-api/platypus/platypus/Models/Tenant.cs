using Nssol.Platypus.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nssol.Platypus.Models
{
    /// <summary>
    /// テナント情報。
    /// ログインユーザに基づいてテナント名がクレームに一つ格納される。
    /// その他の情報は必要に応じてテナント名をキーにDBから随時取得する。
    /// </summary>
    public class Tenant : ModelBase
    {
        /// <summary>
        /// テナント名。
        /// 一意制約があるため、最大長を固定。
        /// </summary>
        [Required]
        [RegularExpression("^[0-9a-z-]+$", ErrorMessage = "Please use only [lower case, number, -, .]")]
        [MaxLength(128)]
        public string Name { get; set; }
        /// <summary>
        /// テナント表示名
        /// </summary>
        [Required]
        public string DisplayName { get; set; }

        #region ストレージ情報
        /// <summary>
        /// ストレージID
        /// </summary>
        public long? StorageId { get; set; }

        /// <summary>
        /// オブジェクトストレージのバケット名
        /// </summary>
        public string StorageBucket { get; set; }

        /// <summary>
        /// データ用のNFSマウント元パス
        /// </summary>
        [NotMapped]
        public string DataNfsPath
        {
            get
            {
                return $"{Storage.NfsRootPath}{StorageBucket}/{ResourceType.Data}";
            }
        }
        
        /// <summary>
        /// コンテナ出力ファイル用のNFSマウント元パス
        /// </summary>
        [NotMapped]
        public string TrainingContainerOutputNfsPath
        {
            get
            {
                return $"{Storage.NfsRootPath}{StorageBucket}/{ResourceType.TrainingContainerOutputFiles}";
            }
        }

        /// <summary>
        /// コンテナ添付ファイル用のNFSマウント元パス
        /// </summary>
        [NotMapped]
        public string TrainingContainerAttachedNfsPath
        {
            get
            {
                return $"{Storage.NfsRootPath}{StorageBucket}/{ResourceType.TrainingContainerAttachedFiles}";
            }
        }
        /// <summary>
        /// 前処理のコンテナ添付ファイル用のNFSマウント元パス
        /// </summary>
        [NotMapped]
        public string PreprocContainerAttachedNfsPath
        {
            get
            {
                return $"{Storage.NfsRootPath}{StorageBucket}/{ResourceType.PreprocContainerAttachedFiles}";
            }
        }
        /// <summary>
        /// 推論ジョブのコンテナ出力ファイル用のNFSマウント元パス
        /// </summary>
        [NotMapped]
        public string InferenceContainerOutputNfsPath
        {
            get
            {
                return $"{Storage.NfsRootPath}{StorageBucket}/{ResourceType.InferenceContainerOutputFiles}";
            }
        }
        /// <summary>
        /// 推論のコンテナ添付ファイル用のNFSマウント元パス
        /// </summary>
        [NotMapped]
        public string InferenceContainerAttachedNfsPath
        {
            get
            {
                return $"{Storage.NfsRootPath}{StorageBucket}/{ResourceType.InferenceContainerAttachedFiles}";
            }
        }

        /// <summary>
        /// ノートブックのコンテナ出力ファイル用のNFSマウント元パス
        /// </summary>
        [NotMapped]
        public string NotebookContainerOutputNfsPath
        {
            get
            {
                return $"{Storage.NfsRootPath}{StorageBucket}/{ResourceType.NotebookContainerOutputFiles}";
            }
        }

        /// <summary>
        /// ノートブックのコンテナ添付ファイル用のNFSマウント元パス
        /// </summary>
        [NotMapped]
        public string NotebookContainerAttachedNfsPath
        {
            get
            {
                return $"{Storage.NfsRootPath}{StorageBucket}/{ResourceType.NotebookContainerAttachedFiles}";
            }
        }

        /// <summary>
        /// 実験のコンテナ出力ファイル用のNFSマウント元パス
        /// </summary>
        [NotMapped]
        public string ExperimentContainerOutputNfsPath
        {
            get
            {
                return $"{Storage.NfsRootPath}{StorageBucket}/{ResourceType.ExperimentContainerOutputFiles}";
            }
        }

        /// <summary>
        /// 実験のコンテナ添付ファイル用のNFSマウント元パス
        /// </summary>
        [NotMapped]
        public string ExperimentContainerAttachedNfsPath
        {
            get
            {
                return $"{Storage.NfsRootPath}{StorageBucket}/{ResourceType.ExperimentContainerAttachedFiles}";
            }
        }

        /// <summary>
        /// 実験前処理のコンテナ添付ファイル用のNFSマウント元パス
        /// </summary>
        [NotMapped]
        public string ExperimentPreprocContainerAttachedNfsPath
        {
            get
            {
                return $"{Storage.NfsRootPath}{StorageBucket}/{ResourceType.ExperimentPreprocContainerAttachedFiles}";
            }
        }
        #endregion

        /// <summary>
        /// GitリポジトリのID
        /// </summary>
        [Required]
        public long? DefaultGitId { get; set; }

        /// <summary>
        /// DockerレジストリのID
        /// </summary>
        [Required]
        public long? DefaultRegistryId { get; set; }

        /// <summary>
        /// Cpu制限（クォータ）
        /// </summary>
        public int? LimitCpu { get; set; }

        /// <summary>
        /// メモリ制限（クォータ）
        /// </summary>
        public int? LimitMemory { get; set; }

        /// <summary>
        /// Gpu制限（クォータ）
        /// </summary>
        public int? LimitGpu { get; set; }

        /// <summary>
        /// ノートブック無期限利用可否フラグ。
        /// Trueの場合、ノートブックの生存期間設定なし（無期限）起動を許可する。
        /// </summary>
        public bool AvailableInfiniteTimeNotebook { get; set; }

        /// <summary>
        /// ストレージ
        /// </summary>
        [ForeignKey(nameof(StorageId))]
        public virtual Storage Storage { get; set; }

        /// <summary>
        /// Gitリポジトリ
        /// </summary>
        [ForeignKey(nameof(DefaultGitId))]
        public virtual Git DefaultGit { get; set; }

        /// <summary>
        /// Dockerレジストリ
        /// </summary>
        [ForeignKey(nameof(DefaultRegistryId))]
        public Registry DefaultRegistry { get; set; }

        /// <summary>
        /// レジストリとのマッピング情報
        /// </summary>
        public virtual ICollection<TenantRegistryMap> RegistryMaps { get; set; }

        /// <summary>
        /// レジストリとのマッピング情報
        /// </summary>
        public virtual ICollection<TenantGitMap> GitMaps { get; set; }

        /// <summary>
        /// Slackの通知先URL
        /// </summary>
        public string SlackUrl { get; set; }
    }
}
