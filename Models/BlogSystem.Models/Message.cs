namespace BlogSystem.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Message
    {
        public int Id { get; set; }

        public int? DialogId { get; set; }

        public virtual Message Dialog { get; set; }

        public string SenderId { get; set; }

        public virtual User Sender { get; set; }

        public string ReceiverId { get; set; }

        public virtual User Receiver { get; set; }

        [Required]
        public string Content { get; set; }

        public bool IsRead { get; set; }

        [Required]
        public DateTime SendOn { get; set; }

        public DateTime? ReadOn { get; set; }

        //public int Id { get; set; }

        //public int DialogId { get; set; }

        //public virtual Dialog Dialog { get; set; }

        //public DateTime SendOn { get; set; }

        //public string Content { get; set; }

        //public MessageSender Sender { get; set; }

        //public MessageReadBy ReadBy { get; set; }
    }
}
