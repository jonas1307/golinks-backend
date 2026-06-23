using Microsoft.AspNetCore.RateLimiting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Golinks.WebAPI.Extensions;

[ExcludeFromCodeCoverage]
public class RateLimitOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var hasRateLimiting =
            context.MethodInfo.GetCustomAttribute<EnableRateLimitingAttribute>() is not null ||
            context.MethodInfo.DeclaringType?.GetCustomAttribute<EnableRateLimitingAttribute>() is not null;

        if (!hasRateLimiting)
            return;

        operation.Responses.TryAdd(
            StatusCodes.Status429TooManyRequests.ToString(),
            new OpenApiResponse { Description = "Too Many Requests" });
    }
}
