using Microsoft.EntityFrameworkCore;
using Nssol.Platypus.Models;
using Nssol.Platypus.Models.CustomModels;
using Nssol.Platypus.Models.TenantModels;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Nssol.Platypus.DataAccess
{
    /// <summary>
    /// メタ情報や認証情報など、テナント共通で扱うデータを処理するためのDBコンテキスト。
    /// </summary>
    public partial class CommonDbContext : DbContext
    {
        #region 共通DbSet

        /// <summary>
        /// 設定値
        /// </summary>
        public virtual DbSet<Setting> Settings { get; set; }

        /// <summary>
        /// テナント
        /// </summary>
        public virtual DbSet<Tenant> Tenants { get; set; }

        /// <summary>
        /// 学習用のtensorboardコンテナ
        /// </summary>
        public virtual DbSet<TensorBoardContainer> TensorBoardContainers { get; set; }

        /// <summary>
        /// Git
        /// </summary>
        public virtual DbSet<Git> Gits { get; set; }

        /// <summary>
        /// Gitとテナントの中間テーブル
        /// </summary>
        public virtual DbSet<TenantGitMap> TenantGitMaps { get; set; }

        /// <summary>
        /// ユーザーマッパー
        /// </summary>
        public virtual DbSet<UserTenantMap> UserTenantMaps { get; set; }

        /// <summary>
        /// ユーザー
        /// </summary>
        public virtual DbSet<User> Users { get; set; }

        /// <summary>
        /// ロール
        /// </summary>
        public virtual DbSet<Role> Roles { get; set; }

        /// <summary>
        /// レジストリ
        /// </summary>
        public virtual DbSet<Registry> Registries { get; set; }

        /// <summary>
        /// レジストリとテナントの中間テーブル
        /// </summary>
        public virtual DbSet<TenantRegistryMap> TenantRegistryMaps { get; set; }

        /// <summary>
        /// ストレージ
        /// </summary>
        public virtual DbSet<Storage> Storages { get; set; }

        /// <summary>
        /// クラスタノード
        /// </summary>
        public virtual DbSet<Node> Nodes { get; set; }

        /// <summary>
        /// クラスタノードとテナントの中間テーブル
        /// </summary>
        public virtual DbSet<NodeTenantMap> NodeTenantMaps { get; set; }

        /// <summary>
        /// テンプレート
        /// </summary>
        public virtual DbSet<Template> Templates { get; set; }

        /// <summary>
        /// テンプレートバージョン
        /// </summary>
        public virtual DbSet<TemplateVersion> TemplateVersions { get; set; }

        /// <summary>
        /// ユーザとロールの中間テーブル
        /// </summary>
        public virtual DbSet<UserRoleMap> UserRoleMaps { get; set; }

        /// <summary>
        /// メニューとロールの中間テーブル
        /// </summary>
        public virtual DbSet<MenuRoleMap> MenuRoleMaps { get; set; }

        /// <summary>
        /// ユーザと<see cref="TenantRegistryMaps"/>の中間テーブル
        /// </summary>
        public virtual DbSet<UserTenantRegistryMap> UserTenantRegistryMaps { get; set; }

        /// <summary>
        /// ユーザと<see cref="TenantGitMaps"/>の中間テーブル
        /// </summary>
        public virtual DbSet<UserTenantGitMap> UserTenantGitMaps { get; set; }

        /// <summary>
        /// リソースモニタサンプルテーブル
        /// </summary>
        public virtual DbSet<ResourceSample> ResourceSamples { get; set; }

        /// <summary>
        /// リソースモニタノードテーブル
        /// </summary>
        public virtual DbSet<ResourceNode> ResourceNodes { get; set; }

        /// <summary>
        /// リソースモニタコンテナテーブル
        /// </summary>
        public virtual DbSet<ResourceContainer> ResourceContainers { get; set; }

        /// <summary>
        /// リソースモニタジョブテーブル
        /// </summary>
        public virtual DbSet<ResourceJob> ResourceJobs { get; set; }

        #endregion

        #region テナント用DbSet
        /// <summary>
        /// データ
        /// </summary>
        public virtual DbSet<Data> Data { get; set; }
        /// <summary>
        /// データプロパティ
        /// </summary>
        public virtual DbSet<DataProperty> DataProperties { get; set; }
        /// <summary>
        /// データファイル
        /// </summary>
        public virtual DbSet<DataFile> DataFiles { get; set; }
        /// <summary>
        /// データセット
        /// </summary>
        public virtual DbSet<DataSet> DataSets { get; set; }
        /// <summary>
        /// データセットエントリ
        /// </summary>
        public virtual DbSet<DataSetEntry> DataSetEntries { get; set; }
        /// <summary>
        /// データ種別
        /// </summary>
        public virtual DbSet<DataType> DataTypes { get; set; }
        /// <summary>
        /// 前処理
        /// </summary>
        public virtual DbSet<Preprocess> Preprocesses { get; set; }
        /// <summary>
        /// 学習用画像
        /// </summary>
        public virtual DbSet<PreprocessHistoryOutput> PreprocessHistoryOutputs { get; set; }
        /// <summary>
        /// 前処理履歴
        /// </summary>
        public virtual DbSet<PreprocessHistory> PreprocessHistories { get; set; }
        /// <summary>
        /// 学習履歴
        /// </summary>
        public virtual DbSet<TrainingHistory> TrainingHistories { get; set; }
        /// <summary>
        /// 学習履歴と親学習履歴の中間テーブル
        /// </summary>
        public virtual DbSet<TrainingHistoryParentMap> TrainingHistoryParentMaps { get; set; }
        /// <summary>
        /// 学習履歴添付ファイル
        /// </summary>
        public virtual DbSet<TrainingHistoryAttachedFile> TrainingHistoryAttachedFiles { get; set; }
        /// <summary>
        /// 推論履歴
        /// </summary>
        public virtual DbSet<InferenceHistory> InferenceHistories { get; set; }
        /// <summary>
        /// 推論履歴と親学習履歴の中間テーブル
        /// </summary>
        public virtual DbSet<InferenceHistoryParentMap> InferenceHistoryParentMaps { get; set; }

        /// <summary>
        /// 推論履歴と親推論履歴の中間テーブル
        /// </summary>
        public virtual DbSet<InferenceHistoryParentInferenceMap> InferenceHistoryParentInferenceMaps { get; set; }

        /// <summary>
        /// 推論履歴添付ファイル
        /// </summary>
        public virtual DbSet<InferenceHistoryAttachedFile> InferenceHistoryAttachedFiles { get; set; }
        /// <summary>
        /// タグ
        /// </summary>
        public virtual DbSet<Tag> Tags { get; set; }
        /// <summary>
        /// データタグマップ
        /// </summary>
        public virtual DbSet<DataTagMap> DataTagMaps { get; set; }
        /// <summary>
        /// データタグマップ
        /// </summary>
        public virtual DbSet<TrainingHistoryTagMap> TrainingHistoryTagMaps { get; set; }
        /// <summary>
        /// ノートブック履歴
        /// </summary>
        public virtual DbSet<NotebookHistory> NotebookHistories { get; set; }
        /// <summary>
        /// ノートブック履歴と親学習履歴の中間テーブル
        /// </summary>
        public virtual DbSet<NotebookHistoryParentTrainingMap> NotebookHistoryParentTrainingMaps { get; set; }
        /// <summary>
        /// ノートブック履歴と親推論履歴の中間テーブル
        /// </summary>
        public virtual DbSet<NotebookHistoryParentInferenceMap> NotebookHistoryParentInferenceMaps { get; set; }

        /// <summary>
        /// 実験
        /// </summary>
        public virtual DbSet<Experiment> Experiments { get; set; }

        /// <summary>
        /// 実験前処理
        /// </summary>
        public virtual DbSet<ExperimentPreprocess> ExperimentPreprocesses { get; set; }

        /// <summary>
        /// アクアリウム推論
        /// </summary>
        public virtual DbSet<Models.TenantModels.Aquarium.Evaluation> AquariumEvaluations { get; set; }

        /// <summary>
        /// アクアリウムデータセット
        /// </summary>
        public virtual DbSet<Models.TenantModels.Aquarium.DataSet> AquariumDatasets { get; set; }
        /// <summary>
        /// アクアリウムデータセットバージョン
        /// </summary>
        public virtual DbSet<Models.TenantModels.Aquarium.DataSetVersion> AquariumDatasetVersions { get; set; }

        #endregion

        #region View

        /// <summary>
        /// データのIndex
        /// </summary>
        public virtual DbSet<DataIndex> DataIndex { get; set; }
        #endregion

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CommonDbContext(
            DbContextOptions<CommonDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// シーケンス関連付け
        /// </summary>
        /// <param name="modelBuilder">modelBuilder</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // VIEWの紐づけ
            modelBuilder.Entity<DataIndex>().HasNoKey().ToView("View_DataIndex");

            // unique制約の付与
            // EFCoreではまだアノテーションではできない
            modelBuilder.Entity<Tenant>()
                    .HasIndex(e => new { e.Name })
                    .IsUnique();
            modelBuilder.Entity<Registry>()
                    .HasIndex(e => new { e.Name })
                    .IsUnique();
            modelBuilder.Entity<Setting>()
                    .HasIndex(e => new { e.ApiSecurityTokenPass })
                    .IsUnique();
            //中間テーブル
            modelBuilder.Entity<MenuRoleMap>()
                    .HasIndex(e => new { e.MenuCode, e.RoleId })
                    .IsUnique();
            modelBuilder.Entity<NodeTenantMap>()
                    .HasIndex(e => new { e.NodeId, e.TenantId })
                    .IsUnique();
            modelBuilder.Entity<TenantGitMap>()
                    .HasIndex(e => new { e.TenantId, e.GitId })
                    .IsUnique();
            modelBuilder.Entity<TenantRegistryMap>()
                    .HasIndex(e => new { e.TenantId, e.RegistryId })
                    .IsUnique();
            modelBuilder.Entity<UserRoleMap>()
                    //TenantMapIdはNullableなので、UKを付けても厳密には一意性を保てない
                    .HasIndex(e => new { e.UserId, e.RoleId, e.TenantMapId })
                    .IsUnique();
            modelBuilder.Entity<UserTenantGitMap>()
                    .HasIndex(e => new { e.UserId, e.TenantGitMapId })
                    .IsUnique();
            modelBuilder.Entity<UserTenantMap>()
                    .HasIndex(e => new { e.TenantId, e.UserId })
                    .IsUnique();
            modelBuilder.Entity<UserTenantRegistryMap>()
                    .HasIndex(e => new { e.UserId, e.TenantRegistryMapId })
                    .IsUnique();
            modelBuilder.Entity<TrainingHistoryParentMap>()
                    .HasIndex(e => new { e.TenantId, e.TrainingHistoryId, e.ParentId })
                    .IsUnique();
            modelBuilder.Entity<InferenceHistoryParentMap>()
                    .HasIndex(e => new { e.TenantId, e.InferenceHistoryId, e.ParentId })
                    .IsUnique();
            modelBuilder.Entity<InferenceHistoryParentInferenceMap>()
                    .HasIndex(e => new { e.TenantId, e.InferenceHistoryId, e.ParentId })
                    .IsUnique();
            modelBuilder.Entity<NotebookHistoryParentTrainingMap>()
                    .HasIndex(e => new { e.TenantId, e.NotebookHistoryId, e.ParentId })
                    .IsUnique();
            modelBuilder.Entity<NotebookHistoryParentInferenceMap>()
                    .HasIndex(e => new { e.TenantId, e.NotebookHistoryId, e.ParentId })
                    .IsUnique();

            // DeleteBehaviorの指定
            // モデルのアノテーションで指定できるとベターだが設定方法が不明なので、ここで実装している。

            // DefaultTenant は Cascade(デフォルト) ではなくRestrict とする。
            modelBuilder.Entity(typeof(User))
                .HasOne(typeof(Tenant), "DefaultTenant")
                .WithMany()
                .HasForeignKey("DefaultTenantId")
                .OnDelete(DeleteBehavior.Restrict);
            // TenantId は Restrict(デフォルト) ではなく Cascade とする。
            modelBuilder.Entity(typeof(Role))
                .HasOne(typeof(Tenant), "Tenant")
                .WithMany()
                .HasForeignKey("TenantId")
                .OnDelete(DeleteBehavior.Cascade);
        }

        /// <summary>
        /// <para>
        /// Override this method to configure the database (and other options) to be used for this context.
        /// This method is called for each instance of the context that is created.
        /// </para>
        /// <para>
        /// In situations where an instance of <see cref="Microsoft.EntityFrameworkCore.DbContextOptions" /> may or may not have been passed
        /// to the constructor, you can use <see cref="Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.IsConfigured" /> to determine if
        /// the options have already been set, and skip some or all of the logic in
        /// <see cref="Microsoft.EntityFrameworkCore.DbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)" />.
        /// </para>
        /// </summary>
        /// <param name="optionsBuilder">A builder used to create or modify options for this context. Databases (and other extensions)
        /// typically define extension methods on this object that allow you to configure the context.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Startup.DefaultConnectionString);
            base.OnConfiguring(optionsBuilder);
        }

        /// <summary>
        /// DB 永続化（同期）
        /// </summary>
        /// <returns>変更数</returns>
        public int SaveChanges(string user)
        {
            var now = DateTime.Now;
            SetCreated(user, now);
            SetModified(user, now);
            return base.SaveChanges();
        }

        /// <summary>
        /// DB 永続化（非同期）
        /// </summary>
        /// <param name="user">更新ユーザ</param>
        /// <param name="cancellationToken">キャンセルトークン</param>
        /// <returns>変更数</returns>
        public async Task<int> SaveChangesAsync(string user, CancellationToken cancellationToken)
        {
            var now = DateTime.Now;
            SetCreated(user, now);
            SetModified(user, now);
            return await base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// 登録情報を更新する
        /// </summary>
        /// <param name="user">更新者</param>
        /// <param name="now">更新日</param>
        private void SetCreated(string user, DateTime now)
        {
            var entities = this.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added)
                .Select(e => e.Entity as ModelBase);

            foreach (var entity in entities)
            {
                entity.CreatedBy = user;
                entity.CreatedAt = now;
            }
        }

        /// <summary>
        /// 更新情報を設定する
        /// </summary>
        /// <param name="user">更新者</param>
        /// <param name="now">更新日</param>
        private void SetModified(string user, DateTime now)
        {
            var entities = this.ChangeTracker.Entries()
                .Where(e => (e.State == EntityState.Added || e.State == EntityState.Modified))
                .Select(e => e.Entity as ModelBase);

            foreach (var entity in entities)
            {
                entity.ModifiedBy = user;
                entity.ModifiedAt = now;
            }
        }
    }
}
