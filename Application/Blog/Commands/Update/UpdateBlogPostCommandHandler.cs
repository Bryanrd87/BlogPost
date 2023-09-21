using Application.Exceptions;
using Domain.Blog;
using MediatR;

namespace Application.Blog.Commands.Update;
public class UpdateBlogPostCommandHandler : IRequestHandler<UpdateBlogPostCommand>
{
    private readonly IBlogPostRepository _blogPostRepository;
    private const string ApplicationUser = "ApplicationUser";
    public UpdateBlogPostCommandHandler(IBlogPostRepository blogPostRepository)
    {
        _blogPostRepository = blogPostRepository;
    }
    public async Task Handle(UpdateBlogPostCommand request, CancellationToken cancellationToken)
    {
        var blogPost = await _blogPostRepository.GetBlogPostByIdAsync(request.BlogPostId, cancellationToken, true, ApplicationUser);

        if (blogPost is null)
        {
            throw new NotFoundException(nameof(blogPost), request.BlogPostId);
        }
       
        blogPost.Content = request.Content;
        blogPost.Title = request.Title;
        blogPost.UpdatedAt = DateTime.Now;

        await _blogPostRepository.UpdateBlogPostAsync(blogPost, cancellationToken);
    }
}