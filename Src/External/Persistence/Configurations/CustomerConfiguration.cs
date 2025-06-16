using Domain.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Constants;

namespace Persistence.Configurations;

internal partial class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable(TableNames.Customers, SchemaNames.eShop);

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id).HasConversion(
            customerId => customerId.Value,
            value => new CustomerId(value));

        builder.Property(c => c.Name).HasMaxLength(CustomerConstants.NameMaxLength);

        builder.Property(c => c.Email).HasMaxLength(CustomerConstants.EmailMaxLength);

        builder.HasIndex(c => c.Email).IsUnique();
    }
}
