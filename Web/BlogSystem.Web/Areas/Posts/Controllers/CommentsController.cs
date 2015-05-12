namespace BlogSystem.Web.Areas.Posts.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Data.SqlClient;
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
        
        [HttpGet]
        public ActionResult CommentPost(int postId, int? parentCommentId)
        {
            return this.PartialView("_CommentForm", new CommentInputModel() { PostId = postId, ParentCommentId = parentCommentId });
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
                    var post = this.Data.Posts.GetById(comment.PostId);
                    post.CommentsCount -= 1;
                    if (comment.ParentCommentId != null)
                    {
                        comment.ParentComment.ReplyCommentsCount -= 1;
                    }

                    if (comment.ReplyCommentsCount > 0)
                    {
                        this.DeleteSubComment(comment, post);
                    }

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
                ModelState.AddModelError(string.Empty, "Invalid data! (User)");
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

        [NonAction]
        public void DeleteSubComment(Comment comment, Post post)
        {
            foreach (var reply in comment.ReplyComments)
            {
                reply.IsDeleted = true;
                if (reply.ReplyCommentsCount > 0)
                {
                    this.DeleteSubComment(reply, post);
                }
            }

            post.CommentsCount -= comment.ReplyCommentsCount;
        }

        [HttpGet]
        public ActionResult PostCommentedMesseges(int page = 1)
        {
            if (page < 1)
            {
                page = 1;
            }

            var userName = this.User.Identity.Name;
            var comments = this.Data.Comments.All().Where(comment => comment.User.UserName != userName && comment.Post.User.UserName == userName && comment.IsDeleted == false)
                .Project().To<LastCommentViewModel>()
                .OrderByDescending(comment => comment.CreatedOn)
                .Skip((page - 1) * 5)
                .Take(5)
                .ToList();

            var model = new LastCommentsPageViewModel()
            {
                Page = page + 1,
                Comments = comments,
            };

            return this.PartialView("_PostCommentedMesseges", model);
        }

        [HttpGet]
        public ActionResult ReadComments(int postId)
        {
            var userName = this.User.Identity.Name;

            var currentPost = this.Data.Posts.All().Project().To<PostViewModel>()
                .FirstOrDefault(post => post.Author == userName && post.Id == postId);

            if (currentPost == null)
            {
                this.TempData["error"] = "Post not found.";
                return this.PartialView("Error");
            }

            var comments = this.Data.Comments.All().Where(comment => comment.IsReadByAuthor == false && comment.PostId == postId);

            if (comments.Count() != 0)
            {
                foreach (var comment in comments)
                {
                    comment.IsReadByAuthor = true;
                }

                this.Data.SaveChanges();
            }

            return this.PartialView("Post", currentPost);
        }

        [HttpGet]
        public ActionResult MarkAllCommentsAsRead()
        {
            var userId = this.User.Identity.GetUserId();

            var comments = this.Data.Comments.All().Where(comment => comment.Post.UserId == userId && comment.IsReadByAuthor == false);

            foreach (var comment in comments)
            {
                comment.IsReadByAuthor = true;
            }

            this.Data.SaveChanges();

            this.TempData["success"] = "All comments marked as read.";

            return this.PartialView("_Message");
        }

        [HttpGet]
        public ActionResult LastComments()
        {
            SqlCommand command = new SqlCommand();
            SqlDependency dependancy = new SqlDependency(command);

            var userId = this.User.Identity.GetUserId();

            var info = this.Data.Comments.All().Where(comment => comment.IsReadByAuthor == false && comment.UserId == userId).ToList();

            foreach (var comment in info)
            {
                comment.IsReadByAuthor = true;
            }

            this.Data.SaveChanges();

            return this.Json(info, JsonRequestBehavior.AllowGet);
        }

        // private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        // {            
        // }

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