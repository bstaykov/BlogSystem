namespace BlogSystem.Models
{
    public class Log
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }

        public string Url { get; set; }

        public LogStatus Status { get; set; }
    }
}
