namespace BlogSystem.Web.Areas.Messages.Models
{
    using System.ComponentModel.DataAnnotations;

    public class MessageInputModel
    {
        public MessageInputModel()
        {
            this.Content = "Hi";
        }

        [Required(ErrorMessage = "Required!")]
        [StringLength(100, ErrorMessage = "Maximum 100 symbols. Minimum 1 symbol.", MinimumLength = 1)]
        public string Content { get; set; }

        [Required(ErrorMessage = "UserName is required!")]
        public string UserName { get; set; }
    }
}