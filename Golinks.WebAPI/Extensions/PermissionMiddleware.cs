namespace Golinks.WebAPI.Extensions;

public class PermissionMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        var requiredPermission = endpoint?.Metadata.GetMetadata<PermissionRequirementAttribute>()?.Policy;

        if (requiredPermission != null)
        {
            var userPermissions = context.User.FindFirst("permissions")?.Value;
            
            if (string.IsNullOrEmpty(userPermissions) || !userPermissions.Split(',').Contains(requiredPermission))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Forbidden: You don't have the required permission.");
                
                return;
            }
        }

        await _next(context);
    }
}
