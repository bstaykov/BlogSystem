namespace BlogSystem.Web.Areas.Posts.Controllers
{
    using System.Web.Mvc;
    using System.Data.Entity.Infrastructure;

    using BlogSystem.Web.Areas.Posts.Models;
    using BlogSystem.Web.Controllers;
    using BlogSystem.Models;
    using AutoMapper;
    using System;

    [Authorize]
    public class CommentsController : BaseController
    {
        [HttpPost]
        public ActionResult GetCommentForm(int postId)
        {
            return this.PartialView("_CommentForm", new CommentInputModel() { PostId = postId});
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            return this.View();
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