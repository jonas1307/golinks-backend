using Golinks.Domain;
using Golinks.Repository.Contracts;
using Golinks.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Golinks.Repository.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddRepositoryServices(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<ILinkRepository, LinkRepository>();
        services.AddScoped<IMetricRepository, MetricRepository>();

        services.AddDbContext<GolinksContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<GolinksContext>());
    }
}