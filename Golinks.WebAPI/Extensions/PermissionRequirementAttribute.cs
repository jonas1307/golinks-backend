using Microsoft.AspNetCore.Authorization;

namespace Golinks.WebAPI.Extensions;

public class PermissionRequirementAttribute : AuthorizeAttribute
{
    public PermissionRequirementAttribute(string permission) : base("PermissionPolicy")
    {
        Policy = permission;
    }
}
