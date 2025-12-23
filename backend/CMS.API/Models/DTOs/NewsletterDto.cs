namespace CMS.API.Models.DTOs;

/// <summary>
/// Newsletter subscription DTO
/// </summary>
public class NewsletterSubscriptionDto
{
    public long Id { get; set; }
    public string Email { get; set; } = null!;
    public bool Active { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime? UnsubscribeDate { get; set; }
}
