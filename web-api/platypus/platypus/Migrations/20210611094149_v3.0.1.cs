using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Nssol.Platypus.Migrations
{
    public partial class v301 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AquariumEvaluations",
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
                    ExperimentId = table.Column<long>(nullable: false),
                    TrainingHistoryId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AquariumEvaluations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AquariumEvaluations_AquariumDatasets_DataSetId",
                        column: x => x.DataSetId,
                        principalTable: "AquariumDatasets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AquariumEvaluations_AquariumDatasetVersions_DataSetVersionId",
                        column: x => x.DataSetVersionId,
                        principalTable: "AquariumDatasetVersions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AquariumEvaluations_Experiments_ExperimentId",
                        column: x => x.ExperimentId,
                        principalTable: "Experiments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AquariumEvaluations_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AquariumEvaluations_TrainingHistories_TrainingHistoryId",
                        column: x => x.TrainingHistoryId,
                        principalTable: "TrainingHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AquariumEvaluations_DataSetId",
                table: "AquariumEvaluations",
                column: "DataSetId");

            migrationBuilder.CreateIndex(
                name: "IX_AquariumEvaluations_DataSetVersionId",
                table: "AquariumEvaluations",
                column: "DataSetVersionId");

            migrationBuilder.CreateIndex(
                name: "IX_AquariumEvaluations_ExperimentId",
                table: "AquariumEvaluations",
                column: "ExperimentId");

            migrationBuilder.CreateIndex(
                name: "IX_AquariumEvaluations_TenantId",
                table: "AquariumEvaluations",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_AquariumEvaluations_TrainingHistoryId",
                table: "AquariumEvaluations",
                column: "TrainingHistoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AquariumEvaluations");
        }
    }
}
