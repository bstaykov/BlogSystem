namespace BlogSystem.Web
{
    using BlogSystem.Web.App_Start;
    using BlogSystem.Web.Infrastructure.Mapping;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ViewenginesConfiguration.RegisterViewEngines(ViewEngines.Engines);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var autoMapperConfig = new AutoMapperConfig();
            autoMapperConfig.Execute();
        }
    }
}
