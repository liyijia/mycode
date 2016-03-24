using RazorEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Routing;

namespace LY.EMIS5.Common.Mvc.Caching
{
    public class DbOutputCache
    {
        private class CachedAction
        {
            public string Key { get; set; }
            public string Controller { get; set; }
            public string Action { get; set; }
            public Dictionary<string, string> Parameters { get; set; }
        }

        private static readonly object _Lock = new object();

        private static Dictionary<string, CachedAction> _CachedActions = new Dictionary<string, CachedAction>();

        public ICacheProvider Provider { get; private set; }

        public string Region { get; private set; }

        /// <summary>
        /// 创建页面缓存对象
        /// </summary>
        /// <param name="provider">Fabric、Memcached或WebCache之一</param>
        public DbOutputCache(ICacheProvider provider, string region = null)
        {
            Provider = provider;
            Region = region;
        }

        public T Get<T>(string key)
        {
            return Provider.Get<T>(key, Region);
        }

        public bool Put(string key, object value, TimeSpan? validFor)
        {
            return Provider.Put(key, value, validFor, Region);
        }

        internal static string GetCacheKey(ControllerContext context, params string[] parameters)
        {
            var controller = context.RouteData.Values["controller"].ToString().Trim();
            var action = context.RouteData.Values["action"].ToString().Trim();
            var cacheKey = controller + "\\" + action + "\\";
            if (parameters != null && parameters.Length > 0)
            {
                foreach (var param in parameters)
                {
                    cacheKey += param + ":" + (HttpContext.Current.ApplicationInstance.GetVaryByCustomString(HttpContext.Current, param) ?? (context.RouteData.Values.ContainsKey(param) ? context.RouteData.Values[param] : context.HttpContext.Request.Params[param])) + ";";
                }
            }

            return cacheKey;
        }

        private static string GetParameterValueFromContext(ControllerContext context, string parameterName)
        {
            return HttpContext.Current.ApplicationInstance.GetVaryByCustomString(HttpContext.Current, parameterName) ?? (context.RouteData.Values.ContainsKey(parameterName) ? context.RouteData.Values[parameterName].ToString() : "");
        }

        public void EnableActionForNotification(ControllerContext context, params string[] parameters)
        {
            var controller = context.RouteData.Values["controller"].ToString().Trim();
            var action = context.RouteData.Values["action"].ToString().Trim();
            var cacheKey = GetCacheKey(context, parameters);

            CachedAction cache = null;
            lock (_Lock)
            {
                if (!_CachedActions.ContainsKey(cacheKey))
                {
                    cache = new CachedAction
                    {
                        Key = cacheKey,
                        Controller = controller,
                        Action = action,
                        Parameters = new Dictionary<string, string>()
                    };
                    _CachedActions.Add(cache.Key, cache);
                }
            }
            if (cache != null && parameters != null && parameters.Length > 0)
            {
                foreach (var parameterName in parameters)
                {
                    cache.Parameters.Add(parameterName.Trim().ToLower(), GetParameterValueFromContext(context, parameterName));
                }
            }
        }

        public void NotifyDependencyChanged(ControllerContext context, string controller, string action, params string[] parameters)
        {
            var constraint = new RouteValueDictionary();
            if (parameters != null && parameters.Length > 0)
            {
                foreach (var parameterName in parameters)
                {
                    var parameterValue = GetParameterValueFromContext(context, parameterName);
                    constraint.Add(parameterName, parameterValue);
                }
            }
            NotifyDependencyChanged(context, controller, action, constraint);
        }

        public void NotifyDependencyChanged(ControllerContext context, string controller, string action, object constraint)
        {
            NotifyDependencyChanged(context, controller, action, constraint == null ? null : new RouteValueDictionary(constraint));
        }

        public void NotifyDependencyChanged(ControllerContext context, string controller, string action, IDictionary<string, object> constraint)
        {
            var query = _CachedActions.Values.Where(c => c.Controller.Equals(controller, StringComparison.CurrentCultureIgnoreCase) && c.Action.Equals(action, StringComparison.CurrentCultureIgnoreCase));
            if (constraint != null)
            {
                foreach (var parameter in constraint)
                {
                    var parameterKey = parameter.Key.Trim().ToLower();
                    var parameterValue = parameter.Value.ToString().Trim().ToLower();
                    if (string.IsNullOrWhiteSpace(parameterValue))
                        parameterValue = GetParameterValueFromContext(context, parameter.Key);
                    query = query.Where(c => c.Parameters.ContainsKey(parameterKey) && c.Parameters[parameterKey].Equals(parameterValue, StringComparison.CurrentCultureIgnoreCase));
                }
            }
            foreach (var cache in query)
            {
                Provider.Remove(cache.Key, Region);
            }
        }
    }
}
