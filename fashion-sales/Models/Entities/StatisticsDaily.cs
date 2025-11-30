namespace fashion_sales.Models.Entities;

public class StatisticsDaily
{
    public DateTime StatDate { get; set; }
    public decimal TotalRevenue { get; set; }
    public int OrderCount { get; set; }
}


