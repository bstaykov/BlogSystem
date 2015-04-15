namespace BlogSystem.Web.Controllers
{
    using System.Web.Mvc;

    using BlogSystem.Web.Infrastructure.Filters;

    [Log]
    public abstract class BaseController : Controller
    {
    }
}