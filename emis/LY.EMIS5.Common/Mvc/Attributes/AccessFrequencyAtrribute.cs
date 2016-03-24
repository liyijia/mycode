using LY.EMIS5.Common.Mvc.Caching;
using LY.EMIS5.Common.Mvc.Caching.CacheProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using LY.EMIS5.Common.Mvc.Attributes;
using LY.EMIS5.Common.Exceptions;

namespace LY.EMIS5.Common.Mvc.Attributes
{
    public class AccessFrequencyAtrribute : ActionFilterAttribute
    {
        private static readonly ICacheProvider Cache = LY.EMIS5.Common.Mvc.Caching.CacheProviders.Couchbase.Instance;
        private static readonly string CacheRegion = "AccessFrequencyFilter";

        /// <summary>
        /// 每分钟的访问频次
        /// </summary>
        public int MaxAccessFrequency { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var lastVisitTimeKey = "LastVisitTime." + filterContext.HttpContext.User.Identity.Name;
                var visitTimesKey = "VisitTimes." + filterContext.HttpContext.User.Identity.Name;

                var lastVisitTime = Cache.Get<DateTime>(lastVisitTimeKey, CacheRegion);
                var visitTimes = Cache.Get<int>(visitTimesKey, CacheRegion);
                if (DateTime.Now - lastVisitTime >= TimeSpan.FromMinutes(1))
                {
                    visitTimes = 0;
                    Cache.Put(lastVisitTimeKey, DateTime.Now, TimeSpan.FromMinutes(1), CacheRegion);
                }
                if (++visitTimes > MaxAccessFrequency)
                  throw new AlertException(0,"操作失败","警告：您访问站点过于频繁，请稍候再试！","Index","Home");
                else
                    Cache.Put(visitTimesKey, visitTimes, TimeSpan.FromMinutes(1), CacheRegion);
            }
        }
    }
}
