using System.ComponentModel.DataAnnotations;

namespace fashion_sales.Models.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "Số điện thoại hoặc email là bắt buộc")]
    [Display(Name = "Số điện thoại hoặc Email")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
    [DataType(DataType.Password)]
    [Display(Name = "Mật khẩu")]
    public string Password { get; set; } = null!;

    [Display(Name = "Ghi nhớ đăng nhập")]
    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }
}

