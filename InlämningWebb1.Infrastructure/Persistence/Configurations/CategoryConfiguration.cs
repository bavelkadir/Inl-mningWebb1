using InlämningWebb1.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InlämningWebb1.Infrastructure.Persistence.Configurations;

/// <summary>Configures the Categories table and its relationship to Products.</summary>
public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.Id); // Id becomes the PRIMARY KEY in the database

        builder.Property(c => c.Name)
               .IsRequired()       // NOT NULL in the database — Name cannot be empty
               .HasMaxLength(100); // VARCHAR(100) — limits how long a name can be

        // This defines the one-to-many relationship:
        // One Category has many Products.
        // Each Product belongs to one Category via the CategoryId foreign key.
        // Cascade delete means: if a Category is deleted, all its Products are deleted too.
        builder.HasMany(c => c.Products)
               .WithOne(p => p.Category)
               .HasForeignKey(p => p.CategoryId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
