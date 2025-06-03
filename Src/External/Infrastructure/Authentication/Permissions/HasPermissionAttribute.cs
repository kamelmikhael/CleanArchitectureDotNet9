using Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authentication.Permissions;

public sealed class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(PermissionEnum permission)
        : base(policy: permission.ToString())
    { }
}
