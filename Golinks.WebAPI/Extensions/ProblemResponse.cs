using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Golinks.WebAPI.Extensions;

[ExcludeFromCodeCoverage]
public static class ProblemResponse
{
    public static Task WriteAsync(HttpContext context, int statusCode, string title, string detail, CancellationToken cancellationToken = default)
    {
        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail
        };

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        return context.Response.WriteAsync(JsonSerializer.Serialize(problem), cancellationToken);
    }
}
