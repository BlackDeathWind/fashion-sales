using System.ComponentModel.DataAnnotations;
using fashion_sales.Models.ViewModels;

namespace fashion_sales.Models.ViewModels;

public class CheckoutViewModel
{
    [Required(ErrorMessage = "Vui lòng chọn Tỉnh/Thành phố")]
    [Display(Name = "Tỉnh/Thành phố")]
    public int? ProvinceCode { get; set; }
    public string? ProvinceName { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn Quận/Huyện")]
    [Display(Name = "Quận/Huyện")]
    public int? DistrictCode { get; set; }
    public string? DistrictName { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn Phường/Xã")]
    [Display(Name = "Phường/Xã")]
    public int? WardCode { get; set; }
    public string? WardName { get; set; }

    [Display(Name = "Địa chỉ chi tiết (Số nhà, tên đường)")]
    public string? StreetAddress { get; set; }

    // Giữ lại để tương thích, sẽ được tạo từ các trường trên
    [Display(Name = "Địa chỉ giao hàng")]
    public string ShippingAddress { get; set; } = null!;

    [Display(Name = "Ghi chú đơn hàng")]
    public string? Note { get; set; }

    public IList<CartItemViewModel> Items { get; set; } = new List<CartItemViewModel>();

    public decimal TotalAmount => Items.Sum(i => i.TotalPrice);
}


