using Domain.Enums;
using Domain.Roles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Constants;

namespace Persistence.Configurations;

public partial class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable(TableNames.RolePermissions, SchemaNames.Identity);

        builder.HasKey(x => new { x.RoleId, x.PermissionId } );

        builder.HasData(
            Create(Role.SuperAdmin, PermissionEnum.ReadUser),
            Create(Role.SuperAdmin, PermissionEnum.UpdateUser),
            Create(Role.SuperAdmin, PermissionEnum.DeleteUser),
            Create(Role.SuperAdmin, PermissionEnum.ActivateUser),
            Create(Role.SuperAdmin, PermissionEnum.DeactivateUser)
        );
    }

    private static RolePermission Create(
        Role role,
        PermissionEnum permission)
    {
        return new()
        {
            RoleId = role.Id,
            PermissionId = (int)permission,
        };
    }
}
