using Golinks.Repository.Configs;
using Golinks.Repository.Contracts;
using Golinks.Repository.Extensions.Settings;
using Golinks.Repository.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Golinks.Repository.Extensions;

public static class ServiceCollectionExtension
{
    public static void RegisterRepositoryServices(this IServiceCollection services, IConfiguration configuration)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
        services.AddTransient<ILinkRepository, LinkRepository>();
        services.AddTransient<IMetricRepository, MetricRepository>();

        services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));

        services.AddSingleton<IMongoDbSettings>(serviceProvider =>
            serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value);

        LinkConfig.Apply();
        MetricConfig.Apply();
    }
}