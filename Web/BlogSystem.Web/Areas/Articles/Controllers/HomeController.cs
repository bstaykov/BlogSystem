namespace BlogSystem.Web.Areas.Articles.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        // GET: Articles/Home
        public ActionResult Index()
        {
            return this.PartialView("Index");
        }

        [HttpGet]
        public ActionResult InsertArticle()
        {
            return this.PartialView("InsertArticle");
        }

        [HttpGet]
        public ActionResult ListedArticles()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ListedArticles2()
        {
            return View();
        }
    }
}