using fashion_sales.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace fashion_sales.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        string[] roles = { "Admin", "Staff", "Customer" };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Thông tin tài khoản seed đọc từ cấu hình (appsettings / secrets), không hard-code trong code
        var adminEmail = configuration["SeedUsers:Admin:Email"] ?? "admin@fashionsales.local";
        var adminPassword = configuration["SeedUsers:Admin:Password"] ?? "Admin@12345";
        var adminFullName = configuration["SeedUsers:Admin:FullName"] ?? "Quản trị viên";

        await EnsureUserWithRoleAsync(userManager, adminEmail, adminPassword, adminFullName, "Admin");

        var staffEmail = configuration["SeedUsers:Staff:Email"] ?? "staff@fashionsales.local";
        var staffPassword = configuration["SeedUsers:Staff:Password"] ?? "Staff@12345";
        var staffFullName = configuration["SeedUsers:Staff:FullName"] ?? "Nhân viên bán hàng";

        await EnsureUserWithRoleAsync(userManager, staffEmail, staffPassword, staffFullName, "Staff");
    }

    private static async Task EnsureUserWithRoleAsync(
        UserManager<ApplicationUser> userManager,
        string email,
        string password,
        string fullName,
        string role)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user != null)
        {
            return;
        }

        user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true,
            FullName = fullName,
            DefaultAddress = "TP. Hồ Chí Minh",
            IsActive = true
        };

        var result = await userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, role);
        }
    }
}

