using Domain.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;
public class UserRepository : IUserRepository
{
    private readonly BlogPostDataContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    public UserRepository(BlogPostDataContext dbContext, UserManager<ApplicationUser> userManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
    }

    public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<ApplicationUser> GetUserByIdAsync(string id)
    {
        var user = await _dbContext.ApplicationUsers.FindAsync(id);
        return user;
    }

    public async Task<ApplicationUser> GetUserByNameAsync(string userName)
    {
        var user = await _dbContext.ApplicationUsers
           .FirstOrDefaultAsync(u => u.UserName.ToLower() == userName.ToLower());
        return user;
    }

    public async Task<IEnumerable<string>> GetRolesByUserAsync(ApplicationUser applicationUser)
    {
        return await _userManager.GetRolesAsync(applicationUser);
    }

    public async Task<IdentityResult> CreateAsync(ApplicationUser applicationUser, string password)
    {
        var user = await _userManager.CreateAsync(applicationUser, password);
        await _dbContext.SaveChangesAsync();

        return user;
    }

    public async Task<IdentityResult> AddToRoleAsync(ApplicationUser applicationUser, string roleName)
    {
        var result = await _userManager.AddToRoleAsync(applicationUser, roleName);
        await _dbContext.SaveChangesAsync();
        return result;
    }
}