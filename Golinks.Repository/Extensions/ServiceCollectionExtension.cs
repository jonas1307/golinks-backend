using Golinks.Repository.Configs;
using Golinks.Repository.Contracts;
using Golinks.Repository.Extensions.Settings;
using Golinks.Repository.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Golinks.Repository.Extensions;

public static class ServiceCollectionExtension
{
    public static void RegisterRepositoryServices(this IServiceCollection services, Action<MongoDbSettings> configureOptions)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (configureOptions == null)
        {
            throw new ArgumentNullException(nameof(configureOptions));
        }

        services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
        services.AddTransient<ILinkRepository, LinkRepository>();

        services.Configure(configureOptions);

        services.AddSingleton<IMongoDbSettings>(serviceProvider =>
            serviceProvider.GetRequiredService<IOptions<MongoDbSettings>>().Value);

        LinkConfig.Apply();
        MetricConfig.Apply();
    }
}