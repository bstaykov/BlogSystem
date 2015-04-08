namespace BlogSystem.Web.Areas.Posts.Models
{
    using BlogSystem.Models;
    using System;
    using System.Linq.Expressions;
    using Microsoft.AspNet.Identity;

    public class MyPostViewModel
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
                    DateTimePosted = post.DateTimePosted,
                };
            }
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public PostCategory Category { get; set; }

        public DateTime DateTimePosted { get; set; }
    }
}