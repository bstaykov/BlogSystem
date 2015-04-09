namespace BlogSystem.Web.Controllers
{
    using BlogSystem.Web.Infrastructure.Filters;
    using System.Web.Mvc;

    [Log]
    public abstract class BaseController : Controller
    {
        // GET: Base
        public ActionResult Index()
        {
            return View();
        }
    }
}