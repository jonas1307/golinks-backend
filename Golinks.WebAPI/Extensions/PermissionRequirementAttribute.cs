using Microsoft.AspNetCore.Authorization;

namespace Golinks.WebAPI.Extensions;

public class PermissionRequirementAttribute : AuthorizeAttribute
{
    public string RequiredPermission { get; }

    public PermissionRequirementAttribute(string permission) : base("PermissionPolicy")
    {
        RequiredPermission = permission;
    }
}
