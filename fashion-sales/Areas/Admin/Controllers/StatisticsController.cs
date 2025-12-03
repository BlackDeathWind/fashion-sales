using fashion_sales.Data;
using fashion_sales.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fashion_sales.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class StatisticsController : Controller
{
    private readonly ApplicationDbContext _context;

    public StatisticsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var today = DateTime.UtcNow.Date;
        var fromDate = today.AddDays(-6); // 7 ngày gần nhất (fromDate..today)

        var data = await _context.StatisticsDaily
            .Where(s => s.StatDate >= fromDate && s.StatDate <= today)
            .OrderBy(s => s.StatDate)
            .ToListAsync();

        // Đảm bảo đủ 7 ngày, nếu thiếu thì fill 0
        var result = new List<StatisticsDaily>();
        for (var d = fromDate; d <= today; d = d.AddDays(1))
        {
            var existing = data.FirstOrDefault(x => x.StatDate.Date == d);
            if (existing != null)
            {
                result.Add(existing);
            }
            else
            {
                result.Add(new StatisticsDaily
                {
                    StatDate = d,
                    TotalRevenue = 0,
                    OrderCount = 0
                });
            }
        }

        return View(result);
    }
}


