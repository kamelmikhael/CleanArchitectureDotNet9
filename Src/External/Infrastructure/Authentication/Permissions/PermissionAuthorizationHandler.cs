using Infrastructure.Authentication.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Collections.Generic;

namespace Infrastructure.Authentication.Permissions;

public class PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
    : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, 
        PermissionRequirement requirement)
    {
        var permissions = context
            .User
            .Claims
            .Where(x => x.Type == CustomClaims.Permissions)
            .Select(x => x.Value)
            .ToHashSet();

        if (permissions.Contains(requirement.Permission))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }

    private async Task<HashSet<string>> GetPermissionsFromDBAsync(AuthorizationHandlerContext context)
    {
        string? userIdStr = context.User.Claims.FirstOrDefault(
            claim => claim.Type == JwtRegisteredClaimNames.Sub)?.Value;

        if (!Guid.TryParse(userIdStr, out Guid userId))
        {
            return [];
        }

        using IServiceScope scope = serviceScopeFactory.CreateScope();

        IPermissionService permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService>();

        HashSet<string> permissions = await permissionService.GetPermissionsAsync(userId);

        return permissions;
    }
}
