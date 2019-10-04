using Microsoft.EntityFrameworkCore.Migrations;

namespace Nssol.Platypus.Migrations
{
    public partial class v112 : Migration
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
