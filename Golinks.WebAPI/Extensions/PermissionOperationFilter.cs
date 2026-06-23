using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Golinks.WebAPI.Extensions;

[ExcludeFromCodeCoverage]
public class PermissionOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var permission = context.MethodInfo
            .GetCustomAttribute<PermissionRequirementAttribute>()?
            .RequiredPermission;

        if (permission is null)
            return;

        operation.Description = string.IsNullOrEmpty(operation.Description)
            ? $"**Required permission:** `{permission}`"
            : $"{operation.Description}\n\n**Required permission:** `{permission}`";
    }
}
