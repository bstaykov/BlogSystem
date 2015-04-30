namespace BlogSystem.Web.Areas.Posts.Models
{
    using System;

    using AutoMapper;

    using BlogSystem.Models;
    using BlogSystem.Web.Infrastructure.Mapping;

    public class CommentViewModel : IMapFrom<Comment>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Content { get; set; }

        public int Likes { get; set; }

        public int ReplyCommentsCount { get; set; }

        public int PostId { get; set; }

        public int? ParentCommentId { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Comment, CommentViewModel>()
                .ForMember(comment => comment.UserName, options => options.MapFrom(comment => comment.User.UserName));
        }
    }
}