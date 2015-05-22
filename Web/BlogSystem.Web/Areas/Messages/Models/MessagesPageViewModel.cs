namespace BlogSystem.Web.Areas.Messages.Models
{
    using System.Collections.Generic;

    public class MessagesPageViewModel
    {
        public int Page { get; set; }

        public IEnumerable<MessageViewModel> Messages { get; set; }
    }
}