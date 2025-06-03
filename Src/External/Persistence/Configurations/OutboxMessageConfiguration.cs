using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Outbox;

namespace Persistence.Configurations;

public partial class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> entity)
    {
        entity.ToTable("OutboxMessages", "Outbox");

        entity.Property(x => x.Type)
            .IsRequired()
            .HasMaxLength(1000);

        entity.Property(x => x.Content)
            .IsRequired();

        OnConfigurePartial(entity);
    }

    partial void OnConfigurePartial(EntityTypeBuilder<OutboxMessage> entity);
}

