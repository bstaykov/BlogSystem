namespace BlogSystem.Web.Areas.Posts.Models
{
    using System;

    using AutoMapper;

    using BlogSystem.Models;

    public class PostListViewModel : TopPostViewModel
    {
        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public override void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Post, PostListViewModel>()
                .ForMember(post => post.Content, options => options.MapFrom(post => post.Content.Substring(0, 20) + "..."));
            configuration.CreateMap<Post, PostListViewModel>()
                .ForMember(post => post.Author, options => options.MapFrom(post => post.User.UserName));
        }
    }
}