using Domain.User;
using Persistence.Repositories;
using System.Threading;

namespace Application.UnitTests.Blog.Commands.Create;
public class CreateBlogPostCommandHandlerTests
{
    private readonly Mock<IBlogPostRepository> _iBlogPostRepositoryMock;
    private readonly Mock<IUserRepository> _iUserRepositoryMock;
    private readonly CreateBlogPostCommandHandler _handler;
    private readonly Mock<IMapper> _mapper;
    private const string title = "Test Blog Post";
    private const string content = "This is a test blog post.";
    private string createdBy = Guid.NewGuid().ToString();
    private Fixture _fixture;
    public CreateBlogPostCommandHandlerTests()
    {
        _iBlogPostRepositoryMock = new Mock<IBlogPostRepository>();
        _iUserRepositoryMock = new Mock<IUserRepository>();
        _mapper = new Mock<IMapper>();
        _handler = new(_iBlogPostRepositoryMock.Object, _mapper.Object, _iUserRepositoryMock.Object);
        _fixture = new Fixture();
        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task Handle_Should_CreateBlogPost()
    {
        // Arrange
        CreateBlogPostCommand request = new(title, content, createdBy);

        var cancellationToken = CancellationToken.None;

        var user = _fixture.Create<ApplicationUser>();

        _mapper.Setup(mapper => mapper.Map<BlogPost>(request)).Returns(new BlogPost());
        _iUserRepositoryMock.Setup(repo => repo.GetUserByIdAsync(request.ApplicationUserId)).ReturnsAsync(user);       

        // Act
        await _handler.Handle(request, cancellationToken);

        // Assert
        _iBlogPostRepositoryMock.Verify(repo => repo.CreateBlogPostAsync(It.IsAny<BlogPost>(), cancellationToken), Times.Once);
    }
}