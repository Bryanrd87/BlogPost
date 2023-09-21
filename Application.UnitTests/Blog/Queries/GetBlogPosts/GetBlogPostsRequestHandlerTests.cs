namespace Application.UnitTests.Blog.Queries.GetBlogPosts;
public class GetBlogPostsRequestHandlerTests
{
    private readonly Mock<IBlogPostRepository> _iBlogPostRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetBlogPostsRequestHandler _handler;
    private readonly Fixture _fixture;
    private const string ApplicationUser = "ApplicationUser";

    public GetBlogPostsRequestHandlerTests()
    {
        _iBlogPostRepositoryMock = new Mock<IBlogPostRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new(_iBlogPostRepositoryMock.Object, _mapperMock.Object);
        _fixture = new();
        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }
    [Fact]
    public async Task Handle_Should_ReturnBlogPostsList()
    {
        // Arrange      
        var blogPostList = _fixture.CreateMany<BlogPost>().ToList();
        var blogPostDetailsDtoList = _fixture.CreateMany<BlogPostDetailsDTO>().ToList();
        GetBlogPostsQuery query = new();        
        var cancellationToken = CancellationToken.None;
        
        _mapperMock.Setup(mapper => mapper.Map<List<BlogPostDetailsDTO>>(blogPostList)).Returns(blogPostDetailsDtoList);

        _iBlogPostRepositoryMock.Setup(repo => repo.GetAllBlogPostsAsync(null, cancellationToken, ApplicationUser, It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(blogPostList);        

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(blogPostDetailsDtoList.Count);
    }
}
