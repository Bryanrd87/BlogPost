using MediatR;

namespace Application.Blog.Commands.Create;
public record CreateBlogPostCommand(string Title, string Content, string ApplicationUserId) : IRequest;
public record CreateBlogPostRequest(string Title, string Content);