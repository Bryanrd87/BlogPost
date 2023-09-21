using MediatR;

namespace Application.Blog.Queries.GetBlogPosts;
public class GetBlogPostsQuery : IRequest<List<BlogPostDetailsDTO>>
{
}