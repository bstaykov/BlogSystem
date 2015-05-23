namespace BlogSystem.Models
{
    using System;
    using System.Collections.Generic;

    public class Dialog
    {
        private ICollection<DialogParticipant> dialogParticipants;
        private ICollection<Message> messages;
        private ICollection<ReadDialog> readDiologs;

        public Dialog()
        {
            this.DialogParticipants = new HashSet<DialogParticipant>();
            this.Messages = new HashSet<Message>();
            this.ReadDialogs = new HashSet<ReadDialog>();
        }

        public int Id { get; set; }

        public DateTime StartedOn { get; set; }

        public string StarterId { get; set; }

        public virtual User Starter { get; set; }

        public virtual ICollection<DialogParticipant> DialogParticipants
        {
            get
            {
                return this.dialogParticipants;
            }

            set
            {
                this.dialogParticipants = value;
            }
        }

        public virtual ICollection<Message> Messages
        {
            get
            {
                return this.messages;
            }

            set
            {
                this.messages = value;
            }
        }

        public virtual ICollection<ReadDialog> ReadDialogs
        {
            get
            {
                return this.readDiologs;
            }

            set
            {
                this.readDiologs = value;
            }
        }

        // public string Content { get; set; }

        // public bool IsRead { get; set; }

        // public DateTime SendOn { get; set; }

        // public string SenderId { get; set; }

        // public virtual User Sender { get; set; }

        // public string ReceiverId { get; set; }

        // public virtual User Receiver { get; set; }
    }
}
