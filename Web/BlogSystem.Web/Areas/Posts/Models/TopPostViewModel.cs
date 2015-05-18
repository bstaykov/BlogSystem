namespace BlogSystem.Web.Areas.Posts.Models
{
    using AutoMapper;

    using BlogSystem.Models;
    using BlogSystem.Web.Infrastructure.Mapping;

    public class TopPostViewModel : TopCommentedPostViewModel
    {
        public int Likes { get; set; }

        public int TimesRead { get; set; }

        public override void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Post, TopPostViewModel>()
                .ForMember(post => post.Author, options => options.MapFrom(post => post.User.UserName));
        }
    }
}