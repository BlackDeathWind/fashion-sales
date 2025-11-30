using fashion_sales.Data;
using fashion_sales.Models.Entities;
using fashion_sales.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fashion_sales.Controllers;

public class ProductController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    public async Task<IActionResult> Search(string q)
    {
        q = q?.Trim();
        var query = _context.Products
            .Where(p => p.IsActive);

        if (!string.IsNullOrEmpty(q))
        {
            query = query.Where(p => p.Name.Contains(q));
        }

        var model = new ProductListViewModel
        {
            SearchTerm = q,
            Products = await query.OrderByDescending(p => p.CreatedAt).ToListAsync()
        };

        return View("SearchResults", model);
    }
}


