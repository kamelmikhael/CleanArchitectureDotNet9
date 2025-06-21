using Domain.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Constants;

namespace Persistence.Configurations;

internal partial class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable(TableNames.Products, SchemaNames.eShop);

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id).HasConversion(
            productId => productId.Value,
            value => new ProductId(value));

        builder.Property(p => p.Name).HasMaxLength(ProductConstants.NameMaxLength);

        builder.Property(p => p.Sku).HasConversion(
            sku => sku.Value,
            value => Sku.Create(value));

        builder.OwnsOne(p => p.Price, priceBuilder => {
            priceBuilder.Property(m => m.Currency).HasMaxLength(MoneyConstants.CurrencyMaxLength);
            priceBuilder.Property(m => m.Amount);
        });
    }
}
