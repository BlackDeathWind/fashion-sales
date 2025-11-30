using fashion_sales.Data;
using fashion_sales.Models.Entities;
using fashion_sales.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        
        // Cấu hình mật khẩu
        options.Password.RequireUppercase = false; // Không bắt buộc chữ hoa
        options.Password.RequireLowercase = false; // Không bắt buộc chữ thường
        options.Password.RequireDigit = false; // Không bắt buộc số
        options.Password.RequireNonAlphanumeric = false; // Không bắt buộc ký tự đặc biệt
        options.Password.RequiredLength = 6; // Chỉ yêu cầu tối thiểu 6 ký tự
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSession();
builder.Services.AddScoped<ICartService, CartService>();

var app = builder.Build();
await DbInitializer.SeedAsync(app.Services);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
