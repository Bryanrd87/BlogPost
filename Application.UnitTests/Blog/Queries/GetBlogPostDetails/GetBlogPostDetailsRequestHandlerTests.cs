namespace Application.UnitTests.Blog.Queries.GetBlogPostDetails;
public class GetBlogPostDetailsRequestHandlerTests
{
    private readonly Mock<IBlogPostRepository> _iBlogPostRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetBlogPostDetailsRequestHandler _handler;
    private const string title = "Title"; 
    private const string content = "content";   
    private string createdBy = Guid.NewGuid().ToString();
   
    public GetBlogPostDetailsRequestHandlerTests()
    {
        _iBlogPostRepositoryMock = new Mock<IBlogPostRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new(_iBlogPostRepositoryMock.Object, _mapperMock.Object);
    }
    [Fact]
    public async Task Handle_Should_Return_NotFound()
    {
        //Arrange
        var blogPostId = Guid.NewGuid();
        var query = new GetBlogPostDetailsQuery(blogPostId);

        // Act & Assert
        var exception = await Should.ThrowAsync<NotFoundException>(async () => await _handler.Handle(query, CancellationToken.None));
        exception.ShouldNotBeNull();
        exception.Message.ShouldBe($"blogPost ({blogPostId}) was not found.");
    }

    [Fact]
    public async Task Handle_Should_Return_BlogPost()
    {
        // Arrange
        var blogPostId = Guid.NewGuid();
        BlogPost blogPost = new()
        {
            BlogPostId = blogPostId,
            Content = content,
            CreatedAt = DateTime.Now,
            ApplicationUserId = createdBy,
            Title = title            
        };

        BlogPostDetailsDTO blogPostDetailsDto = new();
        var query = new GetBlogPostDetailsQuery(blogPostId);
        var cancellationToken = CancellationToken.None;

        _iBlogPostRepositoryMock.Setup(r => r.GetBlogPostByIdAsync(blogPostId, cancellationToken, It.IsAny<bool>(), It.IsAny<string>())).ReturnsAsync(blogPost);

        _mapperMock.Setup(mapper => mapper.Map<BlogPostDetailsDTO>(blogPost)).Returns(blogPostDetailsDto);

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        result.ShouldBe(blogPostDetailsDto);
    }
}
