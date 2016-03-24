using Enyim.Caching;
using LY.EMIS5.Common.Mvc.Caching.CacheProviders;
using Microsoft.ApplicationServer.Caching;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.UI;

namespace LY.EMIS5.Common.Mvc.Caching
{
    class CachedPage
    {
        public string Source { get; set; }
        public DateTime LastModified { get; set; }
    }

    public class DbOutputCacheAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 获取或设置基于参数变化的值
        /// </summary>
        public string VaryByParam { get; set; }

        /// <summary>
        /// 不缓存页面
        /// </summary>
        public bool NoCache { get; set; }

        /// <summary>
        /// 需要级联更新的页面。格式：key=value。
        /// 如果属性的值为空字符串，其值会从上下文中自动获取；否则值为设定的常量；
        /// 例如：
        /// controller=home,action=index,id=,kind=2;controller=home,action=login;
        /// </summary>
        public string Cascades { get; set; }

        /// <summary>
        /// 要将 Cache-Control: max-age 标头设置为的时间跨度（单位：分钟）。在该时间跨度内，浏览器将不会向服务器发起请求
        /// 对于静态页面，可设置为较长的时间；对于动态页面，应设置合适的值；对于需要随时获取最新内容的页面，请不要设置该值
        /// </summary>
        public int MaxAge { get; set; }

        /// <summary>
        /// Ajax请求标志
        /// </summary>
        public string AjaxRequestIdentifier { get; set; }

        private string cacheKey;
        private CachedPage cachedValue;
        private string[] parameters;
        private TextWriter originalWriter;
        private StringWriter cachingWriter;

        public DbOutputCacheAttribute(string varyByParam = null, bool noCache = false, string ajaxRequestIdentifier = null)
        {
            this.VaryByParam = varyByParam;
            this.NoCache = noCache;
            this.AjaxRequestIdentifier = ajaxRequestIdentifier;
        }

        /// <summary>
        /// 这里可以很容易的更改Cache的提供者，比如：Memcached、Fabric等
        /// </summary>
#if DEBUG
        private DbOutputCache Cache = new DbOutputCache(WebCache.Instance, "LY.EMIS5.Web");
#else
        private DbOutputCache Cache = new DbOutputCache(Memcached.Instance, "LY.EMIS5.Web");
#endif

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentNullException("filterContext");
            if (!string.IsNullOrWhiteSpace(VaryByParam))
                parameters = VaryByParam.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).OrderBy(c => c).ToArray();
            var request = filterContext.HttpContext.Request;
            var response = filterContext.HttpContext.Response;
            cacheKey = DbOutputCache.GetCacheKey(filterContext, parameters);
            if (!this.NoCache)
            {
                cachedValue = Cache.Get<CachedPage>(cacheKey);
                if (cachedValue != null)
                {
                    if (request.IsAjaxRequest() && !string.IsNullOrWhiteSpace(AjaxRequestIdentifier))
                    {
                        var source = new Regex("(?<=^{[^{]*\"" + AjaxRequestIdentifier + "\":\")(\\d*)(?=\",)").Replace(cachedValue.Source, request.Params[AjaxRequestIdentifier], 1, 0);
                        filterContext.Result = new ContentResult() { Content = source };
                    }
                    else
                    {
                        response.Cache.SetETag(cachedValue.LastModified.Ticks.ToString());
                        response.Cache.SetLastModified(cachedValue.LastModified);
                        response.Cache.SetCacheability(HttpCacheability.Private);
                        response.Cache.SetMaxAge(TimeSpan.FromMinutes(this.MaxAge));
                        response.Cache.SetSlidingExpiration(true);
                        response.Headers["ETag"] = cachedValue.LastModified.Ticks.ToString();
                        if (request.Headers["If-None-Match"] != null && request.Headers["If-None-Match"] == cachedValue.LastModified.Ticks.ToString())
                        {
                            response.StatusCode = 304;
                            filterContext.Result = new ContentResult();
                        }
                        else
                            filterContext.Result = new ContentResult() { Content = cachedValue.Source };
                    }
                }
                else
                {
                    cachingWriter = new StringWriter(CultureInfo.InvariantCulture);
                    originalWriter = filterContext.HttpContext.Response.Output;
                    filterContext.HttpContext.Response.Output = cachingWriter;
                }
            }
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentNullException("filterContext");

            if (filterContext.Exception == null) //出现异常的页面不缓存，正常的页面才需要缓存
            {
                if (cachedValue == null && !this.NoCache && filterContext.HttpContext.Response.ContentType.Trim().StartsWith("text/", StringComparison.CurrentCultureIgnoreCase))
                {
                    string capturedText = cachingWriter.ToString();
                    filterContext.HttpContext.Response.Output = originalWriter;
                    Cache.EnableActionForNotification(filterContext, parameters);
                    Cache.Put(cacheKey, new CachedPage { Source = capturedText, LastModified = DateTime.Now }, new TimeSpan(0, 20, 0));
                    filterContext.Result.ExecuteResult(filterContext);
                }
                //更新当前页面的缓存
                if (this.NoCache)
                {
                    var controller = filterContext.RouteData.Values["controller"].ToString().Trim();
                    var action = filterContext.RouteData.Values["action"].ToString().Trim();
                    Cache.NotifyDependencyChanged(filterContext, controller, action, parameters);
                }
                //级联更新其他页面的缓存
                if (!string.IsNullOrWhiteSpace(this.Cascades))
                {
                    var cascades = this.Cascades.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    if (cascades != null && cascades.Length > 0)
                    {
                        foreach (DbOutputCacheCascade cascade in cascades)
                        {
                            Cache.NotifyDependencyChanged(filterContext, cascade.Controller, cascade.Action, cascade.Constraint);
                        }
                    }
                }
            }
        }
    }
}
