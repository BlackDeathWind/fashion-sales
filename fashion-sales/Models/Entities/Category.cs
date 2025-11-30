namespace fashion_sales.Models.Entities;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public int? ParentId { get; set; }
    public bool IsActive { get; set; } = true;
    public int DisplayOrder { get; set; }

    public Category? Parent { get; set; }
    public ICollection<Category> Children { get; set; } = new List<Category>();
    public ICollection<Product> Products { get; set; } = new List<Product>();
}


