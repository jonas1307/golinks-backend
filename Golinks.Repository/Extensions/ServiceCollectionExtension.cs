using Golinks.Repository.Contracts;
using Golinks.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Golinks.Repository.Extensions;

public static class ServiceCollectionExtension
{
    public static void RegisterRepositoryServices(this IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
        services.AddTransient<ILinkRepository, LinkRepository>();
        services.AddTransient<IMetricRepository, MetricRepository>();

        services.AddDbContext<GolinksContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
    }
}