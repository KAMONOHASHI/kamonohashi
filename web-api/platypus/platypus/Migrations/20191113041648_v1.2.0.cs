using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Nssol.Platypus.Migrations
{
    public partial class v120 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotebookHistoryParentTrainingMaps",
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
                    table.PrimaryKey("PK_NotebookHistoryParentTrainingMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotebookHistoryParentTrainingMaps_NotebookHistories_Noteboo~",
                        column: x => x.NotebookHistoryId,
                        principalTable: "NotebookHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotebookHistoryParentTrainingMaps_TrainingHistories_ParentId",
                        column: x => x.ParentId,
                        principalTable: "TrainingHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotebookHistoryParentTrainingMaps_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotebookHistoryParentTrainingMaps_NotebookHistoryId",
                table: "NotebookHistoryParentTrainingMaps",
                column: "NotebookHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_NotebookHistoryParentTrainingMaps_ParentId",
                table: "NotebookHistoryParentTrainingMaps",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_NotebookHistoryParentTrainingMaps_TenantId_NotebookHistoryI~",
                table: "NotebookHistoryParentTrainingMaps",
                columns: new[] { "TenantId", "NotebookHistoryId", "ParentId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotebookHistoryParentTrainingMaps");
        }
    }
}
