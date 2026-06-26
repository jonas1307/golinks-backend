using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading.RateLimiting;

namespace Golinks.WebAPI.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static void AddRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.AddPolicy("public", context =>
                RateLimitPartition.GetSlidingWindowLimiter(
                    context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    _ => new SlidingWindowRateLimiterOptions
                    {
                        PermitLimit = 30,
                        Window = TimeSpan.FromMinutes(1),
                        SegmentsPerWindow = 6,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    }));

            options.OnRejected = async (context, cancellationToken) =>
                await ProblemResponse.WriteAsync(
                    context.HttpContext,
                    StatusCodes.Status429TooManyRequests,
                    "Too Many Requests",
                    "Request rate limit exceeded. Please try again later.",
                    cancellationToken);
        });
    }

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
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http
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

            s.OperationFilter<PermissionOperationFilter>();
            s.OperationFilter<RateLimitOperationFilter>();
            s.DocumentFilter<TagDescriptionsDocumentFilter>();

            IncludeXmlComments(s, Assembly.GetExecutingAssembly().GetName().Name);
            IncludeXmlComments(s, "Golinks.Application");
        });
    }

    private static void IncludeXmlComments(SwaggerGenOptions options, string? assemblyName)
    {
        if (string.IsNullOrEmpty(assemblyName))
            return;

        var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{assemblyName}.xml");
        if (File.Exists(xmlPath))
            options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
    }
}
