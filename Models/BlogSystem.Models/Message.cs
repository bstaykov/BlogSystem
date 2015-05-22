namespace BlogSystem.Models
{
    using System;

    public class Message
    {
        public int Id { get; set; }

        public int DialogId { get; set; }

        public virtual Dialog Dialog { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }

        public DateTime SendOn { get; set; }

        public string Content { get; set; }
    }
}
