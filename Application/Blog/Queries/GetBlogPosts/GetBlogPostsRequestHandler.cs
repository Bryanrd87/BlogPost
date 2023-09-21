using AutoMapper;
using Domain.Blog;
using MediatR;

namespace Application.Blog.Queries.GetBlogPosts
{
    public class GetBlogPostsRequestHandler : IRequestHandler<GetBlogPostsQuery, List<BlogPostDetailsDTO>>
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IMapper _mapper;
        private const string ApplicationUser = "ApplicationUser";
        public GetBlogPostsRequestHandler(IBlogPostRepository blogPostRepository, IMapper mapper)
        {
            _blogPostRepository = blogPostRepository;
            _mapper = mapper;
        }
        public async Task<List<BlogPostDetailsDTO>> Handle(GetBlogPostsQuery request, CancellationToken cancellationToken)
        {
            var blogPostList = await _blogPostRepository.GetAllBlogPostsAsync(null, cancellationToken, ApplicationUser, 0, 1);
            return _mapper.Map<List<BlogPostDetailsDTO>>(blogPostList);
        }
    }
}
