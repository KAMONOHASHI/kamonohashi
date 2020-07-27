using Microsoft.EntityFrameworkCore.Migrations;

namespace Nssol.Platypus.Migrations
{
    public partial class v211 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // tensorboardコンテナにマウントした学習履歴IDカラム追加
            migrationBuilder.AddColumn<string>(
                name: "MountedTrainingHistoryIds",
                table: "TensorBoardContainers",
                nullable: true,
                defaultValue: null
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // tensorboardコンテナのマウントした学習履歴IDカラム削除
            migrationBuilder.DropColumn(
                name: "MountedTrainingHistoryIds",
                table: "TensorBoardContainers"
            );
        }
    }
}
