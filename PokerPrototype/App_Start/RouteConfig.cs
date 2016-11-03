using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PokerPrototype
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "logout",
                url: "logout",
                defaults: new { controller = "Home", action = "Logout" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Index"}
          
            );
            routes.MapRoute(
                name: "PageNotFound",
                url: "{*url}",
                defaults: new { controller = "Home", action = "PageNotFound" }
            );
        }
    }
}
