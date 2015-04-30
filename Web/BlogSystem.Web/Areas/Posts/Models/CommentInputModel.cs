namespace BlogSystem.Web.Areas.Posts.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CommentInputModel
    {
        public CommentInputModel()
        {
            this.Content = "Write your comment here...";
        }

        [Required(ErrorMessage = "Required!")]
        [StringLength(100, ErrorMessage = "Maximum 100 symbols.", MinimumLength = 1)]
        public string Content { get; set; }

        [Required(ErrorMessage = "PostId is required!")]
        public int PostId { get; set; }

        public int? ParentCommentId { get; set; }
    }
}