using fashion_sales.Data;
using fashion_sales.Models.ViewModels;
using fashion_sales.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fashion_sales.Controllers;

public class CartController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ICartService _cartService;

    public CartController(ApplicationDbContext context, ICartService cartService)
    {
        _context = context;
        _cartService = cartService;
    }

    public IActionResult Index()
    {
        var cart = _cartService.GetCart(HttpContext.Session);
        return View(cart);
    }

    [HttpPost]
    public async Task<IActionResult> Add(int productId, int quantity = 1)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == productId && p.IsActive);

        if (product == null)
        {
            return NotFound();
        }

        var cart = _cartService.GetCart(HttpContext.Session);
        var existing = cart.FirstOrDefault(c => c.ProductId == productId);
        if (existing == null)
        {
            cart.Add(new CartItemViewModel
            {
                ProductId = product.Id,
                ProductName = product.Name,
                UnitPrice = product.DiscountPrice ?? product.Price,
                Quantity = quantity,
                ImageUrl = product.MainImageUrl
            });
        }
        else
        {
            existing.Quantity += quantity;
        }

        _cartService.SaveCart(HttpContext.Session, cart);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Update(int productId, int quantity)
    {
        var cart = _cartService.GetCart(HttpContext.Session);
        var item = cart.FirstOrDefault(c => c.ProductId == productId);
        if (item != null)
        {
            if (quantity <= 0)
            {
                cart.Remove(item);
            }
            else
            {
                item.Quantity = quantity;
            }
        }

        _cartService.SaveCart(HttpContext.Session, cart);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Remove(int productId)
    {
        var cart = _cartService.GetCart(HttpContext.Session);
        var item = cart.FirstOrDefault(c => c.ProductId == productId);
        if (item != null)
        {
            cart.Remove(item);
        }

        _cartService.SaveCart(HttpContext.Session, cart);
        return RedirectToAction("Index");
    }
}


