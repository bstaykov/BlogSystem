namespace BlogSystem.Web.Infrastructure.Filters
{
    using System;
    using System.Linq;
    using System.Web.Mvc;

    using BlogSystem.Common.Repository;
    using BlogSystem.Data;
    using BlogSystem.Models;

    using Microsoft.AspNet.Identity;

    public class LogAttribute : ActionFilterAttribute
    {
        private readonly IBlogSystemData data;

        public LogAttribute()
            : this(new BlogSystemData(new BlogSystemDbContext()))
        {
        }

        public LogAttribute(IBlogSystemData data)
        {
            this.data = data;
        } 

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // this.SaveLogToDb(filterContext, LogStatus.ActionExecuting);
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            this.SaveLogToDb(filterContext, LogStatus.ActionExecuted);

            base.OnActionExecuted(filterContext);
        }

        private void SaveLogToDb(ControllerContext filterContext, LogStatus status)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated == false)
            {
                return;
            }

            var userName = filterContext.HttpContext.User.Identity.Name;
            var userId = this.data.Users.All().Where(user => user.UserName == userName).Select(user => user.Id).FirstOrDefault();
            var url = filterContext.HttpContext.Request.RawUrl;
            var log = new Log() { UserId = userId, Url = url, Status = status };
            this.data.Logs.Add(log);
            this.data.SaveChanges();
        }
    }
}