namespace BlogSystem.Web.Areas.Votes.Controllers
{
    using System.Web.Mvc;

    using BlogSystem.Common.Repository;
    using BlogSystem.Data;
    using BlogSystem.Models;

    [Authorize]
    public class HomeController : Controller
    {
        // private IRepository<PostLiker> postLiker;
        // private IRepository<Post> post;
        // public HomeController()
        // : this(new GenericRepository<PostLiker>(new BlogSystemDbContext()))
        // {
        // }
        // public HomeController(IRepository<PostLiker> postLiker)
        // {
        // this.postLiker = postLiker;
        // }
        public ActionResult UpVoteById(int postId)
        {
            return this.View();
        }

        public ActionResult DownVoteById(int postId)
        {
            return this.View();
        }
    }
}