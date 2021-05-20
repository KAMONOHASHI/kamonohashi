using Microsoft.EntityFrameworkCore.Migrations;

namespace Nssol.Platypus.Migrations
{
    public partial class v230k : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JupyterLabVersion",
                table: "NotebookHistories",
                nullable: false,
                defaultValue: "1.0.4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JupyterLabVersion",
                table: "NotebookHistories");
        }
    }
}
