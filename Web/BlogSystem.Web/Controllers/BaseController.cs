namespace BlogSystem.Web.Controllers
{
    using System.Web.Mvc;

    using BlogSystem.Data;
    using BlogSystem.Web.Infrastructure.Filters;

    // [Log]
    public abstract class BaseController : Controller
    {
        protected readonly IBlogSystemData Data;

        public BaseController()
            : this(new BlogSystemData(new BlogSystemDbContext()))
        {
        }

        public BaseController(IBlogSystemData data)
        {
            this.Data = data;
        } 
    }
}