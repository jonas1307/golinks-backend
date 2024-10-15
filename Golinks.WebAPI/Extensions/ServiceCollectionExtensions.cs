using Microsoft.OpenApi.Models;
using System.Diagnostics.CodeAnalysis;

namespace Golinks.WebAPI.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static void AddSwaggerConfiguration(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSwaggerGen(s =>
        {
            s.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Golinks",
                Description = "Golinks API",
                Contact = new OpenApiContact { Name = "Jonas Amorim", Url = new Uri("https://go.amorim.dev") },
                License = new OpenApiLicense { Name = "MIT", Url = new Uri("https://github.com/jonas1307/golinks/blob/main/LICENSE") }
            });

            s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Input the JWT like: Bearer {your token}",
                Name = "Authorization",
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            s.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

        });
    }
}
