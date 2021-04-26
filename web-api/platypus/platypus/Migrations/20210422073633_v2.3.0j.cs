using Microsoft.EntityFrameworkCore.Migrations;
using System;
using Nssol.Platypus.Infrastructure;


namespace Nssol.Platypus.Migrations
{
    public partial class v230j : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 共通変数
            string adminUser = ApplicationConst.DefaultFirstAdminUserName;
            DateTime now = DateTime.Now;

            // アクアリウムの初期権限のマッピングを追加
            // ユーザーロール : データセット、実験実行、実験履歴
            migrationBuilder.Sql($"INSERT INTO \"MenuRoleMaps\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"MenuCode\", \"RoleId\") SELECT nextval('\"MenuRoleMaps_Id_seq\"'), '{adminUser}', '{now}', '{adminUser}', '{now}', '{Logic.MenuLogic.AquariumDataSetMenu.Code.ToString()}', \"Id\" FROM \"Roles\" WHERE \"Name\" = 'users';");
            migrationBuilder.Sql($"INSERT INTO \"MenuRoleMaps\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"MenuCode\", \"RoleId\") SELECT nextval('\"MenuRoleMaps_Id_seq\"'), '{adminUser}', '{now}', '{adminUser}', '{now}', '{Logic.MenuLogic.ExperimentMenu.Code.ToString()}', \"Id\" FROM \"Roles\" WHERE \"Name\" = 'users';");
            migrationBuilder.Sql($"INSERT INTO \"MenuRoleMaps\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"MenuCode\", \"RoleId\") SELECT nextval('\"MenuRoleMaps_Id_seq\"'), '{adminUser}', '{now}', '{adminUser}', '{now}', '{Logic.MenuLogic.ExperimentHistoryMenu.Code.ToString()}', \"Id\" FROM \"Roles\" WHERE \"Name\" = 'users';");
            // リサーチャーロール : データセット、実験実行、実験履歴、テンプレート編集
            migrationBuilder.Sql($"INSERT INTO \"MenuRoleMaps\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"MenuCode\", \"RoleId\") SELECT nextval('\"MenuRoleMaps_Id_seq\"'), '{adminUser}', '{now}', '{adminUser}', '{now}', '{Logic.MenuLogic.AquariumDataSetMenu.Code.ToString()}', \"Id\" FROM \"Roles\" WHERE \"Name\" = 'researchers';");
            migrationBuilder.Sql($"INSERT INTO \"MenuRoleMaps\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"MenuCode\", \"RoleId\") SELECT nextval('\"MenuRoleMaps_Id_seq\"'), '{adminUser}', '{now}', '{adminUser}', '{now}', '{Logic.MenuLogic.ExperimentMenu.Code.ToString()}', \"Id\" FROM \"Roles\" WHERE \"Name\" = 'researchers';");
            migrationBuilder.Sql($"INSERT INTO \"MenuRoleMaps\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"MenuCode\", \"RoleId\") SELECT nextval('\"MenuRoleMaps_Id_seq\"'), '{adminUser}', '{now}', '{adminUser}', '{now}', '{Logic.MenuLogic.ExperimentHistoryMenu.Code.ToString()}', \"Id\" FROM \"Roles\" WHERE \"Name\" = 'researchers';");
            migrationBuilder.Sql($"INSERT INTO \"MenuRoleMaps\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"MenuCode\", \"RoleId\") SELECT nextval('\"MenuRoleMaps_Id_seq\"'), '{adminUser}', '{now}', '{adminUser}', '{now}', '{Logic.MenuLogic.TemplateMenu.Code.ToString()}', \"Id\" FROM \"Roles\" WHERE \"Name\" = 'researchers';");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
