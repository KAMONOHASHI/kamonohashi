using Microsoft.EntityFrameworkCore.Migrations;

namespace Nssol.Platypus.Migrations
{
    public partial class v201 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // TensorBoardコンテナの生存期間を管理するカラムを追加
            migrationBuilder.AddColumn<int>(
                name: "ExpiresIn",
                table: "TensorBoardContainers",
                nullable: false,
                defaultValue: 0);

            // ノートブック履歴に実行コマンドカラムを追加
            migrationBuilder.AddColumn<string>(
                name: "EntryPoint",
                table: "NotebookHistories",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // TensorBoardコンテナの生存期間を管理するカラムを削除
            migrationBuilder.DropColumn(
                name: "ExpiresIn",
                table: "TensorBoardContainers");

            // ノートブック履歴から実行コマンドカラムを削除
            migrationBuilder.DropColumn(
                name: "EntryPoint",
                table: "NotebookHistories");
        }
    }
}
