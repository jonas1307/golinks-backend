﻿using Golinks.Application.Contracts;
using Golinks.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Golinks.Application.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static void AddAutoMapperConfiguration(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
        services.AddTransient<ILinkService, LinkService>();
        services.AddTransient<IMetricService, MetricService>();
        services.AddTransient<IActionService, ActionService>();

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    }
}