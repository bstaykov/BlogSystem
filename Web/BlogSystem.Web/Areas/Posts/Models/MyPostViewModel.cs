namespace BlogSystem.Web.Areas.Posts.Models
{
    using System;
    using System.Linq.Expressions;

    using AutoMapper;

    using BlogSystem.Common.Models;
    using BlogSystem.Models;
    using BlogSystem.Web.Infrastructure.Mapping;

    using Microsoft.AspNet.Identity;

    public class MyPostViewModel : IMapFrom<Post>, IHaveCustomMappings
    {
        public static Expression<Func<Post, MyPostViewModel>> FromPost
        {
            get
            {
                return post => new MyPostViewModel
                {
                    Id = post.Id,
                    Title = post.Title,
                    Content = post.Content,
                    Category = post.Category,
                    CreatedOn = post.CreatedOn,
                };
            }
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsDeleted { get; set; }

        public PostCategory Category { get; set; }

        public string Info { get; set; }

        public int Likes { get; set; }

        public int CommentsCount { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Post, MyPostViewModel>()
                .ForMember(post => post.Info, options => options.MapFrom(post => "Likes: " + post.Likes + " Comments: " + post.CommentsCount));
        }
    }
}