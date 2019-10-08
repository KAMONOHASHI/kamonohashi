using Microsoft.EntityFrameworkCore.Migrations;
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
