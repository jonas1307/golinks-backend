using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Golinks.WebAPI.Extensions;

public class PermissionMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        var requiredPermission = endpoint?.Metadata.GetMetadata<PermissionRequirementAttribute>()?.RequiredPermission;

        if (requiredPermission != null)
        {
            if (context.User.Identity?.IsAuthenticated != true)
            {
                await WriteProblemAsync(context, StatusCodes.Status401Unauthorized, "Unauthorized", "Authentication is required to access this resource.");
                return;
            }

            var userPermissions = context.User.FindAll("permissions").Select(c => c.Value).ToHashSet();

            if (!userPermissions.Contains(requiredPermission))
            {
                await WriteProblemAsync(context, StatusCodes.Status403Forbidden, "Forbidden", "You don't have the required permission to access this resource.");
                return;
            }
        }

        await _next(context);
    }

    private static async Task WriteProblemAsync(HttpContext context, int statusCode, string title, string detail)
    {
        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail
        };

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/problem+json";

        await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }
}
