using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LOWEN
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("listProduct", "{Tag}.html", new { controller = "product", action = "listProduct", tag = UrlParameter.Optional }, new { controller = "^P.*", action = "^listProduct$" });
            routes.MapRoute("ListManufacturers", "9/{Tag}/{*catchall}", new { controller = "MenufacturersDisplay", action = "MenufacturerList", tag = UrlParameter.Optional }, new { controller = "^M.*", action = "^MenufacturerList$" });
            routes.MapRoute("DetailManufacturers", "NhaPhanPhoi/{Tag}/{*catchall}", new { controller = "MenufacturersDisplay", action = "MenufacturerDetail", tag = UrlParameter.Optional }, new { controller = "^M.*", action = "^MenufacturerDetail$" });
            routes.MapRoute("ChitietNew", "3/{Tag}/{*catchall}", new { controller = "News", action = "NewsDetail", tag = UrlParameter.Optional }, new { controller = "^N.*", action = "^NewsDetail$" });
 
            routes.MapRoute(name: "Admin", url: "Admin", defaults: new { controller = "Login", action = "LoginIndex" });
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
