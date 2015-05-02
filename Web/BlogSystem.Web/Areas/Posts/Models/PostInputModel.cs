namespace BlogSystem.Web.Areas.Posts.Models
{
    using System.ComponentModel.DataAnnotations;

    using BlogSystem.Models;

    public class PostInputModel
    {
        public PostInputModel()
        {
            this.Title = "Default Title";
            this.Content = "Default Content";
        }

        [Required(ErrorMessage = "Title is required!")]
        [StringLength(50, ErrorMessage = "Length is incorect.", MinimumLength = 5)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is required!")]
        [StringLength(1000, ErrorMessage = "Length is incorect.", MinimumLength = 5)]
        public string Content { get; set; }

        [Required(ErrorMessage = "Category is required!")]
        [EnumDataType(typeof(PostCategory), ErrorMessage = "Invalid category!")]
        public PostCategory Category { get; set; }
    }
}