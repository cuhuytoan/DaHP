using System.ComponentModel.DataAnnotations;

namespace CMS.API.Models.Requests;

/// <summary>
/// Request model for newsletter subscription
/// </summary>
public class NewsletterSubscribeRequest
{
    /// <summary>
    /// Email address to subscribe
    /// </summary>
    [Required(ErrorMessage = "Vui lòng nhập email")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    [StringLength(255, ErrorMessage = "Email không được vượt quá 255 ký tự")]
    public string Email { get; set; } = null!;
}

/// <summary>
/// Request model for newsletter unsubscription
/// </summary>
public class NewsletterUnsubscribeRequest
{
    /// <summary>
    /// Email address to unsubscribe
    /// </summary>
    [Required(ErrorMessage = "Vui lòng nhập email")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string Email { get; set; } = null!;
}
