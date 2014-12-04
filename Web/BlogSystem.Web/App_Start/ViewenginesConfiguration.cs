namespace BlogSystem.Web.App_Start
{
    using System.Web.Mvc;

    public class ViewenginesConfiguration
    {
        internal static void RegisterViewEngines(ViewEngineCollection viewEngineCollection)
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
        }
    }
}