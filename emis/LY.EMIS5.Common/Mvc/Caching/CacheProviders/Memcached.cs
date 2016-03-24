using Enyim.Caching;
using Enyim.Caching.Memcached;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Common.Mvc.Caching.CacheProviders
{
    public sealed class Memcached : ICacheProvider
    {
        private MemcachedClient Cache = null;
        public static readonly Memcached Instance = new Memcached();

        private Memcached()
        {
            Cache = new MemcachedClient();
        }

        public T Get<T>(string key, string region = null)
        {
            return Cache.Get<T>(region + ":" + key);
        }

        public bool Put<T>(string key, T value, string region)
        {
            return Put(key, value, null, region);
        }

        public bool Put<T>(string key, T value, TimeSpan? validFor = null, string region = null)
        {
            if (validFor != null)
                return Cache.Store(StoreMode.Set, region + ":" + key, value, validFor.Value);
            else
                return Cache.Store(StoreMode.Set, region + ":" + key, value);
        }

        public bool Remove(string key, string region = null)
        {
            return Cache.Remove(region + ":" + key);
        }

        public void Dispose()
        {
            if (Cache != null)
            {
                Cache.Dispose();
                Cache = null;
            }
        }
    }
}
