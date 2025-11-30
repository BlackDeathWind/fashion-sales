using Microsoft.AspNetCore.Identity;

namespace fashion_sales.Models.Entities;

public class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
    public string? DefaultAddress { get; set; }
    
    // Địa chỉ chi tiết
    public int? ProvinceCode { get; set; }
    public string? ProvinceName { get; set; }
    public int? DistrictCode { get; set; }
    public string? DistrictName { get; set; }
    public int? WardCode { get; set; }
    public string? WardName { get; set; }
    public string? StreetAddress { get; set; }
    
    public bool IsActive { get; set; } = true;
}


