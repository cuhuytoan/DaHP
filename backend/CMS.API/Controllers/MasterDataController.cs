using CMS.API.Models.DTOs;
using CMS.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS.API.Controllers;

/// <summary>
/// Master Data API for lookups and reference data
/// </summary>
[Route("api/admin/master-data")]
[Authorize(Policy = "EditorOrAdmin")]
public class MasterDataController : BaseController
{
    private readonly IMasterDataService _masterDataService;
    private readonly ILogger<MasterDataController> _logger;

    public MasterDataController(IMasterDataService masterDataService, ILogger<MasterDataController> logger)
    {
        _masterDataService = masterDataService;
        _logger = logger;
    }

    #region Units

    /// <summary>
    /// Get all units
    /// </summary>
    [HttpGet("units")]
    [ProducesResponseType(typeof(List<UnitDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUnits()
    {
        try
        {
            var result = await _masterDataService.GetUnitsAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting units");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Get unit by ID
    /// </summary>
    [HttpGet("units/{id}")]
    [ProducesResponseType(typeof(UnitDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUnitById(long id)
    {
        try
        {
            var result = await _masterDataService.GetUnitByIdAsync(id);
            if (result == null)
                return NotFound(new { message = "Unit not found" });

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting unit {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Create new unit
    /// </summary>
    [HttpPost("units")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(UnitDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateUnit([FromBody] CreateUnitRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _masterDataService.CreateUnitAsync(request);
            return CreatedAtAction(nameof(GetUnitById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating unit");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Update unit
    /// </summary>
    [HttpPut("units/{id}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(UnitDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUnit(long id, [FromBody] UpdateUnitRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _masterDataService.UpdateUnitAsync(id, request);
            if (result == null)
                return NotFound(new { message = "Unit not found" });

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating unit {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Delete unit
    /// </summary>
    [HttpDelete("units/{id}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteUnit(long id)
    {
        try
        {
            var result = await _masterDataService.DeleteUnitAsync(id);
            if (!result)
                return BadRequest(new { message = "Cannot delete unit. It may be in use or not exist." });

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting unit {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    #endregion

    #region Manufacturers

    /// <summary>
    /// Get all manufacturers
    /// </summary>
    [HttpGet("manufacturers")]
    [ProducesResponseType(typeof(List<ManufacturerDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetManufacturers()
    {
        try
        {
            var result = await _masterDataService.GetManufacturersAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting manufacturers");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Get manufacturer by ID
    /// </summary>
    [HttpGet("manufacturers/{id}")]
    [ProducesResponseType(typeof(ManufacturerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetManufacturerById(long id)
    {
        try
        {
            var result = await _masterDataService.GetManufacturerByIdAsync(id);
            if (result == null)
                return NotFound(new { message = "Manufacturer not found" });

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting manufacturer {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Create new manufacturer
    /// </summary>
    [HttpPost("manufacturers")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(ManufacturerDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateManufacturer([FromBody] CreateManufacturerRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _masterDataService.CreateManufacturerAsync(request);
            return CreatedAtAction(nameof(GetManufacturerById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating manufacturer");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Update manufacturer
    /// </summary>
    [HttpPut("manufacturers/{id}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(ManufacturerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateManufacturer(long id, [FromBody] UpdateManufacturerRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _masterDataService.UpdateManufacturerAsync(id, request);
            if (result == null)
                return NotFound(new { message = "Manufacturer not found" });

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating manufacturer {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Delete manufacturer
    /// </summary>
    [HttpDelete("manufacturers/{id}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteManufacturer(long id)
    {
        try
        {
            var result = await _masterDataService.DeleteManufacturerAsync(id);
            if (!result)
                return BadRequest(new { message = "Cannot delete manufacturer. It may be in use or not exist." });

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting manufacturer {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    #endregion

    #region Order Statuses

    /// <summary>
    /// Get all order statuses
    /// </summary>
    [HttpGet("order-statuses")]
    [ProducesResponseType(typeof(List<OrderStatusDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrderStatuses()
    {
        try
        {
            var result = await _masterDataService.GetOrderStatusesAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting order statuses");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    #endregion

    #region Payment Methods

    /// <summary>
    /// Get all payment methods
    /// </summary>
    [HttpGet("payment-methods")]
    [ProducesResponseType(typeof(List<PaymentMethodDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaymentMethods([FromQuery] bool? activeOnly = null)
    {
        try
        {
            var result = await _masterDataService.GetPaymentMethodsAsync(activeOnly);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting payment methods");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Get payment method by ID
    /// </summary>
    [HttpGet("payment-methods/{id}")]
    [ProducesResponseType(typeof(PaymentMethodDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPaymentMethodById(long id)
    {
        try
        {
            var result = await _masterDataService.GetPaymentMethodByIdAsync(id);
            if (result == null)
                return NotFound(new { message = "Payment method not found" });

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting payment method {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Create new payment method
    /// </summary>
    [HttpPost("payment-methods")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(PaymentMethodDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreatePaymentMethod([FromBody] CreatePaymentMethodRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _masterDataService.CreatePaymentMethodAsync(request);
            return CreatedAtAction(nameof(GetPaymentMethodById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating payment method");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Update payment method
    /// </summary>
    [HttpPut("payment-methods/{id}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(PaymentMethodDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePaymentMethod(long id, [FromBody] UpdatePaymentMethodRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _masterDataService.UpdatePaymentMethodAsync(id, request);
            if (result == null)
                return NotFound(new { message = "Payment method not found" });

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating payment method {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Delete payment method
    /// </summary>
    [HttpDelete("payment-methods/{id}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeletePaymentMethod(long id)
    {
        try
        {
            var result = await _masterDataService.DeletePaymentMethodAsync(id);
            if (!result)
                return BadRequest(new { message = "Cannot delete payment method. It may be in use or not exist." });

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting payment method {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Toggle payment method status
    /// </summary>
    [HttpPut("payment-methods/{id}/toggle-status")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> TogglePaymentMethodStatus(long id)
    {
        try
        {
            var result = await _masterDataService.TogglePaymentMethodStatusAsync(id);
            if (!result)
                return NotFound(new { message = "Payment method not found" });

            return Ok(new { message = "Status updated successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error toggling payment method status {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    #endregion

    #region Payment Statuses

    /// <summary>
    /// Get all payment statuses
    /// </summary>
    [HttpGet("payment-statuses")]
    [ProducesResponseType(typeof(List<PaymentStatusDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaymentStatuses()
    {
        try
        {
            var result = await _masterDataService.GetPaymentStatusesAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting payment statuses");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    #endregion

    #region Product Statuses

    /// <summary>
    /// Get all product statuses
    /// </summary>
    [HttpGet("product-statuses")]
    [ProducesResponseType(typeof(List<ProductStatusDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductStatuses()
    {
        try
        {
            var result = await _masterDataService.GetProductStatusesAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting product statuses");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    #endregion

    #region Article Types

    /// <summary>
    /// Get all article types
    /// </summary>
    [HttpGet("article-types")]
    [ProducesResponseType(typeof(List<ArticleTypeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetArticleTypes()
    {
        try
        {
            var result = await _masterDataService.GetArticleTypesAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting article types");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    #endregion

    #region Article Statuses

    /// <summary>
    /// Get all article statuses
    /// </summary>
    [HttpGet("article-statuses")]
    [ProducesResponseType(typeof(List<ArticleStatusDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetArticleStatuses()
    {
        try
        {
            var result = await _masterDataService.GetArticleStatusesAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting article statuses");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    #endregion

    #region Banks

    /// <summary>
    /// Get all banks
    /// </summary>
    [HttpGet("banks")]
    [ProducesResponseType(typeof(List<BankDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBanks([FromQuery] bool? activeOnly = null)
    {
        try
        {
            var result = await _masterDataService.GetBanksAsync(activeOnly);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting banks");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    #endregion

    #region Product Properties

    /// <summary>
    /// Get all property categories
    /// </summary>
    [HttpGet("property-categories")]
    [ProducesResponseType(typeof(List<PropertyCategoryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPropertyCategories([FromQuery] int? productCategoryId = null)
    {
        try
        {
            var result = await _masterDataService.GetPropertyCategoriesAsync(productCategoryId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting property categories");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Get all property types
    /// </summary>
    [HttpGet("property-types")]
    [ProducesResponseType(typeof(List<PropertyTypeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPropertyTypes()
    {
        try
        {
            var result = await _masterDataService.GetPropertyTypesAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting property types");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Get all properties
    /// </summary>
    [HttpGet("properties")]
    [ProducesResponseType(typeof(List<PropertyDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProperties(
        [FromQuery] int? productCategoryId = null,
        [FromQuery] int? propertyCategoryId = null)
    {
        try
        {
            var result = await _masterDataService.GetPropertiesAsync(productCategoryId, propertyCategoryId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting properties");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    #endregion
}
