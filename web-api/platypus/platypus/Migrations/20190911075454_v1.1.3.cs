using Microsoft.EntityFrameworkCore.Migrations;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Types;
using System;

namespace Nssol.Platypus.Migrations
{
    public partial class v113 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Zip",
                table: "TrainingHistories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Zip",
                table: "InferenceHistories",
                nullable: false,
                defaultValue: false);

            // 学習履歴の更新（一律でZipをtrueにする）
            migrationBuilder.Sql("UPDATE \"TrainingHistories\" SET \"Zip\" = true;");
            // 推論履歴の更新（一律でZipをtrueにする）
            migrationBuilder.Sql("UPDATE \"InferenceHistories\" SET \"Zip\" = true;");

            // 共通変数
            string adminUser = ApplicationConst.DefaultFirstAdminUserName;
            DateTime now = DateTime.Now;

            // GitにGitLab.comを編集不可で登録
            migrationBuilder.Sql($"INSERT INTO \"Gits\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"Name\", \"ServiceType\", \"ApiUrl\", \"Token\", \"RepositoryUrl\", \"IsNotEditable\") SELECT nextval('\"Gits_Id_seq\"'), '{adminUser}', '{now}', '{adminUser}', '{now}', 'GitLab.com', {(int)GitServiceType.GitLabCom}, 'https://gitlab.com', null, 'https://gitlab.com', true WHERE NOT EXISTS (SELECT 1 FROM \"Gits\" WHERE \"Name\" = 'GitLab.com');");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Zip",
                table: "TrainingHistories");

            migrationBuilder.DropColumn(
                name: "Zip",
                table: "InferenceHistories");
        }
    }
}
