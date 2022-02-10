using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using System;

namespace Nssol.Platypus.Migrations
{
    public partial class v310 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "JobStartedAt",
                table: "NotebookHistories",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ResourceJobs",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    NodeName = table.Column<string>(nullable: false),
                    NodeCpu = table.Column<int>(nullable: false),
                    NodeMemory = table.Column<int>(nullable: false),
                    NodeGpu = table.Column<int>(nullable: false),
                    TenantId = table.Column<long>(nullable: false),
                    TenantName = table.Column<string>(nullable: false),
                    JobCreatedAt = table.Column<DateTime>(nullable: false),
                    JobStartedAt = table.Column<DateTime>(nullable: true),
                    JobCompletedAt = table.Column<DateTime>(nullable: false),
                    ContainerName = table.Column<string>(nullable: false),
                    Cpu = table.Column<int>(nullable: false),
                    Memory = table.Column<int>(nullable: false),
                    Gpu = table.Column<int>(nullable: false),
                    Status = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceJobs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResourceSamples",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    SampledAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceSamples", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ResourceNodes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    SampleId = table.Column<long>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Cpu = table.Column<int>(nullable: false),
                    Memory = table.Column<int>(nullable: false),
                    Gpu = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceNodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResourceNodes_ResourceSamples_SampleId",
                        column: x => x.SampleId,
                        principalTable: "ResourceSamples",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResourceContainers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    NodeId = table.Column<long>(nullable: false),
                    TenantId = table.Column<long>(nullable: false),
                    TenantName = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Cpu = table.Column<int>(nullable: false),
                    Memory = table.Column<int>(nullable: false),
                    Gpu = table.Column<int>(nullable: false),
                    Status = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceContainers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResourceContainers_ResourceNodes_NodeId",
                        column: x => x.NodeId,
                        principalTable: "ResourceNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResourceContainers_NodeId",
                table: "ResourceContainers",
                column: "NodeId");

            migrationBuilder.CreateIndex(
                name: "IX_ResourceNodes_SampleId",
                table: "ResourceNodes",
                column: "SampleId");

            migrationBuilder.Sql(@"UPDATE ""NotebookHistories"" SET ""JobStartedAt"" = ""StartedAt"";");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResourceContainers");

            migrationBuilder.DropTable(
                name: "ResourceJobs");

            migrationBuilder.DropTable(
                name: "ResourceNodes");

            migrationBuilder.DropTable(
                name: "ResourceSamples");

            migrationBuilder.DropColumn(
                name: "JobStartedAt",
                table: "NotebookHistories");
        }
    }
}
