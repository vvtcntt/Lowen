using LOWEN.Models;
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
            routes.MapRoute("productDetail", "{Tag}.htm", new { controller = "product", action = "productDetail", tag = UrlParameter.Optional }, new { controller = "^P.*", action = "^productDetail$" });
            routes.MapRoute("newsDetail", "news/{Tag}", new { controller = "news", action = "newsDetail", tag = UrlParameter.Optional }, new { controller = "^N.*", action = "^newsDetail$" });
            routes.MapRoute("TagNews", "TagNews/{Tag}", new { controller = "news", action = "TagNews", tag = UrlParameter.Optional }, new { controller = "^N.*", action = "^TagNews$" });
            routes.MapRoute(name: "contact", url: "lien-he", defaults: new { controller = "contact", action = "index" });
            routes.MapRoute(name: "orderBuy", url: "gio-hang", defaults: new { controller = "order", action = "index" });
            routes.MapRoute(name: "tai-ve", url: "tai-ve", defaults: new { controller = "download", action = "listDownload" });
            routes.MapRoute("productTag", "tag/{Tag}", new { controller = "product", action = "productTag", tag = UrlParameter.Optional }, new { controller = "^P.*", action = "^productTag$" });
            routes.MapRoute(name: "Admin", url: "Admin", defaults: new { controller = "Login", action = "LoginIndex" });
            routes.MapRoute(
   name: "CmsRoute",
   url: "{*tag}",
   defaults: new { controller = "news", action = "listNews" },
   constraints: new { tag = new CmsUrlConstraint() }
);
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
