namespace CMS.API.Models.Responses;

/// <summary>
/// Standard API response wrapper
/// </summary>
/// <typeparam name="T">Type of data being returned</typeparam>
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }
    public List<string> Errors { get; set; } = new();
    public PaginationInfo? Pagination { get; set; }

    public static ApiResponse<T> SuccessResponse(T data, string? message = null)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Data = data,
            Message = message
        };
    }

    public static ApiResponse<T> SuccessResponse(T data, PaginationInfo pagination, string? message = null)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Data = data,
            Message = message,
            Pagination = pagination
        };
    }

    public static ApiResponse<T> ErrorResponse(string message, List<string>? errors = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Errors = errors ?? new List<string>()
        };
    }
}

/// <summary>
/// Pagination information for list responses
/// </summary>
public class PaginationInfo
{
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalItems { get => TotalCount; set => TotalCount = value; }
    public int TotalPages { get => PageSize > 0 ? (int)Math.Ceiling((double)TotalCount / PageSize) : 0; set { } }
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;
}

/// <summary>
/// Paginated list result
/// </summary>
/// <typeparam name="T">Type of items in the list</typeparam>
public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public PaginationInfo Pagination { get; set; } = new();

    public PagedResult() { }

    public PagedResult(List<T> items, int totalCount, int currentPage, int pageSize)
    {
        Items = items;
        Pagination = new PaginationInfo
        {
            CurrentPage = currentPage,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}