﻿namespace BlogSystem.Web.Areas.Posts.Controllers
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
        [OutputCache(Duration = 60)]
        public ActionResult Index()
        {
            return this.View();
        }

        [HttpGet]
        public ActionResult Post(int id)
        {
            var post = this.Data.Posts.All()
                .AsQueryable()
                .Where(p => p.Id == id && p.IsDeleted == false)
                .Project().To<PostViewModel>().FirstOrDefault();

            if (post == null)
            {
                this.TempData["error"] = "Error while loading post! (Repost)";
                return this.View("Error");
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
        [OutputCache(Duration = 60, VaryByParam = "pageNumber")]
        public ActionResult Posts(int pageNumber = 1, int postsPerPage = 2)
        {
            PagingHelper.CheckParams(ref pageNumber, ref postsPerPage);

            List<PostListViewModel> postsToDisplay = this.Data.Posts.All()
                .AsQueryable()
                .Where(post => post.IsDeleted == false)
                .Project().To<PostListViewModel>()
                .OrderByDescending(post => post.CreatedOn)
                .Skip((pageNumber - 1) * postsPerPage)
                .Take(postsPerPage)
                .ToList();

            int postsCount = this.Data.Posts.All()
                .Where(post => post.IsDeleted == false).Count();

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
        public ActionResult InsertPostForm(PostInputModel postModel)
        {
            if (ModelState.IsValid)
            {
                Mapper.CreateMap<PostInputModel, Post>();
                Post newPost = Mapper.Map<Post>(postModel);
                newPost.CreatedOn = DateTime.Now;
                newPost.UserId = User.Identity.GetUserId();
                this.Data.Posts.Add(newPost);
                try
                {
                    int result = this.Data.Posts.SaveChanges();
                    if (result == 1)
                    {
                        return this.RedirectToAction("DisplayPost", routeValues: new { id = newPost.Id });

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
                            // this.TempData["error"] = "Error! Post title duplicates!";
                            ModelState.AddModelError(string.Empty, "Error! Post title duplicates!");
                        }
                        else
                        {
                            // this.TempData["error"] = "Error! Post was not added!";
                            ModelState.AddModelError(string.Empty, "SQL error! Try again!");
                        }
                    }
                    else
                    {
                        // this.TempData["error"] = "Error! Post was not added!";
                        ModelState.AddModelError(string.Empty, "Error! Try again!");
                    }
                }
            }

            return this.PartialView("_InsertPostForm", postModel);
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

        [HttpDelete]
        [Authorize]
        public ActionResult DeletePost(int id)
        {
            var userId = this.User.Identity.GetUserId();

            var postToDelete = this.Data.Posts.GetById(id);

            if (postToDelete == null || postToDelete.UserId != userId)
            {
                this.TempData["error"] = "Incorrect post id (Cheater)!";
                return this.PartialView("Error");
            }
            else
            {
                postToDelete.IsDeleted = true;
                this.Data.SaveChanges();
            }

            return this.PartialView("_RePostLink", id);
        }

        [HttpPut]
        [Authorize]
        public ActionResult RePost(int id)
        {
            var userId = this.User.Identity.GetUserId();

            var postToRepost = this.Data.Posts.GetById(id);

            if (postToRepost == null || postToRepost.UserId != userId)
            {
                this.TempData["error"] = "Incorrect post id (Cheater)!";
                return this.PartialView("Error");
            }
            else
            {
                postToRepost.IsDeleted = false;
                this.Data.SaveChanges();
            }

            return this.PartialView("_EditLinks", id);
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

        [HttpGet]
        [Authorize]
        public ActionResult EditPost(int id)
        {
            var post = this.Data.Posts.GetById(id);
            Mapper.CreateMap<Post, PostEditModel>();
            PostEditModel postToBeEdited = Mapper.Map<PostEditModel>(post);
            return this.PartialView("_EditPostForm", postToBeEdited);
        }

        [HttpPut]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(PostEditModel model)
        {
            var post = this.Data.Posts.GetById(model.Id);
            if (post.Id != model.Id || post.UserId != this.User.Identity.GetUserId())
            {
                ModelState.AddModelError(string.Empty, "Invalid data! (User)");
            }

            if (ModelState.IsValid == false)
            {
                this.TempData["error"] = "Invalid data!";
                return this.PartialView("_Message");
            }

            post.ModifiedOn = DateTime.Now;
            post.Content = model.Content;
            post.Title = model.Title;
            post.Category = model.Category;
            try
            {
                this.Data.Posts.SaveChanges();
                this.TempData["success"] = "Post updated!";
                Mapper.CreateMap<Post, PostViewModel>();
                PostViewModel editedPost = Mapper.Map<PostViewModel>(post);

                return this.PartialView("Post", editedPost);
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
                        this.TempData["error"] = "Error! Post was not edited!";
                    }
                }
                else
                {
                    this.TempData["error"] = "Error! Post was not edited!";
                }

                return this.PartialView("_EditPostForm", model);
            }
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