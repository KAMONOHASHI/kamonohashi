using Microsoft.EntityFrameworkCore.Migrations;

namespace Nssol.Platypus.Migrations
{
    public partial class v230i : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"UPDATE ""Registries"" SET ""RegistryUrl"" = 'https://index.docker.io/' WHERE ""Id"" = 1 AND ""ServiceType"" = 1;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
