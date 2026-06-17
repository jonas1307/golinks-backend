using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Golinks.Repository;

public sealed class GolinksContextFactory : IDesignTimeDbContextFactory<GolinksContext>
{
    public GolinksContext CreateDbContext(string[] args)
    {
        var basePath = args.Length > 0 && Directory.Exists(args[0])
            ? args[0]
            : Path.Combine(Directory.GetCurrentDirectory(), "../Golinks.WebAPI");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        var optionsBuilder = new DbContextOptionsBuilder<GolinksContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new GolinksContext(optionsBuilder.Options);
    }
}
