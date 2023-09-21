using Application.Exceptions;
using AutoMapper;
using Domain.Blog;
using MediatR;

namespace Application.Blog.Queries.GetBlogPostDetails;
public class GetBlogPostDetailsRequestHandler : IRequestHandler<GetBlogPostDetailsQuery, BlogPostDetailsDTO>
{
    private readonly IBlogPostRepository _iblogpostrepository;
    private readonly IMapper _mapper;
    private const string ApplicationUser = "ApplicationUser";
    public GetBlogPostDetailsRequestHandler(IBlogPostRepository blogPostRepository, IMapper mapper)
    {
        _iblogpostrepository = blogPostRepository;
        _mapper = mapper;
    }
    public async Task<BlogPostDetailsDTO> Handle(GetBlogPostDetailsQuery request, CancellationToken cancellationToken)
    {
        var blogPost = await _iblogpostrepository.GetBlogPostByIdAsync(request.BlogPostId, cancellationToken, true, ApplicationUser);

        return blogPost == null
                ? throw new NotFoundException(nameof(blogPost), request.BlogPostId)
                : _mapper.Map<BlogPostDetailsDTO>(blogPost);       
    }
}