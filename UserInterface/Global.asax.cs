using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;
using UserInterface.App_Start;
using System.Web.Http;

namespace UserInterface
{
    public class MvcApplication : System.Web.HttpApplication
    {
       

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            // WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            MappingConfig.RegisterMaps();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
