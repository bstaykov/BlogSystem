namespace BlogSystem.Web.Areas.Messages.Models
{
    using BlogSystem.Models;

    public class MessageAlertsModel
    {
        public Message Message { get; set; }

        public int UnreadMessagesCount { get; set; }
    }
}