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
                    DataSetId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AquariumDatasetVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AquariumDatasetVersions_AquariumDatasets_DataSetId",
                        column: x => x.DataSetId,
                        principalTable: "AquariumDatasets",
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
                        name: "FK_ExperimentHistories_DataSets_DataSetId",
                        column: x => x.DataSetId,
                        principalTable: "DataSets",
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
                name: "AquariumDatasetVersionEntries",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    TenantId = table.Column<long>(nullable: false),
                    DataSetVersionId = table.Column<long>(nullable: false),
                    DataId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AquariumDatasetVersionEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AquariumDatasetVersionEntries_Data_DataId",
                        column: x => x.DataId,
                        principalTable: "Data",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AquariumDatasetVersionEntries_AquariumDatasetVersions_DataS~",
                        column: x => x.DataSetVersionId,
                        principalTable: "AquariumDatasetVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AquariumDatasetVersionEntries_Tenants_TenantId",
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
                name: "IX_AquariumDatasetVersionEntries_DataId",
                table: "AquariumDatasetVersionEntries",
                column: "DataId");

            migrationBuilder.CreateIndex(
                name: "IX_AquariumDatasetVersionEntries_DataSetVersionId",
                table: "AquariumDatasetVersionEntries",
                column: "DataSetVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_AquariumDatasetVersionEntries_TenantId",
                table: "AquariumDatasetVersionEntries",
                column: "TenantId");

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
                name: "IX_ExperimentHistories_TemplateId",
                table: "ExperimentHistories",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperimentHistories_TenantId",
                table: "ExperimentHistories",
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
                name: "AquariumDatasetVersionEntries");

            migrationBuilder.DropTable(
                name: "ExperimentHistories");

            migrationBuilder.DropTable(
                name: "TemplateTenantMaps");

            migrationBuilder.DropTable(
                name: "AquariumDatasetVersions");

            migrationBuilder.DropTable(
                name: "Templates");

            migrationBuilder.DropTable(
                name: "AquariumDatasets");
        }
    }
}
