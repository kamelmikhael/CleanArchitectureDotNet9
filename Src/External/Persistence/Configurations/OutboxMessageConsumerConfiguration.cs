using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Outbox;

namespace Persistence.Configurations;

public partial class OutboxMessageConsumerConfiguration : IEntityTypeConfiguration<OutboxMessageConsumer>
{
    public void Configure(EntityTypeBuilder<OutboxMessageConsumer> entity)
    {
        entity.ToTable("OutboxMessageConsumers", "Outbox");

        entity.HasNoKey();

        entity.Property(x => x.Name)
            .IsRequired();
    }
}

