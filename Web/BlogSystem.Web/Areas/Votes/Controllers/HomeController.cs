namespace BlogSystem.Web.Areas.Votes.Controllers
{
    using BlogSystem.Common.Repository;
    using BlogSystem.Data;
    using BlogSystem.Models;
    using System.Web.Mvc;

    [Authorize]
    public class HomeController : Controller
    {
        //private IRepository<PostLiker> postLiker;
        //private IRepository<Post> post;

        //public HomeController()
        //    : this(new GenericRepository<PostLiker>(new BlogSystemDbContext()))
        //{

        //}
        //public HomeController(IRepository<PostLiker> postLiker)
        //{
        //    this.postLiker = postLiker;
        //}

        public ActionResult UpVoteById(int postId)
        {
            return View();
        }

        public ActionResult DownVoteById(int postId)
        {
            return View();
        }
    }
}