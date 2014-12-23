namespace BlogSystem.Web.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class FormInputModel
    {
        public FormInputModel()
        {
            this.Name = "MyName";
            this.Age = 18;
        }

        [Required(ErrorMessage = "Name is required!")]
        [StringLength(20, ErrorMessage = "Length is incorect.", MinimumLength = 5)]
        [RegularExpression(@"^[0-9a-zA-Z\. s]{5,20}$", ErrorMessage = "Invalid Name!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Age is required!")]
        [RegularExpression(@"^([0-9]|[1-9][0-9]|[1][0-2][0-9])$", ErrorMessage = "Invalid Age!")]
        public int Age { get; set; }

        public override string ToString()
        {
            return string.Format("Name: {0} Age: {1}", this.Name, this.Age);
        }
    }
}