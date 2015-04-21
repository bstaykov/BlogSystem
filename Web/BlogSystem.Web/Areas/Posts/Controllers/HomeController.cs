namespace BlogSystem.Web.Areas.Posts.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Web.Mvc;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using BlogSystem.Common.Repository;
    using BlogSystem.Data;
    using BlogSystem.Models;
    using BlogSystem.Web.Areas.Posts.Models;
    using BlogSystem.Web.Controllers;
    using BlogSystem.Web.Infrastructure.Filters;
    using BlogSystem.Web.ViewModels;

    using Microsoft.AspNet.Identity;

    public class HomeController : BaseController
    {
        [HttpGet]
        [OutputCache(Duration = 1)]
        public ActionResult Index()
        {
            return this.View();
        }

        [HttpGet]
        public ActionResult Posts(int pageNumber = 1, int postsPerPage = 2)
        {
            this.CheckParams(ref pageNumber, ref postsPerPage);

            List<PostViewModel> postsToDisplay = this.Data.Posts.All()
                .AsQueryable()
                .Project().To<PostViewModel>()
                .OrderByDescending(post => post.DateTimePosted)
                .Skip((pageNumber - 1) * postsPerPage)
                .Take(postsPerPage)
                .ToList();

            int postsCount = this.Data.Posts.All().Count();

            if (postsToDisplay == null)
            {
                this.TempData["error"] = "Error while loading posts!";
                return this.PartialView("_Posts");
            }

            if (postsCount == 0)
            {
                // TODO CHECK
                this.TempData["error"] = "No posts yet!";
                return this.PartialView("_Posts");
            }

            int startPage;
            int endPage;
            int availablePages;

            this.SetPagingParams(postsCount, postsPerPage, pageNumber, out availablePages, out startPage, out endPage);

            var pagingModel = new PostsPagingViewModel()
            {
                Posts = postsToDisplay,
                Pagination = new PaginationModel() 
                {
                    AreaName = "Posts",
                    ViewName = "Posts",
                    StartPage = startPage,
                    CurrentPage = pageNumber,
                    EndPage = endPage,
                    AvailablePages = availablePages,
                },
            };

            return this.PartialView("_Posts", pagingModel);
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
                this.Data.Posts.Add(newPost);
                this.Data.Posts.SaveChanges();

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
            return this.View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult MyPostsPartial(int pageNumber = 1, int postsPerPage = 3)
        {
            this.CheckParams(ref pageNumber, ref postsPerPage);

            string userId = User.Identity.GetUserId();
            var postsToDisplay = this.Data.Posts.All()
                .AsQueryable()
                .Where(post => post.UserId == userId)
                .Project().To<MyPostViewModel>()
                .OrderBy(post => post.DateTimePosted)
                .Skip((pageNumber - 1) * postsPerPage)
                .Take(postsPerPage)
                .ToList();

            int postsCount = this.Data.Posts.All().Where(post => post.UserId == userId).Count();

            if (postsToDisplay == null)
            {
                this.TempData["error"] = "Error while loading posts!";
                return this.PartialView("_MyPostsPartial");
            }

            if (postsCount == 0)
            {
                // TODO return no posts
                this.TempData["error"] = "No posts yet!";
                return this.PartialView("_MyPostsPartial");
            }

            int startPage;
            int endPage;
            int availablePages;

            this.SetPagingParams(postsCount, postsPerPage, pageNumber, out availablePages, out startPage, out endPage);

            var pagingModel = new MyPostsPagingViewModel()
            {
                Posts = postsToDisplay,
                Pagination = new PaginationModel()
                {
                    AreaName = "Posts",
                    ViewName = "MyPostsPartial",
                    StartPage = startPage,
                    CurrentPage = pageNumber,
                    EndPage = endPage,
                    AvailablePages = availablePages,
                },
            };

            return this.PartialView("_MyPostsPartial", pagingModel);
        }

        [HttpPost]
        [Authorize]
        public ActionResult DeletePost(int id)
        {
            // TODO DELETE Sleep
            Thread.Sleep(2000);

            var userId = this.User.Identity.GetUserId();

            var postToDelete = this.Data.Posts.GetById(id);

            if (postToDelete == null || postToDelete.UserId != userId)
            {
                this.TempData["error"] = "Incorrect id (Cheater)!";
            }
            else
            {
                this.Data.Posts.Delete(postToDelete);
                this.Data.SaveChanges();
                this.TempData["success"] = "Post deleted successfully.";
            }

            return this.PartialView("_DeletePost");
        }

        [HttpPost]
        [Authorize]
        [ChildActionOnly]
        public ActionResult DeleteAllPost()
        {
            var userId = this.User.Identity.GetUserId();

            var posts = this.Data.Posts.All().Where(post => post.UserId == userId);

            if (posts == null)
            {
                this.TempData["error"] = "No posts found!";
            }
            else
            {
                foreach (var post in posts)
                {
                    this.Data.Posts.Delete(post);
                }

                this.Data.SaveChanges();
                this.TempData["success"] = "Posts deleted successfully.";
            }

            return this.RedirectToAction("Index");
        }

        [NonAction]
        private void CheckParams(ref int pageNumber, ref int postsPerPage)
        {
            if (pageNumber <= 0)
            {
                pageNumber = 1;
            }

            if (postsPerPage <= 0 || 20 < postsPerPage)
            {
                postsPerPage = 2;
            }
        }

        [NonAction]
        private void SetPagingParams(int postsCount, int postsPerPage, int pageNumber, out int availablePages, out int startPage, out int endPage)
        {
            startPage = 1;
            endPage = 5;
            availablePages = (int)Math.Ceiling((double)postsCount / postsPerPage);

            if (availablePages <= 5)
            {
                startPage = 1;
                endPage = availablePages;
            }
            else
            {
                startPage = pageNumber - 2;
                endPage = pageNumber + 2;
                while (startPage < 1)
                {
                    startPage++;
                    endPage++;
                }

                while (endPage > availablePages)
                {
                    endPage--;
                    startPage--;
                }
            }
        }
    }
}