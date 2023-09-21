using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Persistence.IntegrationTests;
public class BlogPostRepositoryIntegrationTests : IDisposable
{
    private readonly DbContextOptions<BlogPostDataContext> _options;
    private readonly BlogPostDataContext _context;
    private readonly BlogPostRepository _iBlogPostRepository;
    private readonly Fixture _fixture;
    private const string ConnString = "Server=(localdb)\\MSSQLLocalDB;Database=BlogPostData;Trusted_Connection=True;";

    public BlogPostRepositoryIntegrationTests()
    {

        var dbName = Guid.NewGuid().ToString();
        _options = new DbContextOptionsBuilder<BlogPostDataContext>()
            .UseSqlServer($"Server=(localdb)\\MSSQLLocalDB;Database={dbName};Trusted_Connection=True;MultipleActiveResultSets=true")
            .Options;
        _context = new BlogPostDataContext(_options);
        _context.Database.EnsureCreated();

        _context = new BlogPostDataContext(_options);

        _iBlogPostRepository = new BlogPostRepository(_context);

        _fixture = new();
        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task CreateBlogPost_WithValidData_ShouldAddBlogPostToDatabase()
    {
        // Arrange
        var blogPost = _fixture.Create<BlogPost>();

        // Act
        await _iBlogPostRepository.CreateBlogPostAsync(blogPost, CancellationToken.None);

        // Assert
        using var context = new BlogPostDataContext(_options);       
        var createdBlogPost = await context.BlogPosts.FirstOrDefaultAsync();
        createdBlogPost.ShouldNotBeNull();
        createdBlogPost.BlogPostId.ShouldBeEquivalentTo(blogPost.BlogPostId);
        createdBlogPost.Title.ShouldBeEquivalentTo(blogPost.Title);
        createdBlogPost.Content.ShouldBeEquivalentTo(blogPost.Content);
        createdBlogPost.ApplicationUserId.ShouldBeEquivalentTo(blogPost.ApplicationUserId);
        createdBlogPost.CreatedAt.ShouldBeEquivalentTo(blogPost.CreatedAt);
    }


    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}