using fashion_sales.Models.Entities;

namespace fashion_sales.Models.ViewModels;

public class ProductListViewModel
{
    public string? CategoryName { get; set; }
    public string? CategorySlug { get; set; }
    public string? SearchTerm { get; set; }

    // Bộ lọc
    public string? PriceFilter { get; set; }    // all, lt200, 200-500, gt500
    public string? SortOption { get; set; }     // newest, price-asc, price-desc

    public IEnumerable<Product> Products { get; set; } = Enumerable.Empty<Product>();
}


