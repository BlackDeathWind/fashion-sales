using fashion_sales.Data;
using fashion_sales.Models.Entities;
using fashion_sales.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace fashion_sales.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Staff")]
public class ProductsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _env;

    public ProductsController(ApplicationDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _context.Products
            .Include(p => p.Category)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
        return View(products);
    }

    public async Task<IActionResult> Create()
    {
        var vm = new ProductFormViewModel
        {
            Categories = await GetCategorySelectListAsync()
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductFormViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            vm.Categories = await GetCategorySelectListAsync();
            return View(vm);
        }

        // Require main image on create
        if (vm.MainImage == null || vm.MainImage.Length == 0)
        {
            ModelState.AddModelError("MainImage", "Vui l�ng ch?n ?nh ch�nh cho s?n ph?m.");
            vm.Categories = await GetCategorySelectListAsync();
            return View(vm);
        }

        var product = new Product
        {
            Name = vm.Name,
            Slug = GenerateSlug(vm.Name),
            CategoryId = vm.CategoryId,
            Description = vm.Description,
            Price = vm.Price,
            DiscountPrice = vm.DiscountPrice,
            StockQuantity = vm.StockQuantity,
            IsActive = vm.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        await SaveMainImageAsync(vm.MainImage, product);

        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id);
        if (product == null)
        {
            return NotFound();
        }

        var vm = new ProductFormViewModel
        {
            Id = product.Id,
            Name = product.Name,
            CategoryId = product.CategoryId,
            Description = product.Description,
            Price = product.Price,
            DiscountPrice = product.DiscountPrice,
            StockQuantity = product.StockQuantity,
            IsActive = product.IsActive,
            Categories = await GetCategorySelectListAsync(),
            ExistingProduct = product
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductFormViewModel vm)
    {
        if (id != vm.Id)
        {
            return NotFound();
        }

        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id);
        if (product == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            vm.Categories = await GetCategorySelectListAsync();
            vm.ExistingProduct = product;
            return View(vm);
        }

        product.Name = vm.Name;
        product.Slug = GenerateSlug(vm.Name);
        product.CategoryId = vm.CategoryId;
        product.Description = vm.Description;
        product.Price = vm.Price;
        product.DiscountPrice = vm.DiscountPrice;
        product.StockQuantity = vm.StockQuantity;
        product.IsActive = vm.IsActive;

        await SaveMainImageAsync(vm.MainImage, product);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task<List<SelectListItem>> GetCategorySelectListAsync()
    {
        return await _context.Categories
            .OrderBy(c => c.DisplayOrder)
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            })
            .ToListAsync();
    }

    private async Task SaveMainImageAsync(IFormFile? file, Product product)
    {
        if (file == null || file.Length == 0)
        {
            return;
        }

        var uploadsFolder = Path.Combine(_env.WebRootPath, "images", "products");
        Directory.CreateDirectory(uploadsFolder);

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(uploadsFolder, fileName);

        await using (var stream = System.IO.File.Create(filePath))
        {
            await file.CopyToAsync(stream);
        }

        var url = $"/images/products/{fileName}";

        // set main image URL on product (required)
        product.MainImageUrl = url;
    }

    private static string GenerateSlug(string name)
    {
        var slug = name.Trim().ToLowerInvariant();
        slug = slug.Replace(" ", "-");
        return slug;
    }
}

