namespace BlogSystem.Web.Areas.Messages.Models
{
    using System;
    using System.Collections.Generic;

    using AutoMapper;

    using BlogSystem.Models;
    using BlogSystem.Web.Infrastructure.Mapping;

    public class MessageViewModel : IMapFrom<Message>, IHaveCustomMappings
    {
        public string Content { get; set; }

        public DateTime SendOn { get; set; }

        public MessageParticipantInfo ParticipantInformation { get; set; }

        public string ParticipantName { get; set; }

        public string ParticipantPictureUrl { get; set; }

        public string Sender { get; set; }

        public string SenderPictureUrl { get; set; }

        public bool IsRead { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Message, MessageViewModel>()
                .ForMember(messageViewModel => messageViewModel.Sender, options => options.MapFrom(message => message.Sender == 0 ? message.Dialog.FirstUser.UserName : message.Dialog.SecondUser.UserName));
            configuration.CreateMap<Message, MessageViewModel>()
                .ForMember(messageViewModel => messageViewModel.SenderPictureUrl, options => options.MapFrom(message => message.Sender == 0 ? message.Dialog.FirstUser.ImageUrl : message.Dialog.SecondUser.ImageUrl));

            //configuration.CreateMap<Message, MessageViewModel>()
            //    .ForMember(messageViewModel => messageViewModel.SenderPictureUrl, options => options.MapFrom(message => message.User.ImageUrl));

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