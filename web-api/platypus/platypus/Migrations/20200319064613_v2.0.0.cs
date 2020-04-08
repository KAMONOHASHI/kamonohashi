using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Nssol.Platypus.Infrastructure;
using System;

namespace Nssol.Platypus.Migrations
{
    public partial class v200 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InferenceHistoryParentMaps",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    TenantId = table.Column<long>(nullable: false),
                    InferenceHistoryId = table.Column<long>(nullable: false),
                    ParentId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InferenceHistoryParentMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InferenceHistoryParentMaps_InferenceHistories_InferenceHist~",
                        column: x => x.InferenceHistoryId,
                        principalTable: "InferenceHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InferenceHistoryParentMaps_TrainingHistories_ParentId",
                        column: x => x.ParentId,
                        principalTable: "TrainingHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InferenceHistoryParentMaps_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrainingHistoryParentMaps",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    TenantId = table.Column<long>(nullable: false),
                    TrainingHistoryId = table.Column<long>(nullable: false),
                    ParentId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingHistoryParentMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingHistoryParentMaps_TrainingHistories_ParentId",
                        column: x => x.ParentId,
                        principalTable: "TrainingHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainingHistoryParentMaps_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrainingHistoryParentMaps_TrainingHistories_TrainingHistory~",
                        column: x => x.TrainingHistoryId,
                        principalTable: "TrainingHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InferenceHistoryParentMaps_InferenceHistoryId",
                table: "InferenceHistoryParentMaps",
                column: "InferenceHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_InferenceHistoryParentMaps_ParentId",
                table: "InferenceHistoryParentMaps",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_InferenceHistoryParentMaps_TenantId_InferenceHistoryId_Pare~",
                table: "InferenceHistoryParentMaps",
                columns: new[] { "TenantId", "InferenceHistoryId", "ParentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrainingHistoryParentMaps_ParentId",
                table: "TrainingHistoryParentMaps",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingHistoryParentMaps_TrainingHistoryId",
                table: "TrainingHistoryParentMaps",
                column: "TrainingHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingHistoryParentMaps_TenantId_TrainingHistoryId_Parent~",
                table: "TrainingHistoryParentMaps",
                columns: new[] { "TenantId", "TrainingHistoryId", "ParentId" },
                unique: true);

            // 共通変数
            string adminUser = ApplicationConst.DefaultFirstAdminUserName;
            DateTime now = DateTime.Now;

            // TrainingHistoriesからTrainingHistoryParentMapsにParentIdを移行する。
            migrationBuilder.Sql($"INSERT INTO \"TrainingHistoryParentMaps\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"TenantId\", \"TrainingHistoryId\", \"ParentId\") SELECT nextval('\"TrainingHistoryParentMaps_Id_seq\"'), '{adminUser}', '{now}', '{adminUser}', '{now}', \"TenantId\", \"Id\", \"ParentId\" FROM \"TrainingHistories\" WHERE \"ParentId\" IS NOT NULL;");

            // InferenceHistoriesからInferenceHistoryParentMapsにParentIdを移行する。
            migrationBuilder.Sql($"INSERT INTO \"InferenceHistoryParentMaps\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"TenantId\", \"InferenceHistoryId\", \"ParentId\") SELECT nextval('\"InferenceHistoryParentMaps_Id_seq\"'), '{adminUser}', '{now}', '{adminUser}', '{now}', \"TenantId\", \"Id\", \"ParentId\" FROM \"InferenceHistories\" WHERE \"ParentId\" IS NOT NULL;");

            migrationBuilder.DropForeignKey(
                name: "FK_InferenceHistories_TrainingHistories_ParentId",
                table: "InferenceHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainingHistories_TrainingHistories_ParentId",
                table: "TrainingHistories");

            migrationBuilder.DropIndex(
                name: "IX_TrainingHistories_ParentId",
                table: "TrainingHistories");

            migrationBuilder.DropIndex(
                name: "IX_InferenceHistories_ParentId",
                table: "InferenceHistories");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "TrainingHistories");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "InferenceHistories");

            // 開放ポート番号保持用カラム
            migrationBuilder.AddColumn<string>(
                name: "Ports",
                table: "TrainingHistories",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ParentId",
                table: "TrainingHistories",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ParentId",
                table: "InferenceHistories",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrainingHistories_ParentId",
                table: "TrainingHistories",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_InferenceHistories_ParentId",
                table: "InferenceHistories",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_InferenceHistories_TrainingHistories_ParentId",
                table: "InferenceHistories",
                column: "ParentId",
                principalTable: "TrainingHistories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingHistories_TrainingHistories_ParentId",
                table: "TrainingHistories",
                column: "ParentId",
                principalTable: "TrainingHistories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
            
            // TrainingHistoryParentMapsからTrainingHistoriesにParentIdを移行する。
            migrationBuilder.Sql($"UPDATE \"TrainingHistories\" AS Histories SET \"ParentId\" = ParentMaps.\"ParentId\" FROM \"TrainingHistoryParentMaps\" AS ParentMaps WHERE Histories.\"Id\" = ParentMaps.\"TrainingHistoryId\";");

            // InferenceHistoryParentMapsからInferenceHistoriesにParentIdを移行する。
            migrationBuilder.Sql($"UPDATE \"InferenceHistories\" AS Histories SET \"ParentId\" = ParentMaps.\"ParentId\" FROM \"InferenceHistoryParentMaps\" AS ParentMaps WHERE Histories.\"Id\" = ParentMaps.\"InferenceHistoryId\";");

            migrationBuilder.DropTable(
                name: "InferenceHistoryParentMaps");

            migrationBuilder.DropTable(
                name: "TrainingHistoryParentMaps");

            migrationBuilder.DropColumn(
                name: "Ports",
                table: "TrainingHistories");
        }
    }
}
