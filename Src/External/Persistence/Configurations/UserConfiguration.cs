using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Constants;

namespace Persistence.Configurations;

public partial class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> entity)
    {
        entity.ToTable(TableNames.Users, SchemaNames.Identity);

        entity.Property(x => x.Username)
            .HasConversion(
                x => x.Value,
                x => UserName.Create(x).Value)
            .IsRequired()
            .HasMaxLength(200);

        entity.Property(x => x.Email)
            .HasConversion(
                x => x.Value,
                x => Email.Create(x).Value)
            .IsRequired()
            .HasMaxLength(256);

        entity.HasMany(x => x.Roles)
            .WithMany(x => x.Users)
            .UsingEntity<UserRole>();

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<User> entity);
}
