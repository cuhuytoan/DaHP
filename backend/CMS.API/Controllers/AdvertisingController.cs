using CMS.API.Models.DTOs;
using CMS.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS.API.Controllers;

/// <summary>
/// Public Advertising API controller for banners and ads
/// </summary>
[Route("api/[controller]")]
public class AdvertisingController : BaseController
{
    private readonly IAdvertisingService _advertisingService;
    private readonly ILogger<AdvertisingController> _logger;

    public AdvertisingController(IAdvertisingService advertisingService, ILogger<AdvertisingController> logger)
    {
        _advertisingService = advertisingService;
        _logger = logger;
    }

    /// <summary>
    /// Get all active advertising blocks
    /// </summary>
    /// <returns>List of advertising blocks</returns>
    [HttpGet("blocks")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<List<AdvertisingBlockDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBlocks()
    {
        var blocks = await _advertisingService.GetAllBlocksAsync();
        return ApiSuccess(blocks);
    }

    /// <summary>
    /// Get advertising block by ID with all its active advertisings
    /// </summary>
    /// <param name="blockId">Block ID</param>
    /// <returns>Advertising block with advertisings</returns>
    [HttpGet("block/{blockId:int}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<AdvertisingBlockDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBlock(int blockId)
    {
        var block = await _advertisingService.GetBlockByIdAsync(blockId);
        if (block == null)
        {
            return ApiNotFound("Advertising block not found");
        }
        return ApiSuccess(block);
    }

    /// <summary>
    /// Get active advertisings by block ID
    /// </summary>
    /// <param name="blockId">Block ID</param>
    /// <returns>List of advertisings in the block</returns>
    [HttpGet("by-block/{blockId:int}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<List<AdvertisingDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByBlockId(int blockId)
    {
        var advertisings = await _advertisingService.GetByBlockIdAsync(blockId);
        return ApiSuccess(advertisings);
    }

    /// <summary>
    /// Get single advertising by ID
    /// </summary>
    /// <param name="id">Advertising ID</param>
    /// <returns>Advertising details</returns>
    [HttpGet("{id:long}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<AdvertisingDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id)
    {
        var advertising = await _advertisingService.GetByIdAsync(id);
        if (advertising == null)
        {
            return ApiNotFound("Advertising not found");
        }
        return ApiSuccess(advertising);
    }

    /// <summary>
    /// Track click on advertising (increment counter)
    /// </summary>
    /// <param name="id">Advertising ID</param>
    /// <returns>Success status</returns>
    [HttpPost("{id:long}/click")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> TrackClick(long id)
    {
        var result = await _advertisingService.TrackClickAsync(id);
        if (!result)
        {
            return ApiNotFound("Advertising not found");
        }
        return ApiSuccess(true, "Click tracked successfully");
    }
}
