using CMS.API.Models.DTOs;
using CMS.API.Models.Requests;
using CMS.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS.API.Controllers;

/// <summary>
/// Contact API controller for contact form submissions
/// </summary>
[Route("api/[controller]")]
public class ContactController : BaseController
{
    private readonly IContactService _contactService;
    private readonly ILogger<ContactController> _logger;

    public ContactController(IContactService contactService, ILogger<ContactController> logger)
    {
        _contactService = contactService;
        _logger = logger;
    }

    /// <summary>
    /// Submit a contact form (public endpoint)
    /// </summary>
    /// <param name="request">Contact form data</param>
    /// <returns>Created contact</returns>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<ContactDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Submit([FromBody] ContactRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ApiError("Dữ liệu không hợp lệ", GetModelErrors());
        }

        var contact = await _contactService.SubmitAsync(request);
        return ApiCreated(contact, message: "Cảm ơn bạn đã liên hệ. Chúng tôi sẽ phản hồi sớm nhất có thể.");
    }

    /// <summary>
    /// Get all contacts (admin only)
    /// </summary>
    /// <param name="page">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <returns>List of contacts</returns>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<List<ContactDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var contacts = await _contactService.GetAllAsync(page, pageSize);
        return ApiSuccess(contacts);
    }

    /// <summary>
    /// Get contact by ID (admin only)
    /// </summary>
    /// <param name="id">Contact ID</param>
    /// <returns>Contact details</returns>
    [HttpGet("{id:long}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<ContactDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetById(long id)
    {
        var contact = await _contactService.GetByIdAsync(id);
        if (contact == null)
        {
            return ApiNotFound("Contact not found");
        }
        return ApiSuccess(contact);
    }
}
