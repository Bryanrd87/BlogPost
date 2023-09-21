using Domain.User;

namespace Domain.Blog;
public class BlogPost
{
    public Guid BlogPostId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }    
    public DateTime? UpdatedAt { get; set; }    
    public string ApplicationUserId { get; set; }
    public virtual ApplicationUser ApplicationUser { get; set;}
}