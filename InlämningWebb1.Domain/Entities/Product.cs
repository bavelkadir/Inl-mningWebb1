namespace InlämningWebb1.Domain.Entities;

/// <summary>A product that belongs to one category.</summary>
public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; } // Always decimal for money, never float/double
    public Guid CategoryId { get; set; }

    // Navigation property — null! tells compiler EF Core always fills this in
    public Category Category { get; set; } = null!;
}
