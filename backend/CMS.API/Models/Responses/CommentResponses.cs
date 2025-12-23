namespace CMS.API.Models.Responses;

public class CommentResponse
{
    public long Id { get; set; }
    public long EntityId { get; set; } // Article ID or Product ID
    public long? ParentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool Active { get; set; }
    public DateTime CreateDate { get; set; }
    public string? CreateBy { get; set; }
    public List<CommentResponse> Replies { get; set; } = new();
}

public class ReviewResponse
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public string? UserId { get; set; }
    public string? UserName { get; set; }
    public int Rating { get; set; }
    public string? Title { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool Active { get; set; }
    public DateTime CreateDate { get; set; }
}

public class ReviewStatsResponse
{
    public int TotalReviews { get; set; }
    public double AverageRating { get; set; }
    public int FiveStarCount { get; set; }
    public int FourStarCount { get; set; }
    public int ThreeStarCount { get; set; }
    public int TwoStarCount { get; set; }
    public int OneStarCount { get; set; }
}
