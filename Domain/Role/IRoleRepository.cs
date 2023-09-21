using Microsoft.AspNetCore.Identity;

namespace Domain.Role;
public interface IRoleRepository
{
    Task<bool> RoleExistsAsync(string roleName);
    Task<IdentityResult> CreateAsync(string roleName);
}