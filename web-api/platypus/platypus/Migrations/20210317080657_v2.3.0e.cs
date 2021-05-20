using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Nssol.Platypus.Migrations
{
    public partial class v230e : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExperimentPreprocessHistoryOutput");

            migrationBuilder.DropTable(
                name: "ExperimentTensorBoardContainers");

            migrationBuilder.DropTable(
                name: "TemplateTenantMaps");

            migrationBuilder.DropTable(
                name: "ExperimentHistories");

            migrationBuilder.DropTable(
                name: "ExperimentPreprocessHistories");

            migrationBuilder.DropTable(
                name: "Templates");

            migrationBuilder.CreateTable(
                name: "ExperimentPreprocess",
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
                    table.PrimaryKey("PK_ExperimentPreprocess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExperimentPreprocess_AquariumDatasets_DataSetId",
                        column: x => x.DataSetId,
                        principalTable: "AquariumDatasets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExperimentPreprocess_AquariumDatasetVersions_DataSetVersion~",
                        column: x => x.DataSetVersionId,
                        principalTable: "AquariumDatasetVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExperimentPreprocess_Templates2_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates2",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExperimentPreprocess_TemplateVersions_TemplateVersionId",
                        column: x => x.TemplateVersionId,
                        principalTable: "TemplateVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExperimentPreprocess_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExperimentPreprocess_TrainingHistories_TrainingHistoryId",
                        column: x => x.TrainingHistoryId,
                        principalTable: "TrainingHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Experiment",
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
                    table.PrimaryKey("PK_Experiment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Experiment_AquariumDatasets_DataSetId",
                        column: x => x.DataSetId,
                        principalTable: "AquariumDatasets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Experiment_AquariumDatasetVersions_DataSetVersionId",
                        column: x => x.DataSetVersionId,
                        principalTable: "AquariumDatasetVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Experiment_ExperimentPreprocess_ExperimentPreprocessId",
                        column: x => x.ExperimentPreprocessId,
                        principalTable: "ExperimentPreprocess",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Experiment_Templates2_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates2",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Experiment_TemplateVersions_TemplateVersionId",
                        column: x => x.TemplateVersionId,
                        principalTable: "TemplateVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Experiment_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Experiment_TrainingHistories_TrainingHistoryId",
                        column: x => x.TrainingHistoryId,
                        principalTable: "TrainingHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Experiment_DataSetId",
                table: "Experiment",
                column: "DataSetId");

            migrationBuilder.CreateIndex(
                name: "IX_Experiment_DataSetVersionId",
                table: "Experiment",
                column: "DataSetVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Experiment_ExperimentPreprocessId",
                table: "Experiment",
                column: "ExperimentPreprocessId");

            migrationBuilder.CreateIndex(
                name: "IX_Experiment_TemplateId",
                table: "Experiment",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Experiment_TemplateVersionId",
                table: "Experiment",
                column: "TemplateVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_Experiment_TenantId",
                table: "Experiment",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Experiment_TrainingHistoryId",
                table: "Experiment",
                column: "TrainingHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentPreprocess_DataSetId",
                table: "ExperimentPreprocess",
                column: "DataSetId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentPreprocess_DataSetVersionId",
                table: "ExperimentPreprocess",
                column: "DataSetVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentPreprocess_TemplateId",
                table: "ExperimentPreprocess",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentPreprocess_TemplateVersionId",
                table: "ExperimentPreprocess",
                column: "TemplateVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentPreprocess_TenantId",
                table: "ExperimentPreprocess",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentPreprocess_TrainingHistoryId",
                table: "ExperimentPreprocess",
                column: "TrainingHistoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Experiment");

            migrationBuilder.DropTable(
                name: "ExperimentPreprocess");

            migrationBuilder.CreateTable(
                name: "Templates",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    AccessLevel = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    DisplayId = table.Column<long>(nullable: true),
                    GroupId = table.Column<long>(nullable: true),
                    Memo = table.Column<string>(nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    PreprocessContainerImage = table.Column<string>(nullable: true),
                    PreprocessContainerRegistryId = table.Column<long>(nullable: true),
                    PreprocessContainerTag = table.Column<string>(nullable: true),
                    PreprocessCpu = table.Column<int>(nullable: false),
                    PreprocessEntryPoint = table.Column<string>(nullable: true),
                    PreprocessGpu = table.Column<int>(nullable: false),
                    PreprocessMemory = table.Column<int>(nullable: false),
                    PreprocessRepositoryBranch = table.Column<string>(nullable: true),
                    PreprocessRepositoryCommitId = table.Column<string>(nullable: true),
                    PreprocessRepositoryGitId = table.Column<long>(nullable: true),
                    PreprocessRepositoryName = table.Column<string>(nullable: true),
                    PreprocessRepositoryOwner = table.Column<string>(nullable: true),
                    TrainingContainerImage = table.Column<string>(nullable: true),
                    TrainingContainerRegistryId = table.Column<long>(nullable: true),
                    TrainingContainerTag = table.Column<string>(nullable: true),
                    TrainingCpu = table.Column<int>(nullable: false),
                    TrainingEntryPoint = table.Column<string>(nullable: true),
                    TrainingGpu = table.Column<int>(nullable: false),
                    TrainingMemory = table.Column<int>(nullable: false),
                    TrainingRepositoryBranch = table.Column<string>(nullable: true),
                    TrainingRepositoryCommitId = table.Column<string>(nullable: true),
                    TrainingRepositoryGitId = table.Column<long>(nullable: true),
                    TrainingRepositoryName = table.Column<string>(nullable: true),
                    TrainingRepositoryOwner = table.Column<string>(nullable: true),
                    Version = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Templates_Registries_PreprocessContainerRegistryId",
                        column: x => x.PreprocessContainerRegistryId,
                        principalTable: "Registries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Templates_Registries_TrainingContainerRegistryId",
                        column: x => x.TrainingContainerRegistryId,
                        principalTable: "Registries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExperimentPreprocessHistories",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CompletedAt = table.Column<DateTime>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    DataSetId = table.Column<long>(nullable: false),
                    DataSetVersionId = table.Column<long>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    Options = table.Column<string>(nullable: true),
                    OutputDataSetId = table.Column<long>(nullable: true),
                    StartedAt = table.Column<DateTime>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    TemplateId = table.Column<long>(nullable: true),
                    TenantId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperimentPreprocessHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExperimentPreprocessHistories_AquariumDatasets_DataSetId",
                        column: x => x.DataSetId,
                        principalTable: "AquariumDatasets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExperimentPreprocessHistories_AquariumDatasetVersions_DataS~",
                        column: x => x.DataSetVersionId,
                        principalTable: "AquariumDatasetVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExperimentPreprocessHistories_DataSets_OutputDataSetId",
                        column: x => x.OutputDataSetId,
                        principalTable: "DataSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExperimentPreprocessHistories_Templates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExperimentPreprocessHistories_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TemplateTenantMaps",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    TemplateId = table.Column<long>(nullable: false),
                    TenantId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateTenantMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateTenantMaps_Templates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TemplateTenantMaps_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExperimentHistories",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CompletedAt = table.Column<DateTime>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    DataSetId = table.Column<long>(nullable: false),
                    DataSetVersionId = table.Column<long>(nullable: false),
                    ExperimentPreprocessHistoryId = table.Column<long>(nullable: true),
                    InputDataSetId = table.Column<long>(nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Options = table.Column<string>(nullable: true),
                    StartedAt = table.Column<DateTime>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    TemplateId = table.Column<long>(nullable: false),
                    TenantId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperimentHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExperimentHistories_AquariumDatasets_DataSetId",
                        column: x => x.DataSetId,
                        principalTable: "AquariumDatasets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExperimentHistories_AquariumDatasetVersions_DataSetVersionId",
                        column: x => x.DataSetVersionId,
                        principalTable: "AquariumDatasetVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExperimentHistories_ExperimentPreprocessHistories_Experimen~",
                        column: x => x.ExperimentPreprocessHistoryId,
                        principalTable: "ExperimentPreprocessHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExperimentHistories_DataSets_InputDataSetId",
                        column: x => x.InputDataSetId,
                        principalTable: "DataSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExperimentHistories_Templates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExperimentHistories_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExperimentPreprocessHistoryOutput",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    ExperimentPreprocessedHistoryId = table.Column<long>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    OutputDataId = table.Column<long>(nullable: false),
                    TenantId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperimentPreprocessHistoryOutput", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExperimentPreprocessHistoryOutput_ExperimentPreprocessHisto~",
                        column: x => x.ExperimentPreprocessedHistoryId,
                        principalTable: "ExperimentPreprocessHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExperimentPreprocessHistoryOutput_Data_OutputDataId",
                        column: x => x.OutputDataId,
                        principalTable: "Data",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExperimentPreprocessHistoryOutput_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExperimentTensorBoardContainers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: false),
                    ExperimentHistoryId = table.Column<long>(nullable: false),
                    ExpiresIn = table.Column<int>(nullable: false),
                    Host = table.Column<string>(nullable: true),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    PortNo = table.Column<int>(nullable: true),
                    StartedAt = table.Column<DateTime>(nullable: false),
                    Status = table.Column<string>(nullable: false),
                    TenantId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperimentTensorBoardContainers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExperimentTensorBoardContainers_ExperimentHistories_Experim~",
                        column: x => x.ExperimentHistoryId,
                        principalTable: "ExperimentHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExperimentTensorBoardContainers_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentHistories_DataSetId",
                table: "ExperimentHistories",
                column: "DataSetId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentHistories_DataSetVersionId",
                table: "ExperimentHistories",
                column: "DataSetVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentHistories_ExperimentPreprocessHistoryId",
                table: "ExperimentHistories",
                column: "ExperimentPreprocessHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentHistories_InputDataSetId",
                table: "ExperimentHistories",
                column: "InputDataSetId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentHistories_TemplateId",
                table: "ExperimentHistories",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentHistories_TenantId",
                table: "ExperimentHistories",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentPreprocessHistories_DataSetId",
                table: "ExperimentPreprocessHistories",
                column: "DataSetId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentPreprocessHistories_DataSetVersionId",
                table: "ExperimentPreprocessHistories",
                column: "DataSetVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentPreprocessHistories_OutputDataSetId",
                table: "ExperimentPreprocessHistories",
                column: "OutputDataSetId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentPreprocessHistories_TemplateId",
                table: "ExperimentPreprocessHistories",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentPreprocessHistories_TenantId",
                table: "ExperimentPreprocessHistories",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentPreprocessHistoryOutput_ExperimentPreprocessedHis~",
                table: "ExperimentPreprocessHistoryOutput",
                column: "ExperimentPreprocessedHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentPreprocessHistoryOutput_OutputDataId",
                table: "ExperimentPreprocessHistoryOutput",
                column: "OutputDataId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentPreprocessHistoryOutput_TenantId",
                table: "ExperimentPreprocessHistoryOutput",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentTensorBoardContainers_ExperimentHistoryId",
                table: "ExperimentTensorBoardContainers",
                column: "ExperimentHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentTensorBoardContainers_TenantId",
                table: "ExperimentTensorBoardContainers",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Templates_PreprocessContainerRegistryId",
                table: "Templates",
                column: "PreprocessContainerRegistryId");

            migrationBuilder.CreateIndex(
                name: "IX_Templates_TrainingContainerRegistryId",
                table: "Templates",
                column: "TrainingContainerRegistryId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateTenantMaps_TenantId",
                table: "TemplateTenantMaps",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_TemplateTenantMaps_TemplateId_TenantId",
                table: "TemplateTenantMaps",
                columns: new[] { "TemplateId", "TenantId" },
                unique: true);
        }
    }
}
