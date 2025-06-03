namespace Infrastructure.Authentication.Permissions;

public interface IPermissionService
{
    Task<HashSet<string>> GetPermissionsAsync(Guid userId);
}
