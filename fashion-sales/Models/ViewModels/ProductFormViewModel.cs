using System.ComponentModel.DataAnnotations;
using fashion_sales.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace fashion_sales.Models.ViewModels;

public class ProductFormViewModel
{
    public int? Id { get; set; }

    [Required]
    [Display(Name = "Tên sản phẩm")]
    public string Name { get; set; } = null!;

    [Required]
    [Display(Name = "Danh mục")]
    public int CategoryId { get; set; }

    [Display(Name = "Mô tả")]
    public string? Description { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    [Display(Name = "Giá")]
    public decimal Price { get; set; }

    [Range(0, double.MaxValue)]
    [Display(Name = "Giá khuyến mãi")]
    public decimal? DiscountPrice { get; set; }

    [Range(0, int.MaxValue)]
    [Display(Name = "Tồn kho")]
    public int StockQuantity { get; set; }

    [Display(Name = "Hiển thị")]
    public bool IsActive { get; set; } = true;

    [Display(Name = "Ảnh chính")]
    public IFormFile? MainImage { get; set; }

    public IEnumerable<SelectListItem>? Categories { get; set; }

    public Product? ExistingProduct { get; set; }
}


