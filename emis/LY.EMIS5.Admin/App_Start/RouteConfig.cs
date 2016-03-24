using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using LY.EMIS5.Common.Mvc.Constraints;

namespace LY.EMIS5.Admin
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Report",
                url: "Report/{controller}/{action}/{id}",
                defaults: new { controller = "Report", action = "Index", id = UrlParameter.Optional },
                constraints: new { controller = new ReportControllerConstraint("Report", "rpt") },
                namespaces: new string[] { "LY.EMIS5.Admin.Controllers.Report" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "LY.EMIS5.Admin.Controllers" }
            );
        }
    }
}