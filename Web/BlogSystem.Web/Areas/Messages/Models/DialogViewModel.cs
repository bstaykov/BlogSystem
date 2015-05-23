namespace BlogSystem.Web.Areas.Messages.Models
{
    using System;
    using System.Collections.Generic;

    using AutoMapper;

    using BlogSystem.Models;
    using BlogSystem.Web.Infrastructure.Mapping;

    public class DialogViewModel
    {
        public int Page { get; set; }

        public int DialogId { get; set; }

        public IEnumerable<DialogMessagesViewModel> Messages { get; set; }

        public IEnumerable<MessageParticipantInfo> ParticipantsInfo { get; set; }
    }
}