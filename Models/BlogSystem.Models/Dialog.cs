namespace BlogSystem.Models
{
    using System.Collections.Generic;

    public class Dialog
    {
        private ICollection<Message> messages;

        public Dialog()
        {
            this.Messages = new HashSet<Message>();
        }

        public int Id { get; set; }

        public string FirstUserId { get; set; }

        public virtual User FirstUser { get; set; }

        public string SecondUserId { get; set; }

        public virtual User SecondUser { get; set; }

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
    }
}
