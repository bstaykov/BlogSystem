namespace BlogSystem.Web.Areas.Messages.Models
{
    using System;
    using System.Collections.Generic;

    using AutoMapper;

    using BlogSystem.Models;
    using BlogSystem.Web.Infrastructure.Mapping;

    public class ConversationViewModel : IMapFrom<Message>, IHaveCustomMappings
    {
        public string Content { get; set; }

        public DateTime SendOn { get; set; }

        public string Sender { get; set; }

        public string SenderPictureUrl { get; set; }

        public string Participant { get; set; }

        public string ParticipantPictureUrl { get; set; }

        public bool IsRead { get; set; }

        public DateTime? ReadOn { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Message, ConversationViewModel>()
                .ForMember(messageViewModel => messageViewModel.Sender, options => options.MapFrom(message => message.Sender.UserName));
            configuration.CreateMap<Message, ConversationViewModel>()
                .ForMember(messageViewModel => messageViewModel.SenderPictureUrl, options => options.MapFrom(message => message.Sender.ImageUrl));
        }
    }
}