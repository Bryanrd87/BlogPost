using Domain.Blog;
using Domain.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence;
public class BlogPostDataContext : IdentityDbContext<ApplicationUser>
{
    public BlogPostDataContext() { }
    public BlogPostDataContext(DbContextOptions<BlogPostDataContext> options)
       : base(options)
    {
    }
    public DbSet<BlogPost> BlogPosts { get; set; }   
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>()
            .HasMany(u => u.Blogs)         
            .WithOne(b => b.ApplicationUser)          
            .HasForeignKey(b => b.ApplicationUserId);

        modelBuilder.Entity<BlogPost>(build =>
        {
            build.HasKey(entry => entry.BlogPostId);          
            build.Property(entry => entry.BlogPostId).HasConversion(new GuidToStringConverter()).ValueGeneratedOnAdd();
        });       
    }
}
