using Microsoft.EntityFrameworkCore.Migrations;

namespace Nssol.Platypus.Migrations
{
    public partial class v202 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
