using CMS.API.Models.DTOs;
using CMS.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CMS.API.Controllers;

/// <summary>
/// Admin Content Blocks Management API
/// </summary>
[Route("api/admin/blocks")]
[Authorize(Policy = "EditorOrAdmin")]
public class BlocksController : BaseController
{
    private readonly IBlockService _blockService;
    private readonly ILogger<BlocksController> _logger;

    public BlocksController(IBlockService blockService, ILogger<BlocksController> logger)
    {
        _blockService = blockService;
        _logger = logger;
    }

    private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

    #region Article Block Endpoints

    /// <summary>
    /// Get all article blocks with pagination
    /// </summary>
    [HttpGet("articles")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetArticleBlocks(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] int? categoryId = null,
        [FromQuery] string? keyword = null,
        [FromQuery] bool? active = null)
    {
        try
        {
            var result = await _blockService.GetArticleBlocksAsync(page, pageSize, categoryId, keyword, active);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting article blocks");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Get article block by ID
    /// </summary>
    [HttpGet("articles/{id}")]
    [ProducesResponseType(typeof(ArticleBlockDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetArticleBlockById(long id)
    {
        try
        {
            var result = await _blockService.GetArticleBlockByIdAsync(id);
            if (result == null)
                return NotFound(new { message = "Article block not found" });

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting article block {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Create new article block
    /// </summary>
    [HttpPost("articles")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(ArticleBlockDetailDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateArticleBlock([FromBody] CreateArticleBlockRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _blockService.CreateArticleBlockAsync(request, GetUserId());
            return CreatedAtAction(nameof(GetArticleBlockById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating article block");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Update article block
    /// </summary>
    [HttpPut("articles/{id}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(ArticleBlockDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateArticleBlock(long id, [FromBody] UpdateArticleBlockRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _blockService.UpdateArticleBlockAsync(id, request, GetUserId());
            if (result == null)
                return NotFound(new { message = "Article block not found" });

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating article block {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Delete article block
    /// </summary>
    [HttpDelete("articles/{id}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteArticleBlock(long id)
    {
        try
        {
            var result = await _blockService.DeleteArticleBlockAsync(id);
            if (!result)
                return BadRequest(new { message = "Cannot delete this block. It may be protected or not exist." });

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting article block {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Toggle article block status
    /// </summary>
    [HttpPut("articles/{id}/toggle-status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ToggleArticleBlockStatus(long id)
    {
        try
        {
            var result = await _blockService.ToggleArticleBlockStatusAsync(id, GetUserId());
            if (!result)
                return NotFound(new { message = "Article block not found" });

            return Ok(new { message = "Status updated successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error toggling article block status {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Add articles to block
    /// </summary>
    [HttpPost("articles/{id}/items")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddArticlesToBlock(long id, [FromBody] AddItemsToBlockRequest request)
    {
        try
        {
            var result = await _blockService.AddArticlesToBlockAsync(id, request.ItemIds);
            if (!result)
                return NotFound(new { message = "Article block not found" });

            return Ok(new { message = "Articles added successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding articles to block {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Remove article from block
    /// </summary>
    [HttpDelete("articles/{blockId}/items/{articleId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveArticleFromBlock(long blockId, int articleId)
    {
        try
        {
            var result = await _blockService.RemoveArticleFromBlockAsync(blockId, articleId);
            if (!result)
                return NotFound(new { message = "Article not found in block" });

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing article {ArticleId} from block {BlockId}", articleId, blockId);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    #endregion

    #region Product Block Endpoints

    /// <summary>
    /// Get all product blocks with pagination
    /// </summary>
    [HttpGet("products")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductBlocks(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] int? categoryId = null,
        [FromQuery] string? keyword = null,
        [FromQuery] bool? active = null)
    {
        try
        {
            var result = await _blockService.GetProductBlocksAsync(page, pageSize, categoryId, keyword, active);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting product blocks");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Get product block by ID
    /// </summary>
    [HttpGet("products/{id}")]
    [ProducesResponseType(typeof(ProductBlockDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductBlockById(long id)
    {
        try
        {
            var result = await _blockService.GetProductBlockByIdAsync(id);
            if (result == null)
                return NotFound(new { message = "Product block not found" });

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting product block {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Create new product block
    /// </summary>
    [HttpPost("products")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(ProductBlockDetailDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProductBlock([FromBody] CreateProductBlockRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _blockService.CreateProductBlockAsync(request, GetUserId());
            return CreatedAtAction(nameof(GetProductBlockById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product block");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Update product block
    /// </summary>
    [HttpPut("products/{id}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(typeof(ProductBlockDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProductBlock(long id, [FromBody] UpdateProductBlockRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _blockService.UpdateProductBlockAsync(id, request, GetUserId());
            if (result == null)
                return NotFound(new { message = "Product block not found" });

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating product block {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Delete product block
    /// </summary>
    [HttpDelete("products/{id}")]
    [Authorize(Policy = "AdminOnly")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteProductBlock(long id)
    {
        try
        {
            var result = await _blockService.DeleteProductBlockAsync(id);
            if (!result)
                return BadRequest(new { message = "Cannot delete this block. It may be protected or not exist." });

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product block {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Toggle product block status
    /// </summary>
    [HttpPut("products/{id}/toggle-status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ToggleProductBlockStatus(long id)
    {
        try
        {
            var result = await _blockService.ToggleProductBlockStatusAsync(id, GetUserId());
            if (!result)
                return NotFound(new { message = "Product block not found" });

            return Ok(new { message = "Status updated successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error toggling product block status {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Add products to block
    /// </summary>
    [HttpPost("products/{id}/items")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddProductsToBlock(long id, [FromBody] AddItemsToBlockRequest request)
    {
        try
        {
            var result = await _blockService.AddProductsToBlockAsync(id, request.ItemIds);
            if (!result)
                return NotFound(new { message = "Product block not found" });

            return Ok(new { message = "Products added successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding products to block {Id}", id);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    /// <summary>
    /// Remove product from block
    /// </summary>
    [HttpDelete("products/{blockId}/items/{productId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveProductFromBlock(long blockId, int productId)
    {
        try
        {
            var result = await _blockService.RemoveProductFromBlockAsync(blockId, productId);
            if (!result)
                return NotFound(new { message = "Product not found in block" });

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing product {ProductId} from block {BlockId}", productId, blockId);
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    #endregion
}
