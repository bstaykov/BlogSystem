namespace BlogSystem.Web.Areas.Posts.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using BlogSystem.Common.Repository;
    using BlogSystem.Models;
    using BlogSystem.Web.Areas.Posts.Models;
    using BlogSystem.Web.Infrastructure.Filters;

    using Microsoft.AspNet.Identity;

    public class HomeController : Controller
    {
        private readonly IRepository<Post> posts;

        public HomeController(IRepository<Post> posts)
        {
            this.posts = posts;
        }

        [HttpGet]
        [OutputCache(Duration = 1)]
        public ActionResult Index()
        {
            // var allPosts = this.posts.All()
            // .Select(PostViewModel.FromPost)
            // .OrderBy(post => post.DateTimePosted)
            // .ToList();
            var allPosts = this.posts.All()
                .AsQueryable()
                .Project().To<PostViewModel>()
                .OrderBy(post => post.DateTimePosted);

            return this.View(allPosts);
        }

        [HttpGet]
        [Authorize]
        public ActionResult InsertPost()
        {
            return this.View(new PostInputModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult InsertPost(PostInputModel post)
        {
            if (ModelState.IsValid)
            {
                Mapper.CreateMap<PostInputModel, Post>();
                Post newPost = Mapper.Map<Post>(post);
                newPost.DateTimePosted = DateTime.Now;
                newPost.UserId = User.Identity.GetUserId();

                // Post newPost = new Post()
                // {
                // Title = post.Title,
                // Content = post.Content,
                // DateTimePosted = DateTime.Now,
                // UserId = User.Identity.GetUserId(),
                // CommentsCount = 0,
                // Likes = 0
                // };
                this.posts.Add(newPost);
                this.posts.SaveChanges();

                this.TempData["success"] = "Post was added!";

                return this.RedirectToAction("InsertPost");
            }

            return this.View(post);
        }

        [HttpGet]
        [Authorize]
        [OutputCache(Duration = 1)]
        public ActionResult MyPosts()
        {
            string userId = User.Identity.GetUserId();

            // var myPosts = this.posts.All()
            // .AsQueryable()
            // .Where(post => post.UserId == userId)
            // .Select(MyPostViewModel.FromPost)
            // .OrderBy(post => post.DateTimePosted)
            // .ToList();
            var myPosts = this.posts.All()
                .AsQueryable()
                .Where(post => post.UserId == userId)
                .Project().To<MyPostViewModel>()
                .OrderBy(post => post.DateTimePosted);

            return this.View(myPosts);
        }
    }
}