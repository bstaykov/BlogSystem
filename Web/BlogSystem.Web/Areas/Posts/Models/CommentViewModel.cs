namespace BlogSystem.Web.Areas.Posts.Models
{
    using System;

    using BlogSystem.Models;
    using BlogSystem.Web.Infrastructure.Mapping;

    public class CommentViewModel : IMapFrom<Comment>
    {
        public int Id { get; set; }

        public int UserName { get; set; }

        public string Content { get; set; }

        public int Likes { get; set; }

        public int SubCommentsCount { get; set; }

        public int PostId { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsDeleted { get; set; }
    }
}