namespace fashion_sales.Models.Entities;

public enum OrderStatus
{
    Pending,
    Processing,
    Shipping,
    Completed,
    Cancelled
}

public class Order
{
    public int Id { get; set; }
    public string OrderCode { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public string ShippingAddress { get; set; } = null!;
    
    // Địa chỉ chi tiết
    public int? ProvinceCode { get; set; }
    public string? ProvinceName { get; set; }
    public int? DistrictCode { get; set; }
    public string? DistrictName { get; set; }
    public int? WardCode { get; set; }
    public string? WardName { get; set; }
    public string? StreetAddress { get; set; }
    
    public string? Note { get; set; }

    public ApplicationUser User { get; set; } = null!;
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}


