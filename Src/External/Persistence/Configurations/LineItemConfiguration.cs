using Domain.Customers;
using Domain.Orders;
using Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Constants;

namespace Persistence.Configurations;

internal partial class LineItemConfiguration : IEntityTypeConfiguration<LineItem>
{
    public void Configure(EntityTypeBuilder<LineItem> builder)
    {
        builder.ToTable(TableNames.LineItems, SchemaNames.eShop);

        builder.HasKey(li => li.Id);

        builder.Property(o => o.Id).HasConversion(
            lineItemId => lineItemId.Value,
            value => new LineItemId(value));

        builder.OwnsOne(p => p.Price, priceBuilder => {
            priceBuilder.Property(m => m.Currency).HasMaxLength(MoneyConstants.CurrencyMaxLength);
            priceBuilder.Property(m => m.Amount).HasPrecision(2, 2);
        });

        builder.HasOne<Product>()
            .WithMany()
            .HasForeignKey(li => li.ProductId)
            .IsRequired();
    }
}
