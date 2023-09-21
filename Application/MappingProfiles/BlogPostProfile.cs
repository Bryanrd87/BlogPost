using Application.Blog;
using Application.Blog.Commands.Create;
using Application.Login;
using AutoMapper;
using Domain.Blog;
using Domain.User;

namespace Application.MappingProfiles
{
    public class BlogPostProfile : Profile
    {
        public BlogPostProfile()
        {
            CreateMap<BlogPostDetailsDTO, BlogPost>().ReverseMap().ForMember(x => x.CreatedBy, opt => opt.MapFrom(src => src.ApplicationUser.FullName)); ;
            CreateMap<CreateBlogPostCommand, BlogPost>();
            CreateMap<UserDTO, ApplicationUser>().ReverseMap();
        }
    }
}
