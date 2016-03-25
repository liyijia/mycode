using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Common.Mvc.Extensions
{
    public static class ControllerExtensions
    {
        public static RedirectToRouteResult RedirectToAction(this System.Web.Mvc.Controller contorller, int error, string title, string message, string controllerName, string actionName, object routeValues = null, Exception innerException = null)
        {
            return new RedirectToRouteResult(error, title, message, controllerName, actionName, routeValues, innerException);
        }


        public static RedirectToRouteResult RedirectToAction(string title, string message, string controllerName, string actionName, object routeValues = null, Exception innerException = null)
        {
            return new RedirectToRouteResult(title, message, actionName, routeValues, innerException);
        }

        public static RedirectToRouteResult RedirectToAction(string title, string message, string actionName, object routeValues = null, Exception innerException = null)
        {
            return new RedirectToRouteResult(100, title, message, actionName, routeValues, innerException);
        }
    }
}
