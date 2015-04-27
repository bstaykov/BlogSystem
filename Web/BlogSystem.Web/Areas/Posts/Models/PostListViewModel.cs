namespace BlogSystem.Web.Areas.Posts.Models
{
    using System;

    using AutoMapper;

    using BlogSystem.Models;
    using BlogSystem.Web.Infrastructure.Mapping;

    public class PostListViewModel : IMapFrom<Post>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public PostCategory Category { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Author { get; set; }

        public int Likes { get; set; }

        public int CommentsCount { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Post, PostListViewModel>()
                .ForMember(post => post.Content, options => options.MapFrom(post => post.Content.Substring(0, 100)));
            configuration.CreateMap<Post, PostListViewModel>()
                .ForMember(post => post.Author, options => options.MapFrom(post => post.User.UserName));
        }
    }
}