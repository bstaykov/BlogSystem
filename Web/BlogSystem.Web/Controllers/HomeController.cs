namespace BlogSystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Microsoft.AspNet.Identity;
    using BlogSystem.Web.Models;
    using BlogSystem.Models;
    using System.Data.Entity;
    using BlogSystem.Common.Repository;
    using BlogSystem.Data;
    using System.Threading;
    using BlogSystem.Web.ViewModels;

    public class HomeController : Controller
    {
        private IRepository<User> users;

        public HomeController()
            :this(new GenericRepository<User>(new BlogSystemDbContext()))
        {

        }

        public HomeController(IRepository<User> users)
        {
            this.users = users;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [ChildActionOnly]
        public ActionResult GetUserInfo()
        {
            var userName = this.User.Identity.Name;
            string userUrl = this.users.All().Where(u => u.UserName == userName).First().ImageUrl;
            if (userUrl == null)
            {
                userUrl = "http://imgs.abduzeedo.com/files/articles/baby-animals/Baby-Animals-002.jpg";
            }
            return this.PartialView("_GetUserInfo", userUrl);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            var mess = "Your contact page.";

            ViewBag.Message = mess;

            return View();
        }

        public ActionResult Link1()
        {
            return this.PartialView("_Link1");
        }

        public ActionResult Link2()
        {
            Thread.Sleep(2000);
            return this.PartialView("_Link2");
        }

        [HttpGet]
        public ActionResult Form()
        {
            return this.PartialView("_Form", new FormInputModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Form(FormInputModel model)
        {
            if (ModelState.IsValid)
            {
                string result = string.Format("Name: {0}, Age: {1}", model.Name ,model.Age);
                return this.Content(result);
            }

            return this.PartialView("_Form", model);
        }

        [HttpGet]
        public ActionResult FormServer()
        {
            return this.PartialView("_FormServer", new FormInputModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FormServer(FormInputModel model)
        {
            if (ModelState.IsValid)
            {
                this.TempData["success"] = "Form send successfully.";
                return RedirectToAction("Index");
            }

            return this.PartialView("_FormServer", model);
        }

        [HttpGet]
        public ActionResult FormPage()
        {
            return this.View(new FormInputModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [OutputCache(Duration=15*60)]
        public ActionResult FormPage(FormInputModel model)
        {
            if (ModelState.IsValid)
            {
                this.TempData["success"] = "Form send successfully.";
                return RedirectToAction("Index");
            }

            return this.View(model);
        }

        [HttpGet]
        public ActionResult HtmlEditors()
        {
            return this.View(new TestEditorsModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HtmlEditors(TestEditorsModel model, List<int> extraDonations, List<Person> person)
        {
            //TODO check People for NULL values

            DateTime date;

            if (DateTime.TryParse(string.Format(""), out date) == false)
            {
                ModelState.AddModelError(string.Empty, "Date is invalid!");
            }

            if (ModelState.IsValid)
            {
                var extra = "";
                if (extraDonations != null)
                {
                    extra = string.Join(",", extraDonations);
                }
                this.TempData["success"] = model.ToString() + " " + extra;
                return RedirectToAction("HtmlEditors");
            }

            return this.View(model);
        }

        [HttpGet]
        public ActionResult EnumEditors()
        {
            return this.View(new EnumModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EnumEditors(EnumModel model)
        {
            if (ModelState.IsValid)
            {
                this.TempData["success"] = model.Color.ToString();
                return RedirectToAction("EnumEditors");
            }

            return this.View(model);
        }

        [HttpGet]
        public ActionResult Breadcrumbs(BreadscrumbsModel model)
        {
            return this.PartialView("_Breadcrumbs", model);
        }

        public ActionResult UserInfo(string username)
        {
            var userInfo = this.users.All().Where(u => u.UserName == username).FirstOrDefault();

            UserInfoModel user = new UserInfoModel()
            {
                UserName = userInfo.UserName,
                ImageUrl = userInfo.ImageUrl
            };

            return this.View(user);
        }

        public ActionResult ChatContent()
        {
            return this.PartialView("_ChatContent");
        }

        [HttpGet]
        public ActionResult TestDateTimeEditor()
        {
            return this.PartialView("_TestDateTimeEditor");
        }

        [HttpGet]
        public ActionResult TestDateTime()
        {
            return this.View(DateTime.Now);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TestDateTime(int Day, int Month, int Year)
        {
            DateTime date;

            if (DateTime.TryParse(string.Format("{0}/{1}/{2}", Month, Day, Year), out date) == false)
            {
                ModelState.AddModelError(string.Empty, "Date is invalid!");
            }

            if (ModelState.IsValid)
            {
                this.TempData["success"] = "SUCCESS";

                return RedirectToAction("TestDateTime");
            }

            this.TempData["error"] = "Invalid date!";

            return this.View(new DateTime());
        }

        [HttpGet]
        public ActionResult TestDateTimeComplex()
        {
            return this.View(new TestDateTimeComplexInputModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TestDateTimeComplex(TestDateTimeComplexInputModel DateAdded)
        {
            DateTime date;

            if (DateTime.TryParse(string.Format("{0}/{1}/{2}", DateAdded.Month, DateAdded.Day, DateAdded.Year), out date) == false)
            {
                ModelState.AddModelError(string.Empty, "Date is invalid!");
            }

            DateAdded.DateAdded = date;

            if (ModelState.IsValid)
            {
                this.TempData["success"] = "SUCCESS";

                return RedirectToAction("TestDateTime");
            }

            this.TempData["error"] = "Invalid date!";

            return this.View(DateAdded);
        }
    }
}