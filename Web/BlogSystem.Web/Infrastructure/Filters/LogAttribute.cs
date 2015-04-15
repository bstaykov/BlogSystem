namespace BlogSystem.Web.Infrastructure.Filters
{
    using System;
    using System.Web.Mvc;

    using BlogSystem.Common.Repository;
    using BlogSystem.Data;
    using BlogSystem.Models;

    using Microsoft.AspNet.Identity;

    public class LogAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var user = filterContext.HttpContext.User.Identity.Name;
            var url = filterContext.HttpContext.Request.RawUrl;

            // System.Console.WriteLine(user);
            // BlogSystemDbContext dbContext = new BlogSystemDbContext();
            // dbContext.Logs.Add(new Log() { User = user, Url = url });
            // dbContext.SaveChanges();
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // TODO save do db
            var user = filterContext.HttpContext.User.Identity.Name;
            var url = filterContext.HttpContext.Request.RawUrl;

            base.OnActionExecuted(filterContext);
        }
    }
}