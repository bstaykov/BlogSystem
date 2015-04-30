namespace BlogSystem.Web.Areas.Posts.Models
{
    public class CommentRouteValues
    {
        public int PostId { get; set; }

        public int? ParentCommentId { get; set; }
    }
}