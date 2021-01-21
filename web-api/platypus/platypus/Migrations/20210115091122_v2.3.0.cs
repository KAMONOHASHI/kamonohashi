using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Nssol.Platypus.Migrations
{
    public partial class v230 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    DisplayId = table.Column<long>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Memo = table.Column<string>(nullable: true),
                    Version = table.Column<long>(nullable: true),
                    GroupId = table.Column<long>(nullable: true),
                    AccessLevel = table.Column<int>(nullable: false),
                    PreprocessEntryPoint = table.Column<string>(nullable: true),
                    PreprocessRepositoryGitId = table.Column<long>(nullable: true),
                    PreprocessRepositoryName = table.Column<string>(nullable: true),
                    PreprocessRepositoryOwner = table.Column<string>(nullable: true),
                    PreprocessRepositoryBranch = table.Column<string>(nullable: true),
                    PreprocessRepositoryCommitId = table.Column<string>(nullable: true),
                    PreprocessContainerRegistryId = table.Column<long>(nullable: true),
                    PreprocessContainerImage = table.Column<string>(nullable: true),
                    PreprocessContainerTag = table.Column<string>(nullable: true),
                    PreprocessCpu = table.Column<int>(nullable: false),
                    PreprocessMemory = table.Column<int>(nullable: false),
                    PreprocessGpu = table.Column<int>(nullable: false),
                    TrainingEntryPoint = table.Column<string>(nullable: true),
                    TrainingRepositoryGitId = table.Column<long>(nullable: true),
                    TrainingRepositoryName = table.Column<string>(nullable: true),
                    TrainingRepositoryOwner = table.Column<string>(nullable: true),
                    TrainingRepositoryBranch = table.Column<string>(nullable: true),
                    TrainingRepositoryCommitId = table.Column<string>(nullable: true),
                    TrainingContainerRegistryId = table.Column<long>(nullable: true),
                    TrainingContainerImage = table.Column<string>(nullable: true),
                    TrainingContainerTag = table.Column<string>(nullable: true),
                    TrainingCpu = table.Column<int>(nullable: false),
                    TrainingMemory = table.Column<int>(nullable: false),
                    TrainingGpu = table.Column<int>(nullable: false)
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
                name: "TemplateTenantMaps",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
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
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    TenantId = table.Column<long>(nullable: false),
                    DataSetId = table.Column<long>(nullable: false),
                    InputDataId = table.Column<long>(nullable: false),
                    TemplateId = table.Column<long>(nullable: true),
                    StartedAt = table.Column<DateTime>(nullable: true),
                    CompletedAt = table.Column<DateTime>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    Options = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperimentHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExperimentHistories_AquariumDatasetVersions_DataSetId",
                        column: x => x.DataSetId,
                        principalTable: "AquariumDatasetVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExperimentHistories_Data_InputDataId",
                        column: x => x.InputDataId,
                        principalTable: "Data",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExperimentHistories_Templates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExperimentHistories_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExperimentPreprocessHistories",
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
                    TemplateId = table.Column<long>(nullable: true),
                    StartedAt = table.Column<DateTime>(nullable: true),
                    CompletedAt = table.Column<DateTime>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    Options = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperimentPreprocessHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExperimentPreprocessHistories_AquariumDatasetVersions_DataS~",
                        column: x => x.DataSetId,
                        principalTable: "AquariumDatasetVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "ExperimentTensorBoardContainers",
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
                    Host = table.Column<string>(nullable: true),
                    PortNo = table.Column<int>(nullable: true),
                    ExperimentHistoryId = table.Column<long>(nullable: false),
                    Status = table.Column<string>(nullable: false),
                    StartedAt = table.Column<DateTime>(nullable: false),
                    ExpiresIn = table.Column<int>(nullable: false)
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

            migrationBuilder.CreateTable(
                name: "ExperimentPreprocessHistoryOutput",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    TenantId = table.Column<long>(nullable: false),
                    ExperimentHistoryId = table.Column<long>(nullable: false),
                    OutputDataId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperimentPreprocessHistoryOutput", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExperimentPreprocessHistoryOutput_ExperimentPreprocessHisto~",
                        column: x => x.ExperimentHistoryId,
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
                name: "IX_ExperimentHistories_DataSetId",
                table: "ExperimentHistories",
                column: "DataSetId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentHistories_InputDataId",
                table: "ExperimentHistories",
                column: "InputDataId");

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
                name: "IX_ExperimentPreprocessHistories_TemplateId",
                table: "ExperimentPreprocessHistories",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentPreprocessHistories_TenantId",
                table: "ExperimentPreprocessHistories",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentPreprocessHistoryOutput_ExperimentHistoryId",
                table: "ExperimentPreprocessHistoryOutput",
                column: "ExperimentHistoryId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExperimentPreprocessHistoryOutput");

            migrationBuilder.DropTable(
                name: "ExperimentTensorBoardContainers");

            migrationBuilder.DropTable(
                name: "TemplateTenantMaps");

            migrationBuilder.DropTable(
                name: "ExperimentPreprocessHistories");

            migrationBuilder.DropTable(
                name: "ExperimentHistories");

            migrationBuilder.DropTable(
                name: "AquariumDatasetVersions");

            migrationBuilder.DropTable(
                name: "Templates");

            migrationBuilder.DropTable(
                name: "AquariumDatasets");
        }
    }
}
