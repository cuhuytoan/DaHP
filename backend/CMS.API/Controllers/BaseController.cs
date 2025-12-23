using System.Security.Claims;
using CMS.API.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CMS.API.Controllers;

/// <summary>
/// Base controller with common functionality
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public abstract class BaseController : ControllerBase
{
    /// <summary>
    /// Gets the current user ID from claims
    /// </summary>
    protected string? CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

    /// <summary>
    /// Gets the current user email from claims
    /// </summary>
    protected string? CurrentUserEmail => User.FindFirstValue(ClaimTypes.Email);

    /// <summary>
    /// Gets the current user name from claims
    /// </summary>
    protected string? CurrentUserName => User.FindFirstValue(ClaimTypes.Name);

    /// <summary>
    /// Checks if the current user is in a specific role
    /// </summary>
    protected bool IsInRole(string role) => User.IsInRole(role);

    /// <summary>
    /// Returns a success response with data
    /// </summary>
    protected IActionResult ApiSuccess<T>(T data, string? message = null)
    {
        return Ok(ApiResponse<T>.SuccessResponse(data, message));
    }

    /// <summary>
    /// Returns a success response with data and pagination
    /// </summary>
    protected IActionResult ApiSuccess<T>(T data, PaginationInfo pagination, string? message = null)
    {
        return Ok(ApiResponse<T>.SuccessResponse(data, pagination, message));
    }

    /// <summary>
    /// Returns an error response
    /// </summary>
    protected IActionResult ApiError(string message, List<string>? errors = null)
    {
        return BadRequest(ApiResponse<object>.ErrorResponse(message, errors));
    }

    /// <summary>
    /// Returns a not found response
    /// </summary>
    protected IActionResult ApiNotFound(string message = "Resource not found")
    {
        return NotFound(ApiResponse<object>.ErrorResponse(message));
    }

    /// <summary>
    /// Returns an unauthorized response
    /// </summary>
    protected IActionResult ApiUnauthorized(string message = "Unauthorized")
    {
        return Unauthorized(ApiResponse<object>.ErrorResponse(message));
    }

    /// <summary>
    /// Returns a forbidden response
    /// </summary>
    protected IActionResult ApiForbidden(string message = "Access denied")
    {
        return StatusCode(StatusCodes.Status403Forbidden, ApiResponse<object>.ErrorResponse(message));
    }

    /// <summary>
    /// Returns a created response
    /// </summary>
    protected IActionResult ApiCreated<T>(T data, string? location = null, string? message = null)
    {
        var response = ApiResponse<T>.SuccessResponse(data, message ?? "Created successfully");
        if (!string.IsNullOrEmpty(location))
        {
            return Created(location, response);
        }
        return StatusCode(StatusCodes.Status201Created, response);
    }

    /// <summary>
    /// Returns a no content response
    /// </summary>
    protected new IActionResult NoContent()
    {
        return StatusCode(StatusCodes.Status204NoContent);
    }

    /// <summary>
    /// Gets model state errors as a list
    /// </summary>
    protected List<string> GetModelErrors()
    {
        return ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();
    }
}