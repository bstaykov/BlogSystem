namespace BlogSystem.Web.Areas.Posts.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using BlogSystem.Models;
    using BlogSystem.Web.Areas.Posts.Models;
    using BlogSystem.Web.Controllers;
    using BlogSystem.Web.ViewModels;

    [Authorize]
    public class CommentsController : BaseController
    {
        [HttpPost]
        public ActionResult GetCommentForm(int postId)
        {
            return this.PartialView("_CommentForm", new CommentInputModel() { PostId = postId });
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ViewComments(int id, int pageNumber = 1, int commentsPerPage = 5)
        {
            BlogSystem.Web.Areas.Posts.Controllers.HomeController.CheckParams(ref pageNumber, ref commentsPerPage);

            List<CommentListViewModel> commentsToDisplay = this.Data.Comments.All()
                .AsQueryable()
                .Where(comment => comment.PostId == id)
                .Project().To<CommentListViewModel>()
                .OrderBy(comment => comment.CreatedOn)
                .Skip((pageNumber - 1) * commentsPerPage)
                .Take(commentsPerPage)
                .ToList();

            int commentsCount = this.Data.Comments.All()
                .Where(comment => comment.PostId == id)
                .Count();

            if (commentsToDisplay == null)
            {
                this.TempData["error"] = "Error while loading comments!";
                return this.PartialView("Error");
            }

            if (commentsCount == 0)
            {
                this.TempData["error"] = "No comments yet!";
                return this.PartialView("Error");
            }

            int startPage;
            int endPage;
            int availablePages;

            BlogSystem.Web.Areas.Posts.Controllers.HomeController.SetPagingParams(commentsCount, commentsPerPage, pageNumber, out availablePages, out startPage, out endPage);

            var pagingModel = new CommentsPagingViewModel()
            {
                Comments = commentsToDisplay,
                Pagination = new PaginationModel() 
                {
                    AreaName = "Posts",
                    ActionName = "ViewComments",
                    StartPage = startPage,
                    CurrentPage = pageNumber,
                    EndPage = endPage,
                    AvailablePages = availablePages,
                    Id = id,
                    UpdateTarget = "commentsListed",
                },
            };

            return this.PartialView("_Comments", pagingModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CommentPost(CommentInputModel model)
        {
            var postId = this.Data.Posts.GetById(model.PostId);
            if (postId == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid post!");
            }

            if (ModelState.IsValid == false)
            {
                this.TempData["error"] = "Invalid data!";
                return this.PartialView("_Message");
            }

            Mapper.CreateMap<CommentInputModel, Comment>();
            Comment newComment = Mapper.Map<Comment>(model);
            newComment.UserName = User.Identity.Name;
            newComment.CreatedOn = DateTime.Now;
            this.Data.Comments.Add(newComment);
            try
            {
                this.Data.Comments.SaveChanges();
                this.Data.Posts.GetById(model.PostId).CommentsCount += 1;
                this.Data.Posts.SaveChanges();
                this.TempData["success"] = "Thanks! Contribution is important!";
            }
            catch (DbUpdateException)
            {
                this.TempData["error"] = "Error while saving comment!";
            }

            return this.PartialView("_Message");
        }

        public ActionResult ReplyToComment()
        {
            return this.View();
        }

        public ActionResult Delete()
        {
            return this.View();
        }

        public ActionResult Update()
        {
            return this.View();
        }
    }
}