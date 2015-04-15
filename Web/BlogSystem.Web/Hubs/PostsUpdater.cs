namespace BlogSystem.Web.Hubs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    using BlogSystem.Common.Repository;
    using BlogSystem.Data;
    using BlogSystem.Models;

    using Microsoft.AspNet.SignalR;

    public class PostsUpdater : Hub
    {
        // private IRepository<Post> posts = new GenericRepository<Post>(new BlogSystemDbContext());
        public void SendMessage(string message)
        {
            // var latestPosts = this.posts.All()
            // .AsQueryable()
            // .OrderByDescending(p => p.DateTimePosted)
            // .Select(p => new
            // {
            // Author = p.Author,
            // Title = p.Title,
            // Content = p.Content,
            // DatePosted = p.DateTimePosted
            // })
            // .Take(3)
            // .ToList();
            Clients.All.SendMessage(message);
        }
    }
}