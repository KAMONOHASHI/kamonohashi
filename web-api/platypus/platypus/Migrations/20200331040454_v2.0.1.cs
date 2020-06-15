using Microsoft.EntityFrameworkCore.Migrations;
using Nssol.Platypus.Infrastructure.Types;

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

            // タグテーブルに種別を追加
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Tags",
                nullable: false,
                defaultValue: 0);

            // 登録済みのタグはすべてデータ管理から登録されたものとする。
            migrationBuilder.Sql("UPDATE \"Tags\" SET \"Type\" = 1;");

            // データセットをローカルコピーするか否かのカラムを追加
            migrationBuilder.AddColumn<bool>(
                name: "LocalDataSet",
                table: "TrainingHistories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LocalDataSet",
                table: "NotebookHistories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "LocalDataSet",
                table: "InferenceHistories",
                nullable: false,
                defaultValue: false);
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

            // タグテーブルから TypeがTraining のレコードを削除
            migrationBuilder.Sql($"DELETE FROM \"Tags\" WHERE \"Type\" = {(int)TagType.Training};");

            // タグテーブルから種別を削除
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Tags");

            // データセットをローカルコピーするか否かのカラムを削除
            migrationBuilder.DropColumn(
                name: "LocalDataSet",
                table: "TrainingHistories");

            migrationBuilder.DropColumn(
                name: "LocalDataSet",
                table: "NotebookHistories");

            migrationBuilder.DropColumn(
                name: "LocalDataSet",
                table: "InferenceHistories");
        }
    }
}
