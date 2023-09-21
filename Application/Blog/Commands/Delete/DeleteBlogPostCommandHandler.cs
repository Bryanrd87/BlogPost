using Application.Exceptions;
using Domain.Blog;
using MediatR;

namespace Application.Blog.Commands.Delete;
public class DeleteBlogPostCommandHandler : IRequestHandler<DeleteBlogPostCommand>
{
    private readonly IBlogPostRepository _blogPostRepository;
    public DeleteBlogPostCommandHandler(IBlogPostRepository blogPostRepository)
    {
        _blogPostRepository = blogPostRepository;
    }
    public async Task Handle(DeleteBlogPostCommand request, CancellationToken cancellationToken)
    {
        var blogPost = await _blogPostRepository.GetBlogPostByIdAsync(request.BlogPostId, cancellationToken);

        if (blogPost is null)
        {
            throw new NotFoundException(nameof(blogPost), request.BlogPostId);
        }
        await _blogPostRepository.DeleteBlogPostAsync(blogPost, cancellationToken);
    }
}

