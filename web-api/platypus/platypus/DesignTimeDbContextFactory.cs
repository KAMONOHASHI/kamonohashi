using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Nssol.Platypus.DataAccess;

namespace Nssol.Platypus
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CommonDbContext>
    {
        public CommonDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<CommonDbContext>();
            var connectionString = "Server=wp00069;Port=30000;User Id=platypus;Password=P@ssw0rd!;Database=mikunidb0;Integrated Security=False;Timeout=30";
            builder.UseNpgsql(connectionString);

            Startup.DefaultConnectionString = connectionString;
            return new CommonDbContext(builder.Options);
        }
    }
}