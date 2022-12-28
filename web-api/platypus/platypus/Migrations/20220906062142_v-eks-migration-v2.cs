using Microsoft.EntityFrameworkCore.Migrations;

namespace Nssol.Platypus.Migrations
{
    public partial class veksmigrationv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UsablePendingJobCounts",
                table: "Eks",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsablePendingJobCounts",
                table: "Eks");
        }
    }
}
