using Domain.Customers;
using Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Constants;

namespace Persistence.Configurations;

internal partial class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable(TableNames.Orders, SchemaNames.eShop);

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id).HasConversion(
            orderId => orderId.Value,
            value => new OrderId(value));

        builder.HasOne<Customer>()
            .WithMany()
            .HasForeignKey(o => o.CustomerId)
            .IsRequired();

        builder.HasMany(o => o.LineItems)
            .WithOne()
            .HasForeignKey(li => li.OrderId);
    }
}
