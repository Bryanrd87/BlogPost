namespace Application.UnitTests.Blog.Commands.Delete;
public class DeleteBlogPostCommandHandlerTests
{
    private readonly Mock<IBlogPostRepository> _iBlogPostRepositoryMock;
    private readonly DeleteBlogPostCommandHandler _handler; 
    private const string title = "Title";
    private const string content = "content";
    private string createdBy = Guid.NewGuid().ToString();
    public DeleteBlogPostCommandHandlerTests()
    {
        _iBlogPostRepositoryMock = new Mock<IBlogPostRepository>();        
        _handler = new(_iBlogPostRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingBlogPost_ShouldDeleteSuccessfully()
    {
        // Arrange
        var blogPostId = Guid.NewGuid();
        DeleteBlogPostCommand request = new (blogPostId);
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

        // Assert
        _iBlogPostRepositoryMock.Verify(repo =>
            repo.DeleteBlogPostAsync(blogPost, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistentBlogPost_ShouldThrowNotFoundException()
    {
        //Arrange
        var blogPostId = Guid.NewGuid();
        DeleteBlogPostCommand command = new(blogPostId);      

        // Act & Assert
        var exception = await Should.ThrowAsync<NotFoundException>(async () => await _handler.Handle(command, CancellationToken.None));
        exception.ShouldNotBeNull();
        exception.Message.ShouldBe($"blogPost ({blogPostId}) was not found.");
    }
}