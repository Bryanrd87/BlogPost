using Domain.Blog;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Persistence.Repositories;
public class BlogPostRepository : IBlogPostRepository
{
    private readonly BlogPostDataContext _context;
    internal DbSet<BlogPost> _dbSet;
    public BlogPostRepository(BlogPostDataContext context)
    {
        _context = context;
        _dbSet = _context.Set<BlogPost>();
    }
    public async Task CreateBlogPostAsync(BlogPost blogPost, CancellationToken cancellationToken)
    {
        blogPost.CreatedAt = DateTime.Now;
        await _context.BlogPosts.AddAsync(blogPost, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteBlogPostAsync(BlogPost blogPost, CancellationToken cancellationToken)
    {
        _context.BlogPosts.Remove(blogPost);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<BlogPost>> GetAllBlogPostsAsync(Expression<Func<BlogPost, bool>> filter, CancellationToken cancellationToken, string includeProperties = null, int pageSize = 0, int pageNumber = 1)
    {
        IQueryable<BlogPost> query = _dbSet;

        if (filter is not null)
        {
            query = query.Where(filter);
        }
        if (pageSize > 0)
        {
            if (pageSize > 100)
            {
                pageSize = 100;
            }           
            query = query.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
        }
        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp);
            }
        }
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<BlogPost> GetBlogPostByIdAsync(Guid blogPostId, CancellationToken cancellationToken, bool tracked = true, string includeProperties = null)
    {
        IQueryable<BlogPost> query = _dbSet;
        if (!tracked)
        {
            query = query.AsNoTracking();
        }  
        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp);
            }
        }
        return await query.FirstOrDefaultAsync(x=>x.BlogPostId == blogPostId);
    }

    public async Task UpdateBlogPostAsync(BlogPost blogPost, CancellationToken cancellationToken)
    {
        var existingBlogPost = await _context.BlogPosts.Include(x=>x.ApplicationUser).FirstOrDefaultAsync(x=>x.BlogPostId == blogPost.BlogPostId, cancellationToken);

        existingBlogPost.Title = blogPost.Title;
        existingBlogPost.Content = blogPost.Content;
        existingBlogPost.UpdatedAt = blogPost.UpdatedAt;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
