using System.ComponentModel.DataAnnotations;

namespace fashion_sales.Models.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập số điện thoại hoặc email")]
    [Display(Name = "Số điện thoại hoặc Email")]
    public string PhoneOrEmail { get; set; } = null!;

    [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
    [StringLength(100, ErrorMessage = "Mật khẩu phải có ít nhất {2} ký tự.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Mật khẩu")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Vui lòng đồng ý với điều khoản dịch vụ và chính sách bảo mật")]
    [Display(Name = "Tôi đồng ý với Điều khoản dịch vụ và Chính sách bảo mật")]
    public bool TermsAccepted { get; set; }
}

