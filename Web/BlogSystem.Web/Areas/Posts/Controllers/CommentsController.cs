namespace BlogSystem.Web.Areas.Posts.Controllers
{
    using System.Web.Mvc;

    using BlogSystem.Web.Controllers;

    [Authorize]
    public class CommentsController : BaseController
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return this.View();
        }

        public ActionResult CommentPost()
        {
            return this.View();
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