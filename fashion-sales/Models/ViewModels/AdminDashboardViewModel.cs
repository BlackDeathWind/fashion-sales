using fashion_sales.Models.Entities;

namespace fashion_sales.Models.ViewModels;

public class AdminDashboardViewModel
{
    public int TotalProducts { get; set; }
    public int ActiveProducts { get; set; }

    public int TotalOrders { get; set; }
    public int PendingOrders { get; set; }
    public int ProcessingOrders { get; set; }
    public int CompletedOrders { get; set; }
    public int CancelledOrders { get; set; }

    public decimal TotalRevenue { get; set; }
    public decimal TodayRevenue { get; set; }
    public int TodayOrders { get; set; }
    public int UniqueCustomers { get; set; }

    public List<TopProductStat> TopProducts { get; set; } = new();
}

public class TopProductStat
{
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int QuantitySold { get; set; }
    public decimal Revenue { get; set; }
}