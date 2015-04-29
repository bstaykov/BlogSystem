namespace BlogSystem.Web.Areas.Posts.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CommentUpdateModel
    {
        [Required(ErrorMessage = "Comment Id is Required!")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Required!")]
        [StringLength(100, ErrorMessage = "Maximum 100 symbols.", MinimumLength = 1)]
        public string Content { get; set; }

        [Required(ErrorMessage = "Post Id is required!")]
        public int PostId { get; set; }
    }
}