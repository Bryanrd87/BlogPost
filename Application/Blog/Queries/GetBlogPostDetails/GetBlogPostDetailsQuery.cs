using MediatR;

namespace Application.Blog.Queries.GetBlogPostDetails;
public record GetBlogPostDetailsQuery(Guid BlogPostId) : IRequest<BlogPostDetailsDTO?>;