using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using LOWEN.Models;
namespace LOWEN.Models
{
    public class CmsUrlConstraint : IRouteConstraint
    {
        public bool Match(HttpContextBase httpContext, Route route, string parameterName,  RouteValueDictionary values, RouteDirection routeDirection)
        {
            var db = new LOWENContext();
             if (values[parameterName] != null)
            {
                var tag = values[parameterName].ToString();
                 return db.tblGroupNews.Any(p => p.Tag == tag);
            }
            return false;
        }
    }
}