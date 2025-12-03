using fashion_sales.Data;
using fashion_sales.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fashion_sales.Controllers;

public class CategoryController : Controller
{
    private readonly ApplicationDbContext _context;

    public CategoryController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? slug, int? id, string? price, string? sort)
    {
        var query = _context.Products
            .Include(p => p.Category)
            .Where(p => p.IsActive && p.Category.IsActive);

        string? categoryName = null;
        string? categorySlug = slug;

        if (!string.IsNullOrEmpty(slug))
        {
            query = query.Where(p => p.Category.Slug == slug);
            categoryName = await _context.Categories
                .Where(c => c.Slug == slug)
                .Select(c => c.Name)
                .FirstOrDefaultAsync();
        }
        else if (id.HasValue)
        {
            query = query.Where(p => p.CategoryId == id.Value);
            var cat = await _context.Categories.FindAsync(id.Value);
            categoryName = cat?.Name;
            categorySlug = cat?.Slug;
        }

        // Áp dụng bộ lọc giá
        if (!string.IsNullOrEmpty(price))
        {
            var effectivePrice = query.Select(p => new
            {
                Product = p,
                EffectivePrice = p.DiscountPrice ?? p.Price
            });

            switch (price)
            {
                case "0-49":
                    effectivePrice = effectivePrice.Where(x => x.EffectivePrice >= 0m && x.EffectivePrice <= 49_000m);
                    break;
                case "50-199":
                    effectivePrice = effectivePrice.Where(x => x.EffectivePrice >= 50_000m && x.EffectivePrice <= 199_000m);
                    break;
                case "200-499":
                    effectivePrice = effectivePrice.Where(x => x.EffectivePrice >= 200_000m && x.EffectivePrice <= 499_000m);
                    break;
                case "500+":
                    effectivePrice = effectivePrice.Where(x => x.EffectivePrice >= 500_000m);
                    break;
            }

            query = effectivePrice.Select(x => x.Product);
        }

        // Áp dụng sắp xếp
        sort = string.IsNullOrEmpty(sort) ? "newest" : sort;
        query = sort switch
        {
            "price-asc" => query.OrderBy(p => p.DiscountPrice ?? p.Price),
            "price-desc" => query.OrderByDescending(p => p.DiscountPrice ?? p.Price),
            _ => query.OrderByDescending(p => p.CreatedAt) // newest
        };

        var model = new ProductListViewModel
        {
            CategoryName = categoryName ?? "Tất cả sản phẩm",
            CategorySlug = categorySlug,
            PriceFilter = price,
            SortOption = sort,
            Products = await query.ToListAsync()
        };

        return View(model);
    }
}


