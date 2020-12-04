using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Nssol.Platypus.Migrations
{
    public partial class v214 : Migration
    {
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Templates");

            migrationBuilder.DropTable(
                name: "TemplateTenantMaps");
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    TenantId = table.Column<long>(nullable: false),
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
                });

            migrationBuilder.CreateIndex(
                name: "IX_Templates_PreprocessContainerRegistryId",
                table: "Templates",
                column: "PreprocessContainerRegistryId");

            migrationBuilder.CreateIndex(
                name: "IX_Templates_TenantId",
                table: "Templates",
                column: "TenantId");

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
