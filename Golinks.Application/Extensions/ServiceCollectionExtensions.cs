using FluentValidation;
using Golinks.Application.Contracts;
using Golinks.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Golinks.Application.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
        services.AddScoped<ILinkService, LinkService>();
        services.AddScoped<IActionService, ActionService>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddAutoMapper(cfg => cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies()));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }
}