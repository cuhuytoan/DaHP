using CMS.API.Models.DTOs;
using CMS.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CMS.API.Controllers;

/// <summary>
/// Products API controller
/// </summary>
[Route("api/[controller]")]
public class ProductsController : BaseController
{
    private readonly IProductService _productService;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductService productService, ILogger<ProductsController> logger)
    {
        _productService = productService;
        _logger = logger;
    }

    /// <summary>
    /// Get all products with pagination and filtering
    /// </summary>
    /// <param name="request">Search parameters</param>
    /// <returns>Paginated list of products</returns>
    [HttpGet]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<List<ProductListDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProducts([FromQuery] ProductSearchRequest request)
    {
        var result = await _productService.SearchAsync(request);
        return ApiSuccess(result.Items, result.Pagination);
    }

    /// <summary>
    /// Get product by ID
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <returns>Product details</returns>
    [HttpGet("{id:long}")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<ProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProduct(long id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
        {
            return ApiNotFound("Product not found");
        }
        return ApiSuccess(product);
    }

    /// <summary>
    /// Get product by URL slug
    /// </summary>
    /// <param name="url">Product URL slug</param>
    /// <returns>Product details</returns>
    [HttpGet("url/{url}")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<ProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductByUrl(string url)
    {
        var product = await _productService.GetByUrlAsync(url);
        if (product == null)
        {
            return ApiNotFound("Product not found");
        }
        
        // Increment view counter
        await _productService.IncrementCounterAsync(product.Id);
        
        return ApiSuccess(product);
    }

    /// <summary>
    /// Get product by code
    /// </summary>
    /// <param name="code">Product code</param>
    /// <returns>Product details</returns>
    [HttpGet("code/{code}")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<ProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductByCode(string code)
    {
        var product = await _productService.GetByCodeAsync(code);
        if (product == null)
        {
            return ApiNotFound("Product not found");
        }
        return ApiSuccess(product);
    }

    /// <summary>
    /// Create a new product
    /// </summary>
    /// <param name="request">Product data</param>
    /// <returns>Created product</returns>
    [HttpPost]
    [Authorize(Roles = "Admin,Editor")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<ProductDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ApiError("Invalid request", GetModelErrors());
        }

        var userId = CurrentUserId;
        if (string.IsNullOrEmpty(userId))
        {
            return ApiUnauthorized();
        }

        var product = await _productService.CreateAsync(request, userId);
        return ApiCreated(product, $"/api/products/{product.Id}", "Product created successfully");
    }

    /// <summary>
    /// Update an existing product
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <param name="request">Updated product data</param>
    /// <returns>Updated product</returns>
    [HttpPut("{id:long}")]
    [Authorize(Roles = "Admin,Editor")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<ProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateProduct(long id, [FromBody] UpdateProductRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ApiError("Invalid request", GetModelErrors());
        }

        var userId = CurrentUserId;
        if (string.IsNullOrEmpty(userId))
        {
            return ApiUnauthorized();
        }

        var product = await _productService.UpdateAsync(id, request, userId);
        if (product == null)
        {
            return ApiNotFound("Product not found");
        }

        return ApiSuccess(product, "Product updated successfully");
    }

    /// <summary>
    /// Delete a product
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <returns>No content</returns>
    [HttpDelete("{id:long}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteProduct(long id)
    {
        var userId = CurrentUserId;
        if (string.IsNullOrEmpty(userId))
        {
            return ApiUnauthorized();
        }

        var result = await _productService.DeleteAsync(id, userId);
        if (!result)
        {
            return ApiNotFound("Product not found");
        }

        return NoContent();
    }

    /// <summary>
    /// Approve a product
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <returns>Success status</returns>
    [HttpPost("{id:long}/approve")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ApproveProduct(long id)
    {
        var userId = CurrentUserId;
        if (string.IsNullOrEmpty(userId))
        {
            return ApiUnauthorized();
        }

        var result = await _productService.ApproveAsync(id, userId);
        if (!result)
        {
            return ApiNotFound("Product not found");
        }

        return ApiSuccess(true, "Product approved successfully");
    }

    /// <summary>
    /// Publish a product
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <returns>Success status</returns>
    [HttpPost("{id:long}/publish")]
    [Authorize(Roles = "Admin,Editor")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PublishProduct(long id)
    {
        var userId = CurrentUserId;
        if (string.IsNullOrEmpty(userId))
        {
            return ApiUnauthorized();
        }

        var result = await _productService.PublishAsync(id, userId);
        if (!result)
        {
            return ApiNotFound("Product not found");
        }

        return ApiSuccess(true, "Product published successfully");
    }

    /// <summary>
    /// Get product reviews
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <param name="page">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <returns>Paginated list of reviews</returns>
    [HttpGet("{id:long}/reviews")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<List<ProductReviewDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductReviews(long id, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _productService.GetReviewsAsync(id, page, pageSize);
        return ApiSuccess(result.Items, result.Pagination);
    }

    /// <summary>
    /// Add a review to a product
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <param name="request">Review data</param>
    /// <returns>Created review</returns>
    [HttpPost("{id:long}/reviews")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<ProductReviewDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddProductReview(long id, [FromBody] CreateProductReviewRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ApiError("Invalid request", GetModelErrors());
        }

        request.ProductId = id;
        var review = await _productService.CreateReviewAsync(request, CurrentUserId);
        return ApiCreated(review, message: "Review added successfully");
    }

    /// <summary>
    /// Get product comments
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <param name="page">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <returns>Paginated list of comments</returns>
    [HttpGet("{id:long}/comments")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<List<ProductCommentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProductComments(long id, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _productService.GetCommentsAsync(id, page, pageSize);
        return ApiSuccess(result.Items, result.Pagination);
    }

    /// <summary>
    /// Add a comment to a product
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <param name="request">Comment data</param>
    /// <returns>Created comment</returns>
    [HttpPost("{id:long}/comments")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<ProductCommentDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddProductComment(long id, [FromBody] CreateProductCommentRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ApiError("Invalid request", GetModelErrors());
        }

        request.ProductId = id;
        var comment = await _productService.CreateCommentAsync(request, CurrentUserId);
        return ApiCreated(comment, message: "Comment added successfully");
    }

    /// <summary>
    /// Get top products
    /// </summary>
    /// <param name="count">Number of products to return</param>
    /// <returns>List of top products</returns>
    [HttpGet("top")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<List<ProductListDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTopProducts([FromQuery] int count = 10)
    {
        var products = await _productService.GetTopProductsAsync(count);
        return ApiSuccess(products);
    }

    /// <summary>
    /// Get recent products
    /// </summary>
    /// <param name="count">Number of products to return</param>
    /// <returns>List of recent products</returns>
    [HttpGet("recent")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<List<ProductListDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRecentProducts([FromQuery] int count = 10)
    {
        var products = await _productService.GetRecentProductsAsync(count);
        return ApiSuccess(products);
    }

    /// <summary>
    /// Get best selling products
    /// </summary>
    /// <param name="count">Number of products to return</param>
    /// <returns>List of best selling products</returns>
    [HttpGet("bestsellers")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<List<ProductListDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBestSellers([FromQuery] int count = 10)
    {
        var products = await _productService.GetBestSellersAsync(count);
        return ApiSuccess(products);
    }

    /// <summary>
    /// Get related products
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <param name="count">Number of products to return</param>
    /// <returns>List of related products</returns>
    [HttpGet("{id:long}/related")]
    [ProducesResponseType(typeof(Models.Responses.ApiResponse<List<ProductListDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRelatedProducts(long id, [FromQuery] int count = 5)
    {
        var products = await _productService.GetRelatedProductsAsync(id, count);
        return ApiSuccess(products);
    }
}