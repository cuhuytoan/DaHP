using CMS.API.Models.DTOs;
using CMS.API.Models.Requests;

namespace CMS.API.Services.Interfaces;

/// <summary>
/// Contact service interface for contact form submissions
/// </summary>
public interface IContactService
{
    /// <summary>
    /// Submit a contact form
    /// </summary>
    Task<ContactDto> SubmitAsync(ContactRequest request);

    /// <summary>
    /// Get contact by ID (for admin)
    /// </summary>
    Task<ContactDto?> GetByIdAsync(long id);

    /// <summary>
    /// Get all contacts (for admin)
    /// </summary>
    Task<List<ContactDto>> GetAllAsync(int page = 1, int pageSize = 20);
}
