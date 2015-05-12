namespace BlogSystem.Web.Areas.Posts.Models
{
    using System.ComponentModel.DataAnnotations;

    public class PostEditModel : PostInputModel
    {
        [Required(ErrorMessage = "Post Id is Required!")]
        public int Id { get; set; }
    }
}