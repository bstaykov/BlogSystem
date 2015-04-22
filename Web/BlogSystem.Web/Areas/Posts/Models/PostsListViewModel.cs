namespace BlogSystem.Web.Areas.Posts.Models
{
    using System;

    using BlogSystem.Models;

    public class PostsListViewModel
    {
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