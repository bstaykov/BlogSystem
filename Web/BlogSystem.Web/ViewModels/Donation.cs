namespace BlogSystem.Web.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class Donation
    {
        public Donation()
        {
            this.DonationValue = 100;
        }

        [Range(1, 1000, ErrorMessage = "Donation(1, 1000)")]
        public int DonationValue { get; set; }

        public override string ToString()
        {
            return this.DonationValue.ToString();
        }
    }
}