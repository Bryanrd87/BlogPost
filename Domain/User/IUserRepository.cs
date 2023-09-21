using Microsoft.AspNetCore.Identity;

namespace Domain.User;
public interface IUserRepository
{
    Task<ApplicationUser> GetUserByNameAsync(string userName);
    Task<bool> CheckPasswordAsync(ApplicationUser applicationUser, string password);
    Task<IEnumerable<string>> GetRolesByUserAsync(ApplicationUser applicationUser);
    Task<IdentityResult> CreateAsync(ApplicationUser applicationUser, string password);
    Task<IdentityResult> AddToRoleAsync(ApplicationUser applicationUser, string roleName);
    Task<ApplicationUser> GetUserByIdAsync(string id);
}
