using InlämningWebb1.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InlämningWebb1.Infrastructure.Persistence.Configurations;

/// <summary>Configures the Products table columns and constraints.</summary>
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id); // Id becomes the PRIMARY KEY in the database

        builder.Property(p => p.Name)
               .IsRequired()        // NOT NULL — a product must always have a name
               .HasMaxLength(200);  // VARCHAR(200)

        // decimal(18,2) means: up to 18 digits total, 2 of them after the decimal point.
        // Always use this for money — float/double have rounding errors that cause bugs.
        builder.Property(p => p.Price)
               .HasColumnType("decimal(18,2)");
    }
}
