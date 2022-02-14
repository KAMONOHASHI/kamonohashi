using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Nssol.Platypus.Infrastructure;

namespace Nssol.Platypus.Migrations
{
    public partial class v340 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOrigin",
                table: "UserTenantRegistryMaps",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "UserGroupTenantMapIds",
                table: "UserTenantRegistryMaps",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsOrigin",
                table: "UserTenantMaps",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "UserGroupTenantMapIds",
                table: "UserTenantMaps",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsOrigin",
                table: "UserTenantGitMaps",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "UserGroupTenantMapIds",
                table: "UserTenantGitMaps",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsOrigin",
                table: "UserRoleMaps",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "UserGroupTenantMapIds",
                table: "UserRoleMaps",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserGroups",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Memo = table.Column<string>(nullable: true),
                    IsGroup = table.Column<bool>(nullable: false),
                    Dn = table.Column<string>(nullable: false),
                    IsDirect = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserGroupRoleMaps",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    UserGroupId = table.Column<long>(nullable: false),
                    RoleId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroupRoleMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserGroupRoleMaps_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGroupRoleMaps_UserGroups_UserGroupId",
                        column: x => x.UserGroupId,
                        principalTable: "UserGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserGroupTenantMaps",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedBy = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    UserGroupId = table.Column<long>(nullable: false),
                    TenantId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroupTenantMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserGroupTenantMaps_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGroupTenantMaps_UserGroups_UserGroupId",
                        column: x => x.UserGroupId,
                        principalTable: "UserGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupRoleMaps_RoleId",
                table: "UserGroupRoleMaps",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupRoleMaps_UserGroupId",
                table: "UserGroupRoleMaps",
                column: "UserGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupTenantMaps_TenantId",
                table: "UserGroupTenantMaps",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroupTenantMaps_UserGroupId",
                table: "UserGroupTenantMaps",
                column: "UserGroupId");

            // 共通変数
            string adminUser = ApplicationConst.DefaultFirstAdminUserName;
            DateTime now = DateTime.Now;

            // MenuRoleMapsにユーザグループ管理を追加
            migrationBuilder.Sql($"INSERT INTO \"MenuRoleMaps\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"MenuCode\", \"RoleId\") SELECT nextval('\"MenuRoleMaps_Id_seq\"'), '{adminUser}', '{now}', '{adminUser}', '{now}', '{Logic.MenuLogic.UserGroupMenu.Code.ToString()}', \"Id\" FROM \"Roles\" WHERE \"Name\" = 'admins';");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // KQI上でユーザとテナントとの紐づけがされていないレコードは削除する。
            migrationBuilder.Sql($"DELETE FROM \"UserTenantRegistryMaps\" WHERE \"IsOrigin\" = false;");
            migrationBuilder.Sql($"DELETE FROM \"UserTenantGitMaps\" WHERE \"IsOrigin\" = false;");
            migrationBuilder.Sql($"DELETE FROM \"UserRoleMaps\" WHERE \"IsOrigin\" = false;");
            migrationBuilder.Sql($"DELETE FROM \"UserTenantMaps\" WHERE \"IsOrigin\" = false;");

            // どのテナントにも所属していないユーザを削除する。
            migrationBuilder.Sql($"DELETE FROM \"Users\" AS U WHERE NOT EXISTS (SELECT 1 FROM \"UserTenantMaps\" AS UTM WHERE UTM.\"UserId\" = U.\"Id\");");

            // MenuRoleMapsからユーザグループ管理のアクセス権を削除する。
            migrationBuilder.Sql($"DELETE FROM \"MenuRoleMaps\" WHERE \"MenuCode\" = '{Logic.MenuLogic.UserGroupMenu.Code.ToString()}';");

            migrationBuilder.DropColumn(
                name: "IsOrigin",
                table: "UserTenantRegistryMaps");

            migrationBuilder.DropColumn(
                name: "UserGroupTenantMapIds",
                table: "UserTenantRegistryMaps");

            migrationBuilder.DropColumn(
                name: "IsOrigin",
                table: "UserTenantMaps");

            migrationBuilder.DropColumn(
                name: "UserGroupTenantMapIds",
                table: "UserTenantMaps");

            migrationBuilder.DropColumn(
                name: "IsOrigin",
                table: "UserTenantGitMaps");

            migrationBuilder.DropColumn(
                name: "UserGroupTenantMapIds",
                table: "UserTenantGitMaps");

            migrationBuilder.DropColumn(
                name: "IsOrigin",
                table: "UserRoleMaps");

            migrationBuilder.DropColumn(
                name: "UserGroupTenantMapIds",
                table: "UserRoleMaps");

            migrationBuilder.DropTable(
                name: "UserGroupRoleMaps");

            migrationBuilder.DropTable(
                name: "UserGroupTenantMaps");

            migrationBuilder.DropTable(
                name: "UserGroups");
        }
    }
}
