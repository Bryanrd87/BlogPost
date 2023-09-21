using MediatR;

namespace Application.Blog.Commands.Delete;
public record DeleteBlogPostCommand(Guid BlogPostId) : IRequest;

