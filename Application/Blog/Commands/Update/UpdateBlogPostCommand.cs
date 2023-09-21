using MediatR;

namespace Application.Blog.Commands.Update;
public record UpdateBlogPostCommand(Guid BlogPostId, string Title, string Content) : IRequest;
public record UpdateBlogPostRequest(Guid BlogPostId, string Title, string Content);