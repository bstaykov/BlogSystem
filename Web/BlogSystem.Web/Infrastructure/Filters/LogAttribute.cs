namespace BlogSystem.Web.Infrastructure.Filters
{
    using System.Web.Mvc;
    using Microsoft.AspNet.Identity;
    using BlogSystem.Models;
    using BlogSystem.Common.Repository;
    using BlogSystem.Data;
    using System;

    public class LogAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var user = filterContext.HttpContext.User.Identity.Name;
            var url = filterContext.HttpContext.Request.RawUrl;
            //System.Console.WriteLine(user);

            //BlogSystemDbContext dbContext = new BlogSystemDbContext();
            //dbContext.Logs.Add(new Log() { User = user, Url = url });
            //dbContext.SaveChanges();

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