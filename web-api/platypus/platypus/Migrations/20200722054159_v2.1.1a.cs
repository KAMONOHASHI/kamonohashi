using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Nssol.Platypus.Migrations
{
    public partial class v211a : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotebookHistoryParentInferenceMaps",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    TenantId = table.Column<long>(nullable: false),
                    NotebookHistoryId = table.Column<long>(nullable: false),
                    ParentId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotebookHistoryParentInferenceMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotebookHistoryParentInferenceMaps_NotebookHistories_Notebo~",
                        column: x => x.NotebookHistoryId,
                        principalTable: "NotebookHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotebookHistoryParentInferenceMaps_InferenceHistories_Paren~",
                        column: x => x.ParentId,
                        principalTable: "InferenceHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotebookHistoryParentInferenceMaps_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotebookHistoryParentInferenceMaps_NotebookHistoryId",
                table: "NotebookHistoryParentInferenceMaps",
                column: "NotebookHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_NotebookHistoryParentInferenceMaps_ParentId",
                table: "NotebookHistoryParentInferenceMaps",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_NotebookHistoryParentInferenceMaps_TenantId_NotebookHistory~",
                table: "NotebookHistoryParentInferenceMaps",
                columns: new[] { "TenantId", "NotebookHistoryId", "ParentId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotebookHistoryParentInferenceMaps");
        }
    }
}
