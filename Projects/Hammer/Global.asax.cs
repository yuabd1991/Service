using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace XpMain
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			//routes.MapRoute(
			//    "Default", // Route name
			//    "{controller}/{action}/{id}", // URL with parameters
			//    new { controller = "Home", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
			//    new[] { "EasyUI.Controllers" }
			//);
			routes.MapRoute("NoAction", "{controller}.html", new { controller = "Home", action = "index", id = UrlParameter.Optional }, new[] { "EasyUI.Controllers" });//无Action的匹配
			routes.MapRoute("NoID", "{controller}/{action}.html", new { controller = "Home", action = "index", id = UrlParameter.Optional }, new[] { "EasyUI.Controllers" });//无ID的匹配
			routes.MapRoute("Default", "{controller}/{action}/{id}.html", new { controller = "Home", action = "index", id = UrlParameter.Optional }, new[] { "EasyUI.Controllers" });//默认匹配
			routes.MapRoute("Root", "", new { controller = "Home", action = "index", id = UrlParameter.Optional }, new[] { "EasyUI.Controllers" });//根目录匹配
			routes.MapRoute("Admin", "{areas}", new { controller = "Home", action = "index", id = UrlParameter.Optional }, new[] { "EasyUI.Areas.Admin.Controllers" });//根目录匹配
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}