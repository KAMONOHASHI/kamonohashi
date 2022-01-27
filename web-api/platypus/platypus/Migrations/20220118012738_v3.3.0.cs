using Microsoft.EntityFrameworkCore.Migrations;

namespace Nssol.Platypus.Migrations
{
    public partial class v330 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Mention",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SlackUrl",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mention",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SlackUrl",
                table: "Users");
        }
    }
}
