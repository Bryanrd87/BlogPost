using AutoMapper;
using Domain.Blog;
using Domain.User;
using MediatR;

namespace Application.Blog.Commands.Create;
public class CreateBlogPostCommandHandler : IRequestHandler<CreateBlogPostCommand>
{
    private readonly IBlogPostRepository _blogPostRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    public CreateBlogPostCommandHandler(IBlogPostRepository blogPostRepository, IMapper mapper, IUserRepository userRepository)
    {
        _blogPostRepository = blogPostRepository;
        _mapper = mapper;
        _userRepository = userRepository;
    }
    public async Task Handle(CreateBlogPostCommand request, CancellationToken cancellationToken)
    {
        var blogPost = _mapper.Map<BlogPost>(request);

        var user = await _userRepository.GetUserByIdAsync(request.ApplicationUserId);

        blogPost.ApplicationUser = user;

        await _blogPostRepository.CreateBlogPostAsync(blogPost, cancellationToken);
    }
}

