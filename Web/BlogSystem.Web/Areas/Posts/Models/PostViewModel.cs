namespace BlogSystem.Web.Areas.Posts.Models
{
    using System;
    using System.Linq.Expressions;

    using AutoMapper;

    using BlogSystem.Models;

    public class PostViewModel : PostListViewModel
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

        public override void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Post, PostViewModel>()
                .ForMember(post => post.Author, options => options.MapFrom(post => post.User.UserName));
        }
    }
}