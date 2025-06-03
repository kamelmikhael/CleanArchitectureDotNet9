using Domain.Permissions;
using Domain.Users;
using SharedKernal.Primitives;

namespace Domain.Roles;

public sealed class Role : Enumeration<Role>
{
    public static readonly Role SuperAdmin = new(1, "SuperAdmin");

    public Role(int id, string name) : base(id, name)
    {
    }

    public ICollection<Permission> Permissions { get; set; }
    public ICollection<User> Users { get; set; }
}
