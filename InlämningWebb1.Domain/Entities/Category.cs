namespace InlämningWebb1.Domain.Entities;

/// <summary>A product category. One category has many products.</summary>
public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Navigation property — EF Core populates this when using .Include()
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
