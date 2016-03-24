using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace LY.EMIS5.Common.Mvc.Caching.CacheProviders
{
    public class WebCache : ICacheProvider
    {
        private readonly Cache Cache = HttpContext.Current.Cache;
        public static readonly WebCache Instance = new WebCache();

        private WebCache()
        {
        }

        public T Get<T>(string key, string region = null)
        {
            var cachedValue = Cache.Get(region + ":" + key);
            if (cachedValue != null)
                return (T)cachedValue;
            return default(T);
        }

        public bool Put<T>(string key, T value, string region)
        {
            return Put(key, value, null, region);
        }

        public bool Put<T>(string key, T value, TimeSpan? validFor = null, string region = null)
        {
            Cache.Remove(region + ":" + key);
            Cache.Add(region + ":" + key, value, null, Cache.NoAbsoluteExpiration, validFor ?? Cache.NoSlidingExpiration, CacheItemPriority.Default, null);

            return true;
        }

        public bool Remove(string key, string region = null)
        {
            Cache.Remove(region + ":" + key);

            return true;
        }

        public void Dispose()
        {

        }
    }
}
