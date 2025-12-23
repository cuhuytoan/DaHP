using System.ComponentModel.DataAnnotations;

namespace CMS.API.Models.Requests;

public class CreateCategoryRequest
{
    public int? ParentId { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? Image { get; set; }

    public string? Url { get; set; }

    public int Sort { get; set; } = 0;

    public bool Active { get; set; } = true;

    // SEO Fields
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
}

public class UpdateCategoryRequest
{
    public int? ParentId { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string? Image { get; set; }

    public string? Url { get; set; }

    public int Sort { get; set; } = 0;

    public bool Active { get; set; } = true;

    // SEO Fields
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
}
