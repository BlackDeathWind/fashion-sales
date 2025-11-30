using System.ComponentModel.DataAnnotations;

namespace fashion_sales.Models.ViewModels;

public class ProfileViewModel
{
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    [Display(Name = "Email (tùy chọn)")]
    public string? Email { get; set; }

    [StringLength(100)]
    [Display(Name = "Họ và tên (tùy chọn)")]
    public string? FullName { get; set; }

    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    [Display(Name = "Số điện thoại")]
    public string? PhoneNumber { get; set; }

    [Display(Name = "Tỉnh/Thành phố")]
    public int? ProvinceCode { get; set; }
    public string? ProvinceName { get; set; }

    [Display(Name = "Quận/Huyện")]
    public int? DistrictCode { get; set; }
    public string? DistrictName { get; set; }

    [Display(Name = "Phường/Xã")]
    public int? WardCode { get; set; }
    public string? WardName { get; set; }

    [Display(Name = "Địa chỉ chi tiết (Số nhà, tên đường)")]
    public string? StreetAddress { get; set; }
}

