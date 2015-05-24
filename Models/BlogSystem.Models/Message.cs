namespace BlogSystem.Models
{
    using System;

    public class Message
    {
        public int Id { get; set; }

        public int DialogId { get; set; }

        public virtual Dialog Dialog { get; set; }

        public DateTime SendOn { get; set; }

        public string Content { get; set; }

        public MessageSender Sender { get; set; }

        public MessageReadBy ReadBy { get; set; }
    }
}
