using Microsoft.EntityFrameworkCore.Migrations;
using Nssol.Platypus.Infrastructure;
using Nssol.Platypus.Infrastructure.Types;
using System;
using System.Text;

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


            // UserTenantGitMaps テーブルから不要データを削除するクエリを作成
            StringBuilder query = new StringBuilder();
            query.AppendLine("DELETE FROM \"UserTenantGitMaps\"");
            query.AppendLine("WHERE");
            query.AppendLine("	\"Id\" IN (");
            query.AppendLine("		SELECT UTGM.\"Id\"");
            query.AppendLine("		FROM \"UserTenantGitMaps\" AS UTGM");
            query.AppendLine("		INNER JOIN \"TenantGitMaps\" AS TGM");
            query.AppendLine("		ON UTGM.\"TenantGitMapId\" = TGM.\"Id\"");
            query.AppendLine("		WHERE");
            query.AppendLine("			NOT EXISTS (");
            query.AppendLine("				SELECT * ");
            query.AppendLine("				FROM \"UserTenantMaps\" AS UTM");
            query.AppendLine("				WHERE TGM.\"TenantId\" = UTM.\"TenantId\"");
            query.AppendLine("				AND UTM.\"UserId\" = UTGM.\"UserId\")");
            query.AppendLine("	);");

            migrationBuilder.Sql(query.ToString());

            // 共通変数
            string adminUser = ApplicationConst.DefaultFirstAdminUserName;
            DateTime now = DateTime.Now;

            // GitにGitLab.comを編集不可で登録
            migrationBuilder.Sql($"INSERT INTO \"Gits\" (\"Id\", \"CreatedBy\", \"CreatedAt\", \"ModifiedBy\", \"ModifiedAt\", \"Name\", \"ServiceType\", \"ApiUrl\", \"Token\", \"RepositoryUrl\", \"IsNotEditable\") SELECT nextval('\"Gits_Id_seq\"'), '{adminUser}', '{now}', '{adminUser}', '{now}', 'GitLab.com', {(int)GitServiceType.GitLabCom}, 'https://gitlab.com', null, 'https://gitlab.com', true;");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Zip",
                table: "TrainingHistories");

            migrationBuilder.DropColumn(
                name: "Zip",
                table: "InferenceHistories");

            // Gitから ServiceTypeがGitLabCom 且つ 編集不可 のレコードを削除
            migrationBuilder.Sql($"DELETE FROM \"Gits\" WHERE \"ServiceType\" = {(int)GitServiceType.GitLabCom} AND \"IsNotEditable\" = true;");
        }
    }
}
