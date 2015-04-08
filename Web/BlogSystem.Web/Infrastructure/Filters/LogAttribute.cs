namespace BlogSystem.Web.Infrastructure.Filters
{
    using System.Web.Mvc;

    public class LogAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //// TODO save do db
            //var user = filterContext.HttpContext.User.Identity.Name;
            //var url = filterContext.HttpContext.Request.RawUrl;

            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //// TODO save do db
            //var user = filterContext.HttpContext.User.Identity.Name;
            //var url = filterContext.HttpContext.Request.RawUrl;

            base.OnActionExecuted(filterContext);
        }
    }
}