using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Nssol.Platypus.Infrastructure;

namespace Nssol.Platypus.Migrations
{
    public partial class v110 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotebookHistories",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    TenantId = table.Column<long>(nullable: false),
                    DisplayId = table.Column<long>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    DataSetId = table.Column<long>(nullable: true),
                    ModelGitId = table.Column<long>(nullable: true),
                    ModelRepository = table.Column<string>(nullable: true),
                    ModelRepositoryOwner = table.Column<string>(nullable: true),
                    ModelBranch = table.Column<string>(nullable: true),
                    ModelCommitId = table.Column<string>(nullable: true),
                    ContainerRegistryId = table.Column<long>(nullable: true),
                    ContainerImage = table.Column<string>(nullable: false),
                    ContainerTag = table.Column<string>(nullable: false),
                    Options = table.Column<string>(nullable: true),
                    Cpu = table.Column<int>(nullable: false),
                    Memory = table.Column<int>(nullable: false),
                    Gpu = table.Column<int>(nullable: false),
                    Partition = table.Column<string>(nullable: true),
                    Configuration = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: false),
                    Node = table.Column<string>(nullable: true),
                    StartedAt = table.Column<DateTime>(nullable: true),
                    CompletedAt = table.Column<DateTime>(nullable: true),
                    Memo = table.Column<string>(nullable: true),
                    Favorite = table.Column<bool>(nullable: false),
                    ExpiresIn = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotebookHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotebookHistories_Registries_ContainerRegistryId",
                        column: x => x.ContainerRegistryId,
                        principalTable: "Registries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotebookHistories_DataSets_DataSetId",
                        column: x => x.DataSetId,
                        principalTable: "DataSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotebookHistories_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotebookHistories_ContainerRegistryId",
                table: "NotebookHistories",
                column: "ContainerRegistryId");

            migrationBuilder.CreateIndex(
                name: "IX_NotebookHistories_DataSetId",
                table: "NotebookHistories",
                column: "DataSetId");

            migrationBuilder.CreateIndex(
                name: "IX_NotebookHistories_TenantId",
                table: "NotebookHistories",
                column: "TenantId");

            // 共通変数
            string adminUser = ApplicationConst.DefaultFirstAdminUserName;
            DateTime now = DateTime.Now;

            // MenuRoleMapsにノートブック管理を追加
            migrationBuilder.Sql($"INSERT INTO \"MenuRoleMaps\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"MenuCode\", \"RoleId\") SELECT nextval('\"MenuRoleMaps_Id_seq\"'), '{adminUser}', '{now}', '{adminUser}', '{now}', '{Logic.MenuLogic.NotebookMenu.Code.ToString()}', \"Id\" FROM \"Roles\" WHERE \"Name\" = 'researchers';");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotebookHistories");

            // MenuRoleMapsからノートブック管理のアクセス権を削除する
            migrationBuilder.Sql($"DELETE FROM \"MenuRoleMaps\" WHERE \"MenuCode\" = '{Logic.MenuLogic.NotebookMenu.Code.ToString()}';");
        }
    }
}
