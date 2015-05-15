namespace BlogSystem.Web.Areas.Posts.Models
{
    using System;
    using System.Linq.Expressions;

    using AutoMapper;

    using BlogSystem.Models;
    using BlogSystem.Web.Infrastructure.Mapping;

    using Microsoft.AspNet.Identity;

    public class TopPostViewModel : IMapFrom<Post>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public PostCategory Category { get; set; }

        public string Author { get; set; }

        public int Likes { get; set; }

        public int CommentsCount { get; set; }

        public int TimesRead { get; set; }
    }
}