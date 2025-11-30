using System.ComponentModel.DataAnnotations;
using fashion_sales.Models.Entities;
using fashion_sales.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace fashion_sales.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    [HttpGet]
    public IActionResult Register(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        // Validate PhoneOrEmail
        if (string.IsNullOrWhiteSpace(model.PhoneOrEmail))
        {
            ModelState.AddModelError("PhoneOrEmail", "Vui lòng nhập số điện thoại hoặc email");
        }

        // Kiểm tra xem là email hay phone
        bool isEmail = model.PhoneOrEmail.Contains("@");
        
        if (isEmail)
        {
            var emailAttr = new EmailAddressAttribute();
            if (!emailAttr.IsValid(model.PhoneOrEmail))
            {
                ModelState.AddModelError("PhoneOrEmail", "Email không hợp lệ");
            }
        }
        else
        {
            // Validate phone number (chỉ số, có thể có dấu + ở đầu)
            var phone = model.PhoneOrEmail.Trim().Replace(" ", "").Replace("-", "");
            if (phone.StartsWith("+"))
            {
                phone = phone.Substring(1);
            }
            if (!phone.All(char.IsDigit) || phone.Length < 10)
            {
                ModelState.AddModelError("PhoneOrEmail", "Số điện thoại không hợp lệ");
            }
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Tạo UserName từ PhoneOrEmail
        var userName = model.PhoneOrEmail.Trim().ToLowerInvariant();
        
        // Kiểm tra xem UserName đã tồn tại chưa
        var existingUser = await _userManager.FindByNameAsync(userName);
        if (existingUser != null)
        {
            userName = $"{userName}_{Guid.NewGuid().ToString("N")[..8]}";
        }

        var user = new ApplicationUser
        {
            UserName = userName,
            Email = isEmail ? model.PhoneOrEmail.Trim() : null,
            PhoneNumber = !isEmail ? model.PhoneOrEmail.Trim() : null,
            IsActive = true,
            EmailConfirmed = isEmail // Chỉ confirm nếu có email
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            // Gán role Customer mặc định
            if (!await _roleManager.RoleExistsAsync("Customer"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Customer"));
            }

            await _userManager.AddToRoleAsync(user, "Customer");

            await _signInManager.SignInAsync(user, isPersistent: false);
            
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Tìm user bằng Email hoặc UserName
        var user = await _userManager.FindByEmailAsync(model.Email) 
                   ?? await _userManager.FindByNameAsync(model.Email);
        
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Email hoặc tên đăng nhập không đúng.");
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(
            user.UserName!,
            model.Password,
            model.RememberMe,
            lockoutOnFailure: false);

        if (result.Succeeded)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        if (result.IsLockedOut)
        {
            ModelState.AddModelError(string.Empty, "Tài khoản đã bị khóa.");
            return View(model);
        }

        ModelState.AddModelError(string.Empty, "Email hoặc mật khẩu không đúng.");
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }

        var model = new ProfileViewModel
        {
            Email = user.Email,
            FullName = user.FullName,
            PhoneNumber = user.PhoneNumber,
            ProvinceCode = user.ProvinceCode,
            ProvinceName = user.ProvinceName,
            DistrictCode = user.DistrictCode,
            DistrictName = user.DistrictName,
            WardCode = user.WardCode,
            WardName = user.WardName,
            StreetAddress = user.StreetAddress
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Profile(ProfileViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound();
        }

        user.FullName = model.FullName;
        user.PhoneNumber = model.PhoneNumber;
        
        // Cập nhật địa chỉ chi tiết
        user.ProvinceCode = model.ProvinceCode;
        user.ProvinceName = model.ProvinceName;
        user.DistrictCode = model.DistrictCode;
        user.DistrictName = model.DistrictName;
        user.WardCode = model.WardCode;
        user.WardName = model.WardName;
        user.StreetAddress = model.StreetAddress;
        
        // Tạo địa chỉ đầy đủ để lưu vào DefaultAddress
        var addressParts = new List<string>();
        if (!string.IsNullOrWhiteSpace(model.StreetAddress))
            addressParts.Add(model.StreetAddress);
        if (model.WardCode.HasValue && !string.IsNullOrWhiteSpace(model.WardName))
            addressParts.Add(model.WardName);
        if (model.DistrictCode.HasValue && !string.IsNullOrWhiteSpace(model.DistrictName))
            addressParts.Add(model.DistrictName);
        if (model.ProvinceCode.HasValue && !string.IsNullOrWhiteSpace(model.ProvinceName))
            addressParts.Add(model.ProvinceName);
        
        user.DefaultAddress = addressParts.Any() ? string.Join(", ", addressParts) : null;
        
        // Cập nhật email nếu có thay đổi
        if (user.Email != model.Email)
        {
            user.Email = model.Email;
            user.NormalizedEmail = !string.IsNullOrWhiteSpace(model.Email) 
                ? _userManager.NormalizeEmail(model.Email) 
                : null;
        }

        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
            return RedirectToAction(nameof(Profile));
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }
}

