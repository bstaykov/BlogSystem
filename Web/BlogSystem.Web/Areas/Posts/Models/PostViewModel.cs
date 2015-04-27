namespace BlogSystem.Web.Areas.Posts.Models
{
    using System;
    using System.Linq.Expressions;

    using AutoMapper;

    using BlogSystem.Models;
    using BlogSystem.Web.Infrastructure.Mapping;

    using Microsoft.AspNet.Identity;

    public class PostViewModel : IMapFrom<Post>, IHaveCustomMappings
    {
        public static Expression<Func<Post, PostViewModel>> FromPost
        {
            get
            {
                return post => new PostViewModel
                {
                    Id = post.Id,
                    Title = post.Title,
                    Content = post.Content,
                    Category = post.Category,
                    CreatedOn = post.CreatedOn,
                    Author = post.User.UserName,
                    CommentsCount = post.CommentsCount,
                    Likes = post.Likes
                };
            }
        }

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
            configuration.CreateMap<Post, PostViewModel>()
                .ForMember(post => post.Author, options => options.MapFrom(post => post.User.UserName));
        }
    }
}