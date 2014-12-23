namespace BlogSystem.Web.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class EnumModel
    {
        public EnumModel()
        {
            this.Color = ColorType.Blue;
        }

        [Required(ErrorMessage = "*Required")]
        [EnumDataType(typeof(ColorType), ErrorMessage = "Invalid Color")]
        public ColorType Color { get; set; }
    }
}