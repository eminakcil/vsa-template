using Microsoft.AspNetCore.Authorization;

namespace VsaTemplate.Common.Authorization;

public sealed class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(string permission)
        : base(policy: permission) { }
}
