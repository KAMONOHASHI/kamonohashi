using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Nssol.Platypus.Migrations
{
    public partial class v230d : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Templates2",
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
                    table.PrimaryKey("PK_Templates2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Templates2_Tenants_CreaterTenantId",
                        column: x => x.CreaterTenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Templates2_Users_CreaterUserId",
                        column: x => x.CreaterUserId,
                        principalTable: "Users",
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
                    PreprocessContainerRegistryId = table.Column<long>(nullable: true),
                    PreprocessContainerImage = table.Column<string>(nullable: true),
                    PreprocessContainerTag = table.Column<string>(nullable: true),
                    PreprocessCpu = table.Column<int>(nullable: false),
                    PreprocessMemory = table.Column<int>(nullable: false),
                    PreprocessGpu = table.Column<int>(nullable: false),
                    TrainingEntryPoint = table.Column<string>(nullable: false),
                    TrainingRepositoryGitId = table.Column<long>(nullable: false),
                    TrainingRepositoryName = table.Column<string>(nullable: false),
                    TrainingRepositoryOwner = table.Column<string>(nullable: false),
                    TrainingRepositoryBranch = table.Column<string>(nullable: false),
                    TrainingRepositoryCommitId = table.Column<string>(nullable: false),
                    TrainingContainerRegistryId = table.Column<long>(nullable: false),
                    TrainingContainerImage = table.Column<string>(nullable: false),
                    TrainingContainerTag = table.Column<string>(nullable: false),
                    TrainingCpu = table.Column<int>(nullable: false),
                    TrainingMemory = table.Column<int>(nullable: false),
                    TrainingGpu = table.Column<int>(nullable: false),
                    EvaluationEntryPoint = table.Column<string>(nullable: true),
                    EvaluationRepositoryGitId = table.Column<long>(nullable: true),
                    EvaluationRepositoryName = table.Column<string>(nullable: true),
                    EvaluationRepositoryOwner = table.Column<string>(nullable: true),
                    EvaluationRepositoryBranch = table.Column<string>(nullable: true),
                    EvaluationRepositoryCommitId = table.Column<string>(nullable: true),
                    EvaluationContainerRegistryId = table.Column<long>(nullable: true),
                    EvaluationContainerImage = table.Column<string>(nullable: true),
                    EvaluationContainerTag = table.Column<string>(nullable: true),
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
                        name: "FK_TemplateVersions_Templates2_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates2",
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

            migrationBuilder.CreateIndex(
                name: "IX_Templates2_CreaterTenantId",
                table: "Templates2",
                column: "CreaterTenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Templates2_CreaterUserId",
                table: "Templates2",
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TemplateVersions");

            migrationBuilder.DropTable(
                name: "Templates2");
        }
    }
}
