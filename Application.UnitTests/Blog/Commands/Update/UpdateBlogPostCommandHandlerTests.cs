namespace Application.UnitTests.Blog.Commands.Update;
public class UpdateBlogPostCommandHandlerTests
{
    private readonly Mock<IBlogPostRepository> _iBlogPostRepositoryMock;
    private readonly UpdateBlogPostCommandHandler _handler;   
    private const string title = "Title";
    private const string titleTwo = "Title2";
    private const string content = "content";
    private const string contentTwo = "content2";
    private string createdBy = Guid.NewGuid().ToString();    
    public UpdateBlogPostCommandHandlerTests()
    {
        _iBlogPostRepositoryMock = new Mock<IBlogPostRepository>();       
        _handler = new(_iBlogPostRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingBlogPost_ShouldUpdateSuccessfully()
    {
        // Arrange
        var blogPostId = Guid.NewGuid();
        UpdateBlogPostCommand request = new(blogPostId, titleTwo, contentTwo);
        var cancellationToken = CancellationToken.None;

        BlogPost blogPost = new()
        {
            BlogPostId = blogPostId,
            Content = content,
            ApplicationUserId = createdBy,
            CreatedAt = DateTime.Now,
            Title = title
        };

        _iBlogPostRepositoryMock.Setup(repo => repo.GetBlogPostByIdAsync(blogPostId, cancellationToken, It.IsAny<bool>(), It.IsAny<string>())).ReturnsAsync(blogPost);

        // Act
        await _handler.Handle(request, cancellationToken);

        //Assert            
        _iBlogPostRepositoryMock.Verify(repo =>
                repo.UpdateBlogPostAsync(It.IsAny<BlogPost>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistentBlogPost_ShouldThrowNotFoundException()
    {
        //Arrange
        var blogPostId = Guid.NewGuid();
        UpdateBlogPostCommand command = new(blogPostId, title, content);

        // Act & Assert
        var exception = await Should.ThrowAsync<NotFoundException>(async () => await _handler.Handle(command, CancellationToken.None));
        exception.ShouldNotBeNull();
        exception.Message.ShouldBe($"blogPost ({blogPostId}) was not found.");
    }
}