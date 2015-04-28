namespace BlogSystem.Web.Areas.Posts.Controllers
{
    using System.Web.Mvc;

    using BlogSystem.Web.Areas.Posts.Models;
    using BlogSystem.Web.Controllers;

    [Authorize]
    public class CommentsController : BaseController
    {
        [HttpGet]
        public ActionResult GetCommentForm()
        {
            return this.PartialView("_CommentForm", new CommentInputModel());
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
            if (ModelState.IsValid == false)
            {
                this.TempData["error"] = "Invalid model state!";
                return this.PartialView("_Message");
            }

            this.TempData["success"] = "To be or not to be commented!";
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