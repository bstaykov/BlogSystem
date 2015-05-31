namespace BlogSystem.Web.Areas.Messages.Models
{
    using System;
    using System.Collections.Generic;

    using AutoMapper;

    using BlogSystem.Models;
    using BlogSystem.Web.Infrastructure.Mapping;

    public class DialogMessagesViewModel : IMapFrom<Message>, IHaveCustomMappings
    {
        public string Content { get; set; }

        public DateTime SendOn { get; set; }

        public string Sender { get; set; }

        public string SenderPictureUrl { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            // configuration.CreateMap<Message, DialogMessagesViewModel>()
            // .ForMember(dialogMessagesViewModel => dialogMessagesViewModel.Sender, options => options.MapFrom(message => message.User.UserName));
            // configuration.CreateMap<Message, DialogMessagesViewModel>()
            // .ForMember(dialogMessagesViewModel => dialogMessagesViewModel.SenderPictureUrl, options => options.MapFrom(message => message.User.ImageUrl));
        }
    }
}