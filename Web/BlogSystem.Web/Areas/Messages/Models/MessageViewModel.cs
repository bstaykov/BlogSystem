namespace BlogSystem.Web.Areas.Messages.Models
{
    using System;
    using System.Collections.Generic;

    using AutoMapper;

    using BlogSystem.Models;
    using BlogSystem.Web.Infrastructure.Mapping;

    public class MessageViewModel : IMapFrom<Message>, IHaveCustomMappings
    {
        public int DialogId { get; set; }

        public string Content { get; set; }

        public DateTime SendOn { get; set; }

        public IEnumerable<MessageParticipantInfo> ParticipantsInfo { get; set; }

        public IEnumerable<string> ReadBy { get; set; }

        public string Sender { get; set; }

        public string SenderPictureUrl { get; set; }

        // public bool IsRead { get; set; }

        // public string Receiver { get; set; }

        // public string ReceiverPictureUrl { get; set; }
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Message, MessageViewModel>()
                .ForMember(messageViewModel => messageViewModel.Sender, options => options.MapFrom(message => message.User.UserName));
            configuration.CreateMap<Message, MessageViewModel>()
                .ForMember(messageViewModel => messageViewModel.SenderPictureUrl, options => options.MapFrom(message => message.User.ImageUrl));

            // configuration.CreateMap<MessageContent, MessageViewModel>()
            // .ForMember(message => message.ParticipantsNames, options => options.MapFrom(message => message.User.ImageUrl));
            // configuration.CreateMap<Message, MessageViewModel>()
            // .ForMember(message => message.Receiver, options => options.MapFrom(message => message.Receiver.UserName));
            // configuration.CreateMap<Message, MessageViewModel>()
            // .ForMember(message => message.SenderPictureUrl, options => options.MapFrom(message => message.Sender.ImageUrl));
            // configuration.CreateMap<Message, MessageViewModel>()
            // .ForMember(message => message.ReceiverPictureUrl, options => options.MapFrom(message => message.Receiver.ImageUrl));
        }
    }
}