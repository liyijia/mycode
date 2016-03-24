using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Common.Exceptions
{
    public class AlertException : EmisException
    {
        public string ControllerName { get; private set; }
        public string ActionName { get; private set; }
        public object RouteValues { get; private set; }

        public AlertException(int error, string title, string message, string actionName = null, object routeValues = null, Exception innerException = null) :
            this(error, title, message, null, actionName, routeValues, innerException)
        {

        }

        public AlertException(int error, string title, string message, string controllerName, string actionName, object routeValues = null, Exception innerException = null) :
            base(error, title, message, innerException)
        {
            this.ControllerName = controllerName;
            this.ActionName = actionName;
            this.RouteValues = routeValues;
        }
    }
}
