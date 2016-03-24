using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace LY.EMIS5.Common.Mvc.Constraints
{
    public class ReportControllerConstraint : IRouteConstraint
    {
        private string[] _values;

        public ReportControllerConstraint(params string[] values)
        {
            List<string> tmpValues = new List<string>();
            foreach (var str in values)
            {
                tmpValues.Add(str.ToUpper());
            }

            this._values = tmpValues.ToArray();
        }

        bool IRouteConstraint.Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
        {
            string value = values[parameterName].ToString();

            return _values.Where(m => value.ToUpper().EndsWith(m)).Any();
        }
    }
}
