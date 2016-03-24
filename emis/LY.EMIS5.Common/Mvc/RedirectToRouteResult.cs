using LY.EMIS5.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Common.Mvc
{
    public class RedirectToRouteResult : System.Web.Mvc.ActionResult
    {
        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public object RouteValues { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public int Error { get; set; }

        public Exception InnerException { get; set; }

        public RedirectToRouteResult(int error, string title, string message, string controllerName, string actionName, object routeValues = null, Exception innerException = null)
        {
            this.Error = error;
            this.Title = title;
            this.Message = message;
            this.ControllerName = controllerName;
            this.ActionName = actionName;
            this.RouteValues = routeValues;
            this.InnerException = innerException;
        }

        public RedirectToRouteResult(int error, string title, string message, string actionName, object routeValues = null, Exception innerException = null)
            : this(error, title, message, null, actionName, routeValues, innerException)
        {
        }

        public RedirectToRouteResult(string title, string message, string controllerName, string actionName, object routeValues = null, Exception innerException = null)
            : this(100, title, message, controllerName, actionName, routeValues, innerException)
        {
        }

        public RedirectToRouteResult(string title, string message, string actionName, object routeValues = null, Exception innerException = null)
            : this(100, title, message, null, actionName, routeValues, innerException)
        {
        }

        public override void ExecuteResult(System.Web.Mvc.ControllerContext context)
        {
            throw new AlertException(this.Error, this.Title, this.Message, this.ControllerName, this.ActionName, this.RouteValues, this.InnerException);
        }
    }
}
