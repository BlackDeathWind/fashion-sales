using fashion_sales.Data;
using fashion_sales.Models.Entities;
using fashion_sales.Models.ViewModels;
using fashion_sales.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fashion_sales.Controllers;

[Authorize(Roles = "Customer")]
public class OrderController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ICartService _cartService;
    private readonly UserManager<ApplicationUser> _userManager;

    public OrderController(
        ApplicationDbContext context,
        ICartService cartService,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _cartService = cartService;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Checkout()
    {
        var cart = _cartService.GetCart(HttpContext.Session);
        if (!cart.Any())
        {
            return RedirectToAction("Index", "Cart");
        }

        var user = await _userManager.GetUserAsync(User);
        var model = new CheckoutViewModel
        {
            ProvinceCode = user?.ProvinceCode,
            ProvinceName = user?.ProvinceName,
            DistrictCode = user?.DistrictCode,
            DistrictName = user?.DistrictName,
            WardCode = user?.WardCode,
            WardName = user?.WardName,
            StreetAddress = user?.StreetAddress,
            ShippingAddress = user?.DefaultAddress ?? string.Empty,
            Items = cart
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(CheckoutViewModel model)
    {
        var cart = _cartService.GetCart(HttpContext.Session);
        if (!cart.Any())
        {
            ModelState.AddModelError(string.Empty, "Giỏ hàng đang trống.");
        }

        // Validate address fields
        if (!model.ProvinceCode.HasValue || !model.DistrictCode.HasValue || !model.WardCode.HasValue || string.IsNullOrWhiteSpace(model.StreetAddress))
        {
            TempData["ErrorMessage"] = "Vui lòng cập nhật đầy đủ thông tin địa chỉ giao hàng trước khi đặt hàng.";
            return RedirectToAction("Profile", "Account");
        }

        if (!ModelState.IsValid)
        {
            model.Items = cart;
            return View(model);
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Challenge();
        }

        // Tạo địa chỉ đầy đủ từ các thành phần
        var addressParts = new List<string>();
        if (!string.IsNullOrWhiteSpace(model.StreetAddress))
            addressParts.Add(model.StreetAddress);
        if (model.WardCode.HasValue && !string.IsNullOrWhiteSpace(model.WardName))
            addressParts.Add(model.WardName);
        if (model.DistrictCode.HasValue && !string.IsNullOrWhiteSpace(model.DistrictName))
            addressParts.Add(model.DistrictName);
        if (model.ProvinceCode.HasValue && !string.IsNullOrWhiteSpace(model.ProvinceName))
            addressParts.Add(model.ProvinceName);

        var fullAddress = addressParts.Any() ? string.Join(", ", addressParts) : model.ShippingAddress;

        var order = new Order
        {
            OrderCode = $"FS{DateTime.UtcNow:yyyyMMddHHmmss}",
            UserId = user.Id,
            OrderDate = DateTime.UtcNow,
            ShippingAddress = fullAddress,
            ProvinceCode = model.ProvinceCode,
            ProvinceName = model.ProvinceName,
            DistrictCode = model.DistrictCode,
            DistrictName = model.DistrictName,
            WardCode = model.WardCode,
            WardName = model.WardName,
            StreetAddress = model.StreetAddress,
            Note = model.Note,
            Status = OrderStatus.Pending,
            TotalAmount = cart.Sum(i => i.TotalPrice),
            Items = cart.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                TotalPrice = i.TotalPrice
            }).ToList()
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        _cartService.SaveCart(HttpContext.Session, new List<CartItemViewModel>());

        return RedirectToAction(nameof(Success), new { code = order.OrderCode });
    }

    [HttpGet]
    public async Task<IActionResult> Success(string code)
    {
        if (string.IsNullOrEmpty(code))
        {
            return RedirectToAction("Index", "Home");
        }

        var order = await _context.Orders
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.OrderCode == code);

        if (order == null)
        {
            return RedirectToAction("Index", "Home");
        }

        return View(order);
    }

    [HttpGet]
    public async Task<IActionResult> MyOrders()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Challenge();
        }

        var orders = await _context.Orders
            .Where(o => o.UserId == user.Id)
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();

        return View(orders);
    }

    [HttpGet]
    public async Task<IActionResult> Details(string code)
    {
        if (string.IsNullOrEmpty(code))
        {
            return RedirectToAction("MyOrders");
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Challenge();
        }

        var order = await _context.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.OrderCode == code && o.UserId == user.Id);

        if (order == null)
        {
            return RedirectToAction("MyOrders");
        }

        return View(order);
    }
}


