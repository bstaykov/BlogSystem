namespace BlogSystem.Web.Areas.Administration.Controllers
{
    using System.Web.Mvc;

    public class LogsController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return this.View();
        }
    }
}