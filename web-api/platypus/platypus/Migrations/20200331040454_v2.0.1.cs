using Microsoft.EntityFrameworkCore.Migrations;
using Nssol.Platypus.Infrastructure.Types;

namespace Nssol.Platypus.Migrations
{
    public partial class v201 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExpiresIn",
                table: "TensorBoardContainers",
                nullable: false,
                defaultValue: 0);

            // タグテーブルに種別を追加
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Tags",
                nullable: false,
                defaultValue: 0);

            // 登録済みのタグはすべてデータ管理から登録されたものとする。
            migrationBuilder.Sql("UPDATE \"Tags\" SET \"Type\" = 1;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiresIn",
                table: "TensorBoardContainers");

            // タグテーブルから TypeがTraining のレコードを削除
            migrationBuilder.Sql($"DELETE FROM \"Tags\" WHERE \"Type\" = {(int)TagType.Training};");

            // タグテーブルから種別を削除
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Tags");
        }
    }
}
