namespace BlogSystem.Web.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;
    using System.Text;

    public class TestEditorsModel : IValidatableObject
    {
        public TestEditorsModel()
        {
            this.Name = "Baj Ivan";
            this.Donations = new List<Donation>() { new Donation() };
            this.IsDonationAgreed = true;
            this.Price = 1.23d;
        }

        [Required(ErrorMessage = "*Required")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Length(5-20)")]
        public string Name { get; set; }

        // [Required(ErrorMessage = "*Required")]
        public List<Donation> Donations { get; set; }

        [Required(ErrorMessage = "*Required")]
        [IsTrue]
        public bool IsDonationAgreed { get; set; }

        [Required(ErrorMessage = "*Required")]
        [Range(0, 2000, ErrorMessage = "Price(0, 2000)")]
        public double Price { get; set; }

        public List<Person> People { get; set; }
        
        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var donation in this.Donations)
            {
                sb.Append(donation.ToString());
            }

            return string.Format("Name: {0} Donation: {1} IsDonationAgreed: {2} Price: {3}", this.Name, sb.ToString(), this.IsDonationAgreed, this.Price);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.IsDonationAgreed == false)
            {
                yield return new ValidationResult("Agree to sell appartment ?!", new[] { "IsDonationAgreed" });
            }
        }
    }

    public class IsTrueAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (TestEditorsModel)validationContext.ObjectInstance;

            if (model.IsDonationAgreed != true)
            {
                return new ValidationResult("Check here to agree 1000$ donation!");
            }

            return ValidationResult.Success;
        }
    }
}