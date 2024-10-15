using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Golinks.Repository;

public sealed class GolinksContextFactory : IDesignTimeDbContextFactory<GolinksContext>
{
    public GolinksContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var optionsBuilder = new DbContextOptionsBuilder<GolinksContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new GolinksContext(optionsBuilder.Options);
    }
}
