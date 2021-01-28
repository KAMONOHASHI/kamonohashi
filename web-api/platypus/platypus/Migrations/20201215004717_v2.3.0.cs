using Microsoft.EntityFrameworkCore.Migrations;

namespace Nssol.Platypus.Migrations
{
    public partial class v230 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFlat",
                table: "DataSets",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFlat",
                table: "DataSets");
        }
    }
}
