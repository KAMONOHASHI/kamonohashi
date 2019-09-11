using Microsoft.EntityFrameworkCore.Migrations;
using System.Text;

namespace Nssol.Platypus.Migrations
{
    public partial class v112 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            StringBuilder query = new StringBuilder();

            // UserTenantGitMaps テーブルから不要データを削除するクエリを作成
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

        }
    }
}
