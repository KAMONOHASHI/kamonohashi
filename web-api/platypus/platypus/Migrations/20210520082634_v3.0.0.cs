using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Nssol.Platypus.Infrastructure;

namespace Nssol.Platypus.Migrations
{
    public partial class v300 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JupyterLabVersion",
                table: "NotebookHistories",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsFlat",
                table: "DataSets",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "AquariumDatasets",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    TenantId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    LatestVersion = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AquariumDatasets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AquariumDatasets_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Templates",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Memo = table.Column<string>(nullable: true),
                    LatestVersion = table.Column<long>(nullable: false),
                    AccessLevel = table.Column<int>(nullable: false),
                    CreaterUserId = table.Column<long>(nullable: false),
                    CreaterTenantId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Templates_Tenants_CreaterTenantId",
                        column: x => x.CreaterTenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Templates_Users_CreaterUserId",
                        column: x => x.CreaterUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AquariumDatasetVersions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    TenantId = table.Column<long>(nullable: false),
                    Version = table.Column<long>(nullable: false),
                    AquariumDataSetId = table.Column<long>(nullable: false),
                    DataSetId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AquariumDatasetVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AquariumDatasetVersions_AquariumDatasets_AquariumDataSetId",
                        column: x => x.AquariumDataSetId,
                        principalTable: "AquariumDatasets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AquariumDatasetVersions_DataSets_DataSetId",
                        column: x => x.DataSetId,
                        principalTable: "DataSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AquariumDatasetVersions_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TemplateVersions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    TemplateId = table.Column<long>(nullable: false),
                    Version = table.Column<long>(nullable: false),
                    PreprocessEntryPoint = table.Column<string>(nullable: true),
                    PreprocessRepositoryGitId = table.Column<long>(nullable: true),
                    PreprocessRepositoryName = table.Column<string>(nullable: true),
                    PreprocessRepositoryOwner = table.Column<string>(nullable: true),
                    PreprocessRepositoryBranch = table.Column<string>(nullable: true),
                    PreprocessRepositoryCommitId = table.Column<string>(nullable: true),
                    PreprocessRepositoryToken = table.Column<string>(nullable: true),
                    PreprocessContainerRegistryId = table.Column<long>(nullable: true),
                    PreprocessContainerImage = table.Column<string>(nullable: true),
                    PreprocessContainerTag = table.Column<string>(nullable: true),
                    PreprocessContainerToken = table.Column<string>(nullable: true),
                    PreprocessCpu = table.Column<int>(nullable: false),
                    PreprocessMemory = table.Column<int>(nullable: false),
                    PreprocessGpu = table.Column<int>(nullable: false),
                    TrainingEntryPoint = table.Column<string>(nullable: false),
                    TrainingRepositoryGitId = table.Column<long>(nullable: false),
                    TrainingRepositoryName = table.Column<string>(nullable: false),
                    TrainingRepositoryOwner = table.Column<string>(nullable: false),
                    TrainingRepositoryBranch = table.Column<string>(nullable: false),
                    TrainingRepositoryCommitId = table.Column<string>(nullable: false),
                    TrainingRepositoryToken = table.Column<string>(nullable: true),
                    TrainingContainerRegistryId = table.Column<long>(nullable: false),
                    TrainingContainerImage = table.Column<string>(nullable: false),
                    TrainingContainerTag = table.Column<string>(nullable: false),
                    TrainingContainerToken = table.Column<string>(nullable: true),
                    TrainingCpu = table.Column<int>(nullable: false),
                    TrainingMemory = table.Column<int>(nullable: false),
                    TrainingGpu = table.Column<int>(nullable: false),
                    EvaluationEntryPoint = table.Column<string>(nullable: true),
                    EvaluationRepositoryGitId = table.Column<long>(nullable: true),
                    EvaluationRepositoryName = table.Column<string>(nullable: true),
                    EvaluationRepositoryOwner = table.Column<string>(nullable: true),
                    EvaluationRepositoryBranch = table.Column<string>(nullable: true),
                    EvaluationRepositoryCommitId = table.Column<string>(nullable: true),
                    EvaluationRepositoryToken = table.Column<string>(nullable: true),
                    EvaluationContainerRegistryId = table.Column<long>(nullable: true),
                    EvaluationContainerImage = table.Column<string>(nullable: true),
                    EvaluationContainerTag = table.Column<string>(nullable: true),
                    EvaluationContainerToken = table.Column<string>(nullable: true),
                    EvaluationCpu = table.Column<int>(nullable: false),
                    EvaluationMemory = table.Column<int>(nullable: false),
                    EvaluationGpu = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateVersions_Registries_EvaluationContainerRegistryId",
                        column: x => x.EvaluationContainerRegistryId,
                        principalTable: "Registries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TemplateVersions_Gits_EvaluationRepositoryGitId",
                        column: x => x.EvaluationRepositoryGitId,
                        principalTable: "Gits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TemplateVersions_Registries_PreprocessContainerRegistryId",
                        column: x => x.PreprocessContainerRegistryId,
                        principalTable: "Registries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TemplateVersions_Gits_PreprocessRepositoryGitId",
                        column: x => x.PreprocessRepositoryGitId,
                        principalTable: "Gits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TemplateVersions_Templates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TemplateVersions_Registries_TrainingContainerRegistryId",
                        column: x => x.TrainingContainerRegistryId,
                        principalTable: "Registries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TemplateVersions_Gits_TrainingRepositoryGitId",
                        column: x => x.TrainingRepositoryGitId,
                        principalTable: "Gits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExperimentPreprocesses",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    TenantId = table.Column<long>(nullable: false),
                    DataSetId = table.Column<long>(nullable: false),
                    DataSetVersionId = table.Column<long>(nullable: false),
                    TemplateId = table.Column<long>(nullable: false),
                    TemplateVersionId = table.Column<long>(nullable: false),
                    TrainingHistoryId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperimentPreprocesses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExperimentPreprocesses_AquariumDatasets_DataSetId",
                        column: x => x.DataSetId,
                        principalTable: "AquariumDatasets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExperimentPreprocesses_AquariumDatasetVersions_DataSetVersi~",
                        column: x => x.DataSetVersionId,
                        principalTable: "AquariumDatasetVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExperimentPreprocesses_Templates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExperimentPreprocesses_TemplateVersions_TemplateVersionId",
                        column: x => x.TemplateVersionId,
                        principalTable: "TemplateVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExperimentPreprocesses_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExperimentPreprocesses_TrainingHistories_TrainingHistoryId",
                        column: x => x.TrainingHistoryId,
                        principalTable: "TrainingHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Experiments",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    TenantId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    DataSetId = table.Column<long>(nullable: false),
                    DataSetVersionId = table.Column<long>(nullable: false),
                    TemplateId = table.Column<long>(nullable: false),
                    TemplateVersionId = table.Column<long>(nullable: false),
                    ExperimentPreprocessId = table.Column<long>(nullable: true),
                    TrainingHistoryId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experiments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Experiments_AquariumDatasets_DataSetId",
                        column: x => x.DataSetId,
                        principalTable: "AquariumDatasets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Experiments_AquariumDatasetVersions_DataSetVersionId",
                        column: x => x.DataSetVersionId,
                        principalTable: "AquariumDatasetVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Experiments_ExperimentPreprocesses_ExperimentPreprocessId",
                        column: x => x.ExperimentPreprocessId,
                        principalTable: "ExperimentPreprocesses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Experiments_Templates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Experiments_TemplateVersions_TemplateVersionId",
                        column: x => x.TemplateVersionId,
                        principalTable: "TemplateVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Experiments_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Experiments_TrainingHistories_TrainingHistoryId",
                        column: x => x.TrainingHistoryId,
                        principalTable: "TrainingHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AquariumDatasets_TenantId",
                table: "AquariumDatasets",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AquariumDatasetVersions_AquariumDataSetId",
                table: "AquariumDatasetVersions",
                column: "AquariumDataSetId");

            migrationBuilder.CreateIndex(
                name: "IX_AquariumDatasetVersions_DataSetId",
                table: "AquariumDatasetVersions",
                column: "DataSetId");

            migrationBuilder.CreateIndex(
                name: "IX_AquariumDatasetVersions_TenantId",
                table: "AquariumDatasetVersions",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentPreprocesses_DataSetId",
                table: "ExperimentPreprocesses",
                column: "DataSetId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentPreprocesses_DataSetVersionId",
                table: "ExperimentPreprocesses",
                column: "DataSetVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentPreprocesses_TemplateId",
                table: "ExperimentPreprocesses",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentPreprocesses_TemplateVersionId",
                table: "ExperimentPreprocesses",
                column: "TemplateVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentPreprocesses_TenantId",
                table: "ExperimentPreprocesses",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentPreprocesses_TrainingHistoryId",
                table: "ExperimentPreprocesses",
                column: "TrainingHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Experiments_DataSetId",
                table: "Experiments",
                column: "DataSetId");

            migrationBuilder.CreateIndex(
                name: "IX_Experiments_DataSetVersionId",
                table: "Experiments",
                column: "DataSetVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Experiments_ExperimentPreprocessId",
                table: "Experiments",
                column: "ExperimentPreprocessId");

            migrationBuilder.CreateIndex(
                name: "IX_Experiments_TemplateId",
                table: "Experiments",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Experiments_TemplateVersionId",
                table: "Experiments",
                column: "TemplateVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Experiments_TenantId",
                table: "Experiments",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Experiments_TrainingHistoryId",
                table: "Experiments",
                column: "TrainingHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Templates_CreaterTenantId",
                table: "Templates",
                column: "CreaterTenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Templates_CreaterUserId",
                table: "Templates",
                column: "CreaterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateVersions_EvaluationContainerRegistryId",
                table: "TemplateVersions",
                column: "EvaluationContainerRegistryId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateVersions_EvaluationRepositoryGitId",
                table: "TemplateVersions",
                column: "EvaluationRepositoryGitId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateVersions_PreprocessContainerRegistryId",
                table: "TemplateVersions",
                column: "PreprocessContainerRegistryId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateVersions_PreprocessRepositoryGitId",
                table: "TemplateVersions",
                column: "PreprocessRepositoryGitId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateVersions_TemplateId",
                table: "TemplateVersions",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateVersions_TrainingContainerRegistryId",
                table: "TemplateVersions",
                column: "TrainingContainerRegistryId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateVersions_TrainingRepositoryGitId",
                table: "TemplateVersions",
                column: "TrainingRepositoryGitId");

            migrationBuilder.Sql(@"UPDATE ""Registries"" SET ""RegistryUrl"" = 'https://index.docker.io/' WHERE ""Id"" = 1 AND ""ServiceType"" = 1;");

            // 共通変数
            string adminUser = ApplicationConst.DefaultFirstAdminUserName;
            DateTime now = DateTime.Now;

            // アクアリウムの初期権限のマッピングを追加
            // ユーザーロール : データセット、実験実行、実験履歴
            migrationBuilder.Sql($"INSERT INTO \"MenuRoleMaps\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"MenuCode\", \"RoleId\") SELECT nextval('\"MenuRoleMaps_Id_seq\"'), '{adminUser}', '{now}', '{adminUser}', '{now}', '{Logic.MenuLogic.AquariumDataSetMenu.Code.ToString()}', \"Id\" FROM \"Roles\" WHERE \"Name\" = 'users';");
            migrationBuilder.Sql($"INSERT INTO \"MenuRoleMaps\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"MenuCode\", \"RoleId\") SELECT nextval('\"MenuRoleMaps_Id_seq\"'), '{adminUser}', '{now}', '{adminUser}', '{now}', '{Logic.MenuLogic.ExperimentMenu.Code.ToString()}', \"Id\" FROM \"Roles\" WHERE \"Name\" = 'users';");
            migrationBuilder.Sql($"INSERT INTO \"MenuRoleMaps\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"MenuCode\", \"RoleId\") SELECT nextval('\"MenuRoleMaps_Id_seq\"'), '{adminUser}', '{now}', '{adminUser}', '{now}', '{Logic.MenuLogic.ExperimentHistoryMenu.Code.ToString()}', \"Id\" FROM \"Roles\" WHERE \"Name\" = 'users';");
            // リサーチャーロール : データセット、実験実行、実験履歴、テンプレート編集
            migrationBuilder.Sql($"INSERT INTO \"MenuRoleMaps\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"MenuCode\", \"RoleId\") SELECT nextval('\"MenuRoleMaps_Id_seq\"'), '{adminUser}', '{now}', '{adminUser}', '{now}', '{Logic.MenuLogic.AquariumDataSetMenu.Code.ToString()}', \"Id\" FROM \"Roles\" WHERE \"Name\" = 'researchers';");
            migrationBuilder.Sql($"INSERT INTO \"MenuRoleMaps\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"MenuCode\", \"RoleId\") SELECT nextval('\"MenuRoleMaps_Id_seq\"'), '{adminUser}', '{now}', '{adminUser}', '{now}', '{Logic.MenuLogic.ExperimentMenu.Code.ToString()}', \"Id\" FROM \"Roles\" WHERE \"Name\" = 'researchers';");
            migrationBuilder.Sql($"INSERT INTO \"MenuRoleMaps\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"MenuCode\", \"RoleId\") SELECT nextval('\"MenuRoleMaps_Id_seq\"'), '{adminUser}', '{now}', '{adminUser}', '{now}', '{Logic.MenuLogic.ExperimentHistoryMenu.Code.ToString()}', \"Id\" FROM \"Roles\" WHERE \"Name\" = 'researchers';");
            migrationBuilder.Sql($"INSERT INTO \"MenuRoleMaps\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"MenuCode\", \"RoleId\") SELECT nextval('\"MenuRoleMaps_Id_seq\"'), '{adminUser}', '{now}', '{adminUser}', '{now}', '{Logic.MenuLogic.TemplateMenu.Code.ToString()}', \"Id\" FROM \"Roles\" WHERE \"Name\" = 'researchers';");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Experiments");

            migrationBuilder.DropTable(
                name: "ExperimentPreprocesses");

            migrationBuilder.DropTable(
                name: "AquariumDatasetVersions");

            migrationBuilder.DropTable(
                name: "TemplateVersions");

            migrationBuilder.DropTable(
                name: "AquariumDatasets");

            migrationBuilder.DropTable(
                name: "Templates");

            migrationBuilder.DropColumn(
                name: "JupyterLabVersion",
                table: "NotebookHistories");

            migrationBuilder.DropColumn(
                name: "IsFlat",
                table: "DataSets");
        }
    }
}
