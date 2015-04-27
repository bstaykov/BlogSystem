namespace BlogSystem.Web.Areas.Posts.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CommentInputModel
    {
        public CommentInputModel()
        {
            this.Content = "Write your comment here...";
        }

        [Required(ErrorMessage = "Category is required!")]
        [StringLength(100, ErrorMessage = "Maximum 100 symbols.")]
        public string Content { get; set; }
    }
}