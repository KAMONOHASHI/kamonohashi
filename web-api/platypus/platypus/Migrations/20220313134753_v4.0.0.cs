using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Nssol.Platypus.Migrations
{
    public partial class v400 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrainingSearchHistories",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    TenantId = table.Column<long>(nullable: false),
                    DisplayId = table.Column<long>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    IdLower = table.Column<long>(nullable: true),
                    IdUpper = table.Column<long>(nullable: true),
                    TrainingName = table.Column<string>(nullable: true),
                    TrainingNameOr = table.Column<bool>(nullable: true),
                    ParentName = table.Column<string>(nullable: true),
                    ParentNameOr = table.Column<bool>(nullable: true),
                    StartedAtLower = table.Column<string>(nullable: true),
                    StartedAtUpper = table.Column<string>(nullable: true),
                    StartedBy = table.Column<string>(nullable: true),
                    StartedByOr = table.Column<bool>(nullable: true),
                    DataSet = table.Column<string>(nullable: true),
                    DataSetOr = table.Column<bool>(nullable: true),
                    Memo = table.Column<string>(nullable: true),
                    MemoOr = table.Column<bool>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    StatusOr = table.Column<bool>(nullable: true),
                    EntryPoint = table.Column<string>(nullable: true),
                    EntryPointOr = table.Column<bool>(nullable: true),
                    Tags = table.Column<string>(nullable: true),
                    TagsOr = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingSearchHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingSearchHistories_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrainingSearchHistories_TenantId",
                table: "TrainingSearchHistories",
                column: "TenantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingSearchHistories");
        }
    }
}
