using System.ComponentModel.DataAnnotations;

namespace CMS.API.Models.Requests;

/// <summary>
/// Request model for contact form submission
/// </summary>
public class ContactRequest
{
    /// <summary>
    /// Full name of the person submitting the contact form
    /// </summary>
    [Required(ErrorMessage = "Vui lòng nhập họ tên")]
    [StringLength(200, ErrorMessage = "Họ tên không được vượt quá 200 ký tự")]
    public string FullName { get; set; } = null!;

    /// <summary>
    /// Email address
    /// </summary>
    [Required(ErrorMessage = "Vui lòng nhập email")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    [StringLength(255, ErrorMessage = "Email không được vượt quá 255 ký tự")]
    public string Email { get; set; } = null!;

    /// <summary>
    /// Phone number (optional)
    /// </summary>
    [StringLength(20, ErrorMessage = "Số điện thoại không được vượt quá 20 ký tự")]
    [RegularExpression(@"^[0-9+\-\s]*$", ErrorMessage = "Số điện thoại không hợp lệ")]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Message/description content
    /// </summary>
    [Required(ErrorMessage = "Vui lòng nhập nội dung")]
    [StringLength(2000, ErrorMessage = "Nội dung không được vượt quá 2000 ký tự")]
    public string Descriptions { get; set; } = null!;
}
