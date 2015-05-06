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
    using BlogSystem.Web.Infrastructure.Pagging;
    using BlogSystem.Web.ViewModels;

    using Microsoft.AspNet.Identity;

    [Authorize]
    public class CommentsController : BaseController
    {
        [HttpPost]
        public ActionResult GetCommentForm(int postId, int? parentCommentId)
        {
            return this.PartialView("_CommentForm", new CommentInputModel() { PostId = postId, ParentCommentId = parentCommentId });
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ViewComments(int postId, int? parentCommentId = null, int pageNumber = 1, int commentsPerPage = 5)
        {
            PagingHelper.CheckParams(ref pageNumber, ref commentsPerPage);

            List<CommentViewModel> commentsToDisplay = this.Data.Comments.All()
                .AsQueryable()
                .Where(comment => comment.PostId == postId && comment.IsDeleted == false && comment.ParentCommentId == parentCommentId)
                .Project().To<CommentViewModel>()
                .OrderBy(comment => comment.CreatedOn)
                .Skip((pageNumber - 1) * commentsPerPage)
                .Take(commentsPerPage)
                .ToList();

            int commentsCount = this.Data.Comments.All().Where(comment => comment.PostId == postId && comment.IsDeleted == false && comment.ParentCommentId == parentCommentId).Count();

            if (commentsToDisplay == null)
            {
                this.TempData["error"] = "Error while loading comments!";

                return this.PartialView("Error");

                // return this.PartialView("_Message");
            }

            if (commentsCount == 0)
            {
                this.TempData["error"] = "No comments yet!";
                return this.PartialView("Error");

                // return this.PartialView("_Message");
            }

            int startPage;
            int endPage;
            int availablePages;

            PagingHelper.SetPagingParams(commentsCount, commentsPerPage, pageNumber, out availablePages, out startPage, out endPage);

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
                    PostId = postId,
                    ParentCommentId = parentCommentId,
                    UpdateTarget = "p" + postId + "pc" + parentCommentId,
                },
            };

            return this.PartialView("_Comments", pagingModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CommentPost(CommentInputModel model)
        {
            var post = this.Data.Posts.GetById(model.PostId);
            if (post == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid post!");
            }

            Comment parentComment = null;
            if (model.ParentCommentId != null)
            {
                int parentCommentId = (int)model.ParentCommentId;
                parentComment = this.Data.Comments.GetById(parentCommentId);
                if (parentComment != null && parentComment.PostId != post.Id) 
                {
                    ModelState.AddModelError(string.Empty, "PostId/CommentId mismatched!");
                }
            }

            if (ModelState.IsValid == false)
            {
                this.TempData["error"] = "Invalid data!";
                return this.PartialView("_Message");
            }

            Mapper.CreateMap<CommentInputModel, Comment>();
            Comment newComment = Mapper.Map<Comment>(model);
            newComment.UserId = this.User.Identity.GetUserId();
            newComment.CreatedOn = DateTime.Now;
            this.Data.Comments.Add(newComment);
            try
            {
                if (parentComment != null)
                {
                    parentComment.ReplyCommentsCount += 1;
                }

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

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var comment = this.Data.Comments.GetById(id);

            if (comment.User.UserName == User.Identity.Name)
            {
                try
                {
                    comment.IsDeleted = true;
                    this.Data.Posts.GetById(comment.PostId).CommentsCount -= 1;
                    this.Data.SaveChanges();
                    this.TempData["success"] = "Comment deleted.";
                }
                catch (DbUpdateException)
                {
                    this.TempData["error"] = "Error while saving comment!";
                }
            }

            return this.PartialView("_Message");
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            var comment = this.Data.Comments.GetById(id);
            Mapper.CreateMap<Comment, CommentUpdateModel>();
            CommentUpdateModel commentToBeEdited = Mapper.Map<CommentUpdateModel>(comment);
            return this.PartialView("_CommentUpdateForm", commentToBeEdited);
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public ActionResult Update(CommentUpdateModel model)
        {
            var comment = this.Data.Comments.GetById(model.Id);
            if (comment.PostId != model.PostId || comment.UserId != this.User.Identity.GetUserId())
            {
                ModelState.AddModelError(string.Empty, "Invalid data!");
            }

            if (ModelState.IsValid == false)
            {
                this.TempData["error"] = "Invalid data!";
                return this.PartialView("_Message");
            }

            comment.ModifiedOn = DateTime.Now;
            comment.Content = model.Content;
            try
            {
                this.Data.Comments.SaveChanges();
                this.TempData["success"] = "Thanks! Contribution is important!";

                return this.PartialView("_CommentUpdateResult", model.Content);
            }
            catch (DbUpdateException)
            {
                this.TempData["error"] = "Error while editing comment!";
            }

            return this.PartialView("_Message");
        }

        // TODO Delete Me
        public ActionResult DeleteAllCommentsTemp()
        {
            var comments = this.Data.Comments.All();

            foreach (var coment in comments)
            {
                this.Data.Comments.Delete(coment);
            }

            this.Data.SaveChanges();

            return null;
        }

        // TODO Delete Me
        public ActionResult DeleteAllLogsTemp()
        {
            var logs = this.Data.Logs.All();

            foreach (var log in logs)
            {
                this.Data.Logs.Delete(log);
            }

            this.Data.SaveChanges();

            return null;
        }
    }
}