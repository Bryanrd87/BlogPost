using Domain.Role;
using Microsoft.AspNetCore.Identity;

namespace Persistence.Repositories;
public class RoleRepository : IRoleRepository
{    
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly BlogPostDataContext _blogPostDataContext;
    public RoleRepository(RoleManager<IdentityRole> roleManager, BlogPostDataContext blogPostDataContext)
    {
        _roleManager = roleManager;
        _blogPostDataContext = blogPostDataContext; 
    }

    public async Task<IdentityResult> CreateAsync(string roleName)
    {
        var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
        await _blogPostDataContext.SaveChangesAsync();
        return result;
    }

    public async Task<bool> RoleExistsAsync(string roleName)
    {
        return await _roleManager.RoleExistsAsync(roleName);
    }
}
