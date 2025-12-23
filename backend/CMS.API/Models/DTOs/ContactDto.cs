namespace CMS.API.Models.DTOs;

/// <summary>
/// Contact DTO for responses
/// </summary>
public class ContactDto
{
    public long Id { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Descriptions { get; set; }
    public DateTime? CreateDate { get; set; }
}
