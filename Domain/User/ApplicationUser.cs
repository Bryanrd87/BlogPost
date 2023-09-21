using Domain.Blog;
using Microsoft.AspNetCore.Identity;

namespace Domain.User;
public class ApplicationUser : IdentityUser
{  
    public string FullName { get; set; }
    public virtual ICollection<BlogPost> Blogs { get; set; }
}
