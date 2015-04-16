namespace BlogSystem.Web.Areas.Posts.Controllers
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Web.Mvc;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using BlogSystem.Common.Repository;
    using BlogSystem.Data;
    using BlogSystem.Models;
    using BlogSystem.Web.Areas.Posts.Models;
    using BlogSystem.Web.Infrastructure.Filters;

    using Microsoft.AspNet.Identity;

    public class HomeController : Controller
    {
        private readonly IBlogSystemData data;

        public HomeController()
            : this(new BlogSystemData(new BlogSystemDbContext()))
        {
        }

        public HomeController(IBlogSystemData data)
        {
            this.data = data;
        }

        [HttpGet]
        [OutputCache(Duration = 1)]
        public ActionResult Index()
        {
            var allPosts = this.data.Posts.All()
                .AsQueryable()
                .Project().To<PostViewModel>()
                .OrderBy(post => post.DateTimePosted);

            if (allPosts.Count() == 0) 
            {
                this.TempData["error"] = "No posts yet!";
                return this.View();
            }

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
                this.data.Posts.Add(newPost);
                this.data.Posts.SaveChanges();

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
            var myPosts = this.data.Posts.All()
                .AsQueryable()
                .Where(post => post.UserId == userId)
                .Project().To<MyPostViewModel>()
                .OrderBy(post => post.DateTimePosted);

            return this.View(myPosts);
        }

        [HttpPost]
        [Authorize]
        public ActionResult DeletePost(int id)
        {
            // TODO DELETE Sleep
            Thread.Sleep(2000);

            var userId = this.User.Identity.GetUserId();

            var postToDelete = this.data.Posts.GetById(id);

            if (postToDelete == null || postToDelete.UserId != userId)
            {
                this.TempData["error"] = "Incorrect id (Cheater)!";
            }
            else
            {
                this.data.Posts.Delete(postToDelete);
                this.data.SaveChanges();
                this.TempData["success"] = "Post deleted successfully.";
            }

            return this.PartialView("_DeletePost");
        }

        [HttpPost]
        [Authorize]
        public ActionResult DeleteAllPost()
        {
            var userId = this.User.Identity.GetUserId();

            var posts = this.data.Posts.All().Where(post => post.UserId == userId);

            if (posts == null)
            {
                this.TempData["error"] = "No posts found!";
            }
            else
            {
                foreach (var post in posts)
                {
                    this.data.Posts.Delete(post);
                }

                this.data.SaveChanges();
                this.TempData["success"] = "Posts deleted successfully.";
            }

            return this.RedirectToAction("Index");
        }
    }
}