namespace BlogSystem.Web.Areas.Messages.Models
{
    using System;
    using System.Collections.Generic;

    using AutoMapper;

    using BlogSystem.Models;
    using BlogSystem.Web.Infrastructure.Mapping;

    public class MessageViewModel : IMapFrom<MessageAlertsModel>, IHaveCustomMappings
    {
        public string Content { get; set; }

        public DateTime SendOn { get; set; }

        public string Sender { get; set; }

        public string SenderPictureUrl { get; set; }

        public string Receiver { get; set; }

        public string ReceiverPictureUrl { get; set; }

        // public bool IsRead { get; set; }
        public int UnreadMessagesCount { get; set; }

        // public DateTime? ReadOn { get; set; }
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<MessageAlertsModel, MessageViewModel>()
                .ForMember(messageViewModel => messageViewModel.Content, options => options.MapFrom(message => message.Message.Content));
            configuration.CreateMap<MessageAlertsModel, MessageViewModel>()
                .ForMember(messageViewModel => messageViewModel.SendOn, options => options.MapFrom(message => message.Message.SendOn));

            configuration.CreateMap<MessageAlertsModel, MessageViewModel>()
                .ForMember(messageViewModel => messageViewModel.Sender, options => options.MapFrom(message => message.Message.Sender.UserName));
            configuration.CreateMap<MessageAlertsModel, MessageViewModel>()
                .ForMember(messageViewModel => messageViewModel.SenderPictureUrl, options => options.MapFrom(message => message.Message.Sender.ImageUrl));
            configuration.CreateMap<MessageAlertsModel, MessageViewModel>()
                .ForMember(messageViewModel => messageViewModel.Receiver, options => options.MapFrom(message => message.Message.Receiver.UserName));
            configuration.CreateMap<MessageAlertsModel, MessageViewModel>()
                .ForMember(messageViewModel => messageViewModel.ReceiverPictureUrl, options => options.MapFrom(message => message.Message.Receiver.ImageUrl));
        }
    }
}