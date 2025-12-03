using System.Diagnostics;
using fashion_sales.Data;
using fashion_sales.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fashion_sales.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Lấy tổng số sản phẩm đang active
            var totalProducts = await _context.Products
                .Where(p => p.IsActive)
                .CountAsync();

            // Lấy tổng số đơn hàng
            var totalOrders = await _context.Orders.CountAsync();

            // Lấy 4 sản phẩm mới nhất đang active, bao gồm Category
            var latestProducts = await _context.Products
                .Where(p => p.IsActive)
                .Include(p => p.Category)
                .OrderByDescending(p => p.CreatedAt)
                .Take(4)
                .ToListAsync();

            ViewBag.TotalProducts = totalProducts;
            ViewBag.TotalOrders = totalOrders;
            ViewBag.LatestProducts = latestProducts;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
