using CMS.API.Models.DTOs;
using CMS.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMS.API.Controllers;

/// <summary>
/// Admin Advertising Management API
/// </summary>
[Route("api/admin/advertising")]
[Authorize(Policy = "EditorOrAdmin")]
public class AdvertisingAdminController : BaseController
{
    private readonly IAdvertisingAdminService _advertisingService;
    private readonly ILogger<AdvertisingAdminController> _logger;

    public AdvertisingAdminController(IAdvertisingAdminService advertisingService, ILogger<AdvertisingAdminController> logger)
    {
        _advertisingService = advertisingService;
        _logger = logger;
    }

    private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

    #region Advertising Block Endpoints

    /// <summary>
    /// Get all advertising blocks with pagination
    /// </summary>
    [HttpGet("blocks")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBlocks(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? keyword = null,
        [FromQuery] bool? active = null)
    {
        try
        {
            var result = await _advertisingService.GetBlocksAsync(page, pageSize, keyword, active);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting advertising blocks");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Get advertising block by ID
    /// </summary>
    [HttpGet("blocks/{id}")]
    [ProducesResponseType(typeof(AdvertisingBlockDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBlockById(long id)
    {
        try
        {
            var result = await _advertisingService.GetBlockDetailByIdAsync(id);
            if (result == null)
                return NotFound(new { message = "Advertising block not found" });

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting advertising block {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Create new advertising block
    /// </summary>
    [HttpPost("blocks")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(AdvertisingBlockDetailDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBlock([FromBody] CreateAdvertisingBlockRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _advertisingService.CreateBlockAsync(request, GetUserId());
            return CreatedAtAction(nameof(GetBlockById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating advertising block");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Update advertising block
    /// </summary>
    [HttpPut("blocks/{id}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(AdvertisingBlockDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateBlock(long id, [FromBody] UpdateAdvertisingBlockRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _advertisingService.UpdateBlockAsync(id, request, GetUserId());
            if (result == null)
                return NotFound(new { message = "Advertising block not found" });

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating advertising block {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Delete advertising block
    /// </summary>
    [HttpDelete("blocks/{id}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteBlock(long id)
    {
        try
        {
            var result = await _advertisingService.DeleteBlockAsync(id);
            if (!result)
                return BadRequest(new { message = "Cannot delete this block. It may be protected or not exist." });

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting advertising block {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Toggle advertising block status
    /// </summary>
    [HttpPut("blocks/{id}/toggle-status")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ToggleBlockStatus(long id)
    {
        try
        {
            var result = await _advertisingService.ToggleBlockStatusAsync(id, GetUserId());
            if (!result)
                return NotFound(new { message = "Advertising block not found" });

            return Ok(new { message = "Status updated successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error toggling advertising block status {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    #endregion

    #region Advertising Endpoints

    /// <summary>
    /// Get all advertisings with pagination
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAdvertisings(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] int? blockId = null,
        [FromQuery] int? typeId = null,
        [FromQuery] string? keyword = null,
        [FromQuery] bool? active = null)
    {
        try
        {
            var result = await _advertisingService.GetAdvertisingsAsync(page, pageSize, blockId, typeId, keyword, active);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting advertisings");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Get advertising by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(AdvertisingDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAdvertisingById(long id)
    {
        try
        {
            var result = await _advertisingService.GetAdvertisingByIdAsync(id);
            if (result == null)
                return NotFound(new { message = "Advertising not found" });

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting advertising {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Create new advertising
    /// </summary>
    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(AdvertisingDetailDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAdvertising([FromBody] CreateAdvertisingRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _advertisingService.CreateAdvertisingAsync(request, GetUserId());
            return CreatedAtAction(nameof(GetAdvertisingById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating advertising");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Update advertising
    /// </summary>
    [HttpPut("{id}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(AdvertisingDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAdvertising(long id, [FromBody] UpdateAdvertisingRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _advertisingService.UpdateAdvertisingAsync(id, request, GetUserId());
            if (result == null)
                return NotFound(new { message = "Advertising not found" });

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating advertising {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Delete advertising
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAdvertising(long id)
    {
        try
        {
            var result = await _advertisingService.DeleteAdvertisingAsync(id);
            if (!result)
                return BadRequest(new { message = "Cannot delete this advertising. It may be protected or not exist." });

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting advertising {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Toggle advertising status
    /// </summary>
    [HttpPut("{id}/toggle-status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ToggleAdvertisingStatus(long id)
    {
        try
        {
            var result = await _advertisingService.ToggleAdvertisingStatusAsync(id, GetUserId());
            if (!result)
                return NotFound(new { message = "Advertising not found" });

            return Ok(new { message = "Status updated successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error toggling advertising status {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Update sort order for advertisings in a block
    /// </summary>
    [HttpPut("blocks/{blockId}/sort")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateSortOrder(long blockId, [FromBody] List<long> advertisingIds)
    {
        try
        {
            await _advertisingService.UpdateSortOrderAsync(blockId, advertisingIds, GetUserId());
            return Ok(new { message = "Sort order updated successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating sort order for block {BlockId}", blockId);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    #endregion

    #region Advertising Type Endpoints

    /// <summary>
    /// Get all advertising types
    /// </summary>
    [HttpGet("types")]
    [ProducesResponseType(typeof(List<AdvertisingTypeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTypes()
    {
        try
        {
            var result = await _advertisingService.GetTypesAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting advertising types");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    #endregion
}
