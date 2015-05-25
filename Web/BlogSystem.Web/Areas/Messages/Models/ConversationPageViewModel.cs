namespace BlogSystem.Web.Areas.Messages.Models
{
    using System.Collections.Generic;

    public class ConversationPageViewModel
    {
        public int Page { get; set; }

        public string ParticipantPictureUrl { get; set; }

        public string ParticipantName { get; set; }

        public IEnumerable<ConversationViewModel> Messages { get; set; }
    }
}