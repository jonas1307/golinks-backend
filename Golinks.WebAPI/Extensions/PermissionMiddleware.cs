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
            var userPermissions = context.User.FindAll("permissions").Select(c => c.Value).ToHashSet();

            if (!userPermissions.Contains(requiredPermission))
            {
                await ProblemResponse.WriteAsync(context, StatusCodes.Status403Forbidden, "Forbidden", "You don't have the required permission to access this resource.");
                return;
            }
        }

        await _next(context);
    }
}
