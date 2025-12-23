namespace CMS.API.Models.Responses;

public class CategoryResponse
{
    public long Id { get; set; }
    public long? ParentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Image { get; set; }
    public string? Url { get; set; }
    public int Sort { get; set; }
    public bool Active { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    public string? CreateBy { get; set; }
    public DateTime? CreateDate { get; set; }
    public string? LastEditBy { get; set; }
    public DateTime? LastEditDate { get; set; }
}

public class CategoryTreeResponse
{
    public long Id { get; set; }
    public long? ParentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Url { get; set; }
    public string? Image { get; set; }
    public int Sort { get; set; }
    public int Level { get; set; }
    public bool HaveChild { get; set; }
    public bool Active { get; set; }
    public List<CategoryTreeResponse> Children { get; set; } = new();
}

public class BreadcrumbItem
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Url { get; set; }
    public int Level { get; set; }
}
