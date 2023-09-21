using Domain.Blog;
using Domain.Role;
using Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;

namespace Persistence;
public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
    this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BlogPostDataContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("BlogPostDefaultConnString"), sqlServerOptions => sqlServerOptions.EnableRetryOnFailure());
        });

        services.AddIdentityCore<ApplicationUser>()
              .AddRoles<IdentityRole>()
              .AddEntityFrameworkStores<BlogPostDataContext>();
        services.AddScoped<IBlogPostRepository, BlogPostRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();

        return services;
    }
}
