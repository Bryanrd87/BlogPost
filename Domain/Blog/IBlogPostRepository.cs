using System.Linq.Expressions;

namespace Domain.Blog;
public interface IBlogPostRepository
{
    Task<BlogPost> GetBlogPostByIdAsync(Guid BlogPostId, CancellationToken cancellationToken, bool tracked = true, string includeProperties = null);
    Task<IEnumerable<BlogPost>> GetAllBlogPostsAsync(Expression<Func<BlogPost, bool>> filter, CancellationToken cancellationToken, string includeProperties = null, int pageSize = 0, int pageNumber = 1);
    Task CreateBlogPostAsync(BlogPost blogPost, CancellationToken cancellationToken);
    Task UpdateBlogPostAsync(BlogPost blogPost, CancellationToken cancellationToken);
    Task DeleteBlogPostAsync(BlogPost blogPost, CancellationToken cancellationToken);
}
