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
    using BlogSystem.Web.Infrastructure.Pagging;
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
        public ActionResult Post(int id)
        {
            var post = this.Data.Posts.All()
                .AsQueryable()
                .Project().To<PostViewModel>()
                .FirstOrDefault(p => p.Id == id);

            if (post == null)
            {
                return null;
            }

            return this.View("Post", post);
        }

        [HttpGet]
        public ActionResult DisplayPost(int id)
        {
            var post = this.Data.Posts.All()
                .AsQueryable()
                .Project().To<PostViewModel>()
                .FirstOrDefault(p => p.Id == id);

            if (post == null)
            {
                return null;
            }

            return this.PartialView("Post", post);
        }

        [HttpGet]
        [OutputCache(Duration = 1, VaryByParam = "pageNumber")]
        public ActionResult Posts(int pageNumber = 1, int postsPerPage = 2)
        {
            PagingHelper.CheckParams(ref pageNumber, ref postsPerPage);

            List<PostListViewModel> postsToDisplay = this.Data.Posts.All()
                .AsQueryable()
                .Project().To<PostListViewModel>()
                .OrderByDescending(post => post.CreatedOn)
                .Skip((pageNumber - 1) * postsPerPage)
                .Take(postsPerPage)
                .ToList();

            int postsCount = this.Data.Posts.All().Count();

            if (postsToDisplay == null)
            {
                this.TempData["error"] = "Error while loading posts!";
                return this.PartialView("Error");
            }

            if (postsCount == 0)
            {
                this.TempData["error"] = "No posts yet!";
                return this.PartialView("Error");
            }

            int startPage;
            int endPage;
            int availablePages;

            PagingHelper.SetPagingParams(postsCount, postsPerPage, pageNumber, out availablePages, out startPage, out endPage);

            var pagingModel = new PostsPagingViewModel()
            {
                Posts = postsToDisplay,
                Pagination = new PaginationModel() 
                {
                    AreaName = "Posts",
                    ActionName = "Posts",
                    StartPage = startPage,
                    CurrentPage = pageNumber,
                    EndPage = endPage,
                    AvailablePages = availablePages,
                    UpdateTarget = "postsShown"
                },
            };

            return this.PartialView("_Posts", pagingModel);
        }

        [HttpGet]
        [Authorize]
        public ActionResult InsertPost()
        {
            return this.View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult InsertPostForm()
        {
            return this.PartialView("_InsertPostForm", new PostInputModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult InsertPostForm(PostInputModel post)
        {
            if (ModelState.IsValid)
            {
                Mapper.CreateMap<PostInputModel, Post>();
                Post newPost = Mapper.Map<Post>(post);
                newPost.CreatedOn = DateTime.Now;
                newPost.UserId = User.Identity.GetUserId();
                this.Data.Posts.Add(newPost);
                try
                {
                    int result = this.Data.Posts.SaveChanges();
                    if (result == 1)
                    {
                        return this.RedirectToAction("DisplayPost", new { id = newPost.Id });

                        // this.TempData["id"] = newPost.Id;

                        // return this.RedirectToAction("InsertPostForm");
                    }

                    this.TempData["error"] = "Post was not added!";
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                {
                    if (ex.InnerException.InnerException is System.Data.SqlClient.SqlException)
                    {
                        var sqlException = ex.InnerException.InnerException as System.Data.SqlClient.SqlException;

                        if (sqlException.Number == 2601)
                        {
                            this.TempData["error"] = "Error! Post title duplicates!";
                        }
                        else
                        {
                            this.TempData["error"] = "Error! Post was not added!";
                        }
                    }
                    else
                    {
                        this.TempData["error"] = "Error! Post was not added!";
                    }
                }
            }

            return this.PartialView("_InsertPostForm", post);
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
            PagingHelper.CheckParams(ref pageNumber, ref postsPerPage);

            var userId = User.Identity.GetUserId();
            var postsToDisplay = this.Data.Posts.All()
                .AsQueryable()
                .Where(post => post.UserId == userId)
                .Project().To<MyPostViewModel>()
                .OrderByDescending(post => post.CreatedOn)
                .Skip((pageNumber - 1) * postsPerPage)
                .Take(postsPerPage)
                .ToList();

            int postsCount = this.Data.Posts.All().Where(post => post.UserId == userId).Count();

            if (postsToDisplay == null)
            {
                this.TempData["error"] = "Error while loading your posts!";
                return this.PartialView("Error");
            }

            if (postsCount == 0)
            {
                this.TempData["error"] = "No posts yet!";
                return this.PartialView("Error");
            }

            int startPage;
            int endPage;
            int availablePages;

            PagingHelper.SetPagingParams(postsCount, postsPerPage, pageNumber, out availablePages, out startPage, out endPage);

            var pagingModel = new MyPostsPagingViewModel()
            {
                Posts = postsToDisplay,
                Pagination = new PaginationModel()
                {
                    AreaName = "Posts",
                    ActionName = "MyPostsPartial",
                    StartPage = startPage,
                    CurrentPage = pageNumber,
                    EndPage = endPage,
                    AvailablePages = availablePages,
                    UpdateTarget = "postsShown",
                },
            };

            return this.PartialView("_MyPostsPartial", pagingModel);
        }

        [HttpPost]
        [Authorize]
        public ActionResult DeletePost(int id)
        {
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

        // TODO Delete Me
        public ActionResult DeleteAllPostTemp()
        {
            var posts = this.Data.Posts.All();

            foreach (var post in posts)
            {
                this.Data.Posts.Delete(post);
            }

            this.Data.SaveChanges();

            return null;
        }
    }
}