﻿using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System;

namespace Nssol.Platypus.Migrations
{
    public partial class v212 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InferenceHistoryParentInferenceMaps",
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
                    table.PrimaryKey("PK_InferenceHistoryParentInferenceMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InferenceHistoryParentInferenceMaps_InferenceHistories_Infe~",
                        column: x => x.InferenceHistoryId,
                        principalTable: "InferenceHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InferenceHistoryParentInferenceMaps_InferenceHistories_Pare~",
                        column: x => x.ParentId,
                        principalTable: "InferenceHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InferenceHistoryParentInferenceMaps_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InferenceHistoryParentInferenceMaps_InferenceHistoryId",
                table: "InferenceHistoryParentInferenceMaps",
                column: "InferenceHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_InferenceHistoryParentInferenceMaps_ParentId",
                table: "InferenceHistoryParentInferenceMaps",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_InferenceHistoryParentInferenceMaps_TenantId_InferenceHisto~",
                table: "InferenceHistoryParentInferenceMaps",
                columns: new[] { "TenantId", "InferenceHistoryId", "ParentId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InferenceHistoryParentInferenceMaps");
        }
    }
}
