namespace Application.Blog;
public record BlogPostDetailsDTO
{
    public Guid BlogPostId { get; init; }
    public string Title { get; init; } 
    public string Content { get; init; }
    public DateTime CreatedAt { get; init; }
    public string CreatedBy { get; init; }
    public DateTime? UpdatedAt { get; init; }  
}