using fashion_sales.Data;
using fashion_sales.Models.Entities;
using fashion_sales.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fashion_sales.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Staff")]
public class DashboardController : Controller
{
    private readonly ApplicationDbContext _context;

    public DashboardController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var today = DateTime.UtcNow.Date;

        var ordersQuery = _context.Orders.AsQueryable();
        var completedOrdersQuery = ordersQuery.Where(o => o.Status == OrderStatus.Completed);

        var model = new AdminDashboardViewModel
        {
            TotalProducts = await _context.Products.CountAsync(),
            ActiveProducts = await _context.Products.CountAsync(p => p.IsActive),
            TotalOrders = await ordersQuery.CountAsync(),
            PendingOrders = await ordersQuery.CountAsync(o => o.Status == OrderStatus.Pending),
            ProcessingOrders = await ordersQuery.CountAsync(o => o.Status == OrderStatus.Processing || o.Status == OrderStatus.Shipping),
            CompletedOrders = await ordersQuery.CountAsync(o => o.Status == OrderStatus.Completed),
            CancelledOrders = await ordersQuery.CountAsync(o => o.Status == OrderStatus.Cancelled),
            TotalRevenue = await completedOrdersQuery.SumAsync(o => (decimal?)o.TotalAmount) ?? 0m,
            TodayRevenue = await completedOrdersQuery
                .Where(o => o.OrderDate.Date == today)
                .SumAsync(o => (decimal?)o.TotalAmount) ?? 0m,
            TodayOrders = await ordersQuery.CountAsync(o => o.OrderDate.Date == today),
            UniqueCustomers = await ordersQuery
                .Select(o => o.UserId)
                .Distinct()
                .CountAsync()
        };

        model.TopProducts = await _context.OrderItems
            .Include(oi => oi.Product)
            .GroupBy(oi => new { oi.ProductId, oi.Product.Name })
            .Select(g => new TopProductStat
            {
                ProductId = g.Key.ProductId,
                Name = g.Key.Name,
                QuantitySold = g.Sum(x => x.Quantity),
                Revenue = g.Sum(x => x.TotalPrice)
            })
            .OrderByDescending(x => x.QuantitySold)
            .Take(5)
            .ToListAsync();

        return View(model);
    }
}

