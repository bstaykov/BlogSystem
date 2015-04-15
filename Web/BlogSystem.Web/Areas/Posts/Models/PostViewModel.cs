namespace BlogSystem.Web.Areas.Posts.Models
{
    using System;
    using System.Linq.Expressions;

    using BlogSystem.Models;
    using BlogSystem.Web.Infrastructure.Mapping;

    using Microsoft.AspNet.Identity;

    public class PostViewModel : IMapFrom<Post>
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
                    DateTimePosted = post.DateTimePosted,
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

        public DateTime DateTimePosted { get; set; }

        public string Author { get; set; }

        public int Likes { get; set; }

        public int CommentsCount { get; set; }
    }
}