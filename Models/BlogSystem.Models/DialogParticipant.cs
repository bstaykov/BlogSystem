namespace BlogSystem.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class DialogParticipant
    {
        [Key, Column(Order = 0)]
        public int DialogId { get; set; }

        public virtual Dialog Dialog { get; set; }

        [Key, Column(Order = 1)]
        public string UserId { get; set; }

        public virtual User User { get; set; }

        public DateTime DateAdded { get; set; }
    }
}
