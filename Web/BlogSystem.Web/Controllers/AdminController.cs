namespace BlogSystem.Web.Controllers
{
    using System.Web.Mvc;

    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        // GET: Admin
        public ActionResult Index()
        {
            return this.View();
        }
    }
}