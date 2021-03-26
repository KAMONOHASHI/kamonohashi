using Microsoft.EntityFrameworkCore.Migrations;

namespace Nssol.Platypus.Migrations
{
    public partial class v230h : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EvaluationContainerToken",
                table: "TemplateVersions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EvaluationRepositoryToken",
                table: "TemplateVersions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreprocessContainerToken",
                table: "TemplateVersions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PreprocessRepositoryToken",
                table: "TemplateVersions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrainingContainerToken",
                table: "TemplateVersions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrainingRepositoryToken",
                table: "TemplateVersions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EvaluationContainerToken",
                table: "TemplateVersions");

            migrationBuilder.DropColumn(
                name: "EvaluationRepositoryToken",
                table: "TemplateVersions");

            migrationBuilder.DropColumn(
                name: "PreprocessContainerToken",
                table: "TemplateVersions");

            migrationBuilder.DropColumn(
                name: "PreprocessRepositoryToken",
                table: "TemplateVersions");

            migrationBuilder.DropColumn(
                name: "TrainingContainerToken",
                table: "TemplateVersions");

            migrationBuilder.DropColumn(
                name: "TrainingRepositoryToken",
                table: "TemplateVersions");
        }
    }
}
