namespace BlogSystem.Web.Areas.Posts.Models
{
    using System;

    using AutoMapper;

    using BlogSystem.Models;
    using BlogSystem.Web.Infrastructure.Mapping;

    public class LastCommentViewModel : IMapFrom<Comment>, IHaveCustomMappings
    {
        public string UserName { get; set; }

        public string PictureUrl { get; set; }

        public int PostId { get; set; }

        public bool IsReadByAuthor { get; set; }

        public DateTime CreatedOn { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Comment, LastCommentViewModel>()
                .ForMember(comment => comment.UserName, options => options.MapFrom(comment => comment.User.UserName));
            configuration.CreateMap<Comment, LastCommentViewModel>()
                .ForMember(comment => comment.PictureUrl, options => options.MapFrom(comment => comment.User.ImageUrl));
        }
    }
}