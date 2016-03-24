using Microsoft.ApplicationServer.Caching;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Common.Mvc.Caching.CacheProviders
{
    public class Fabric : ICacheProvider
    {
        private DataCacheFactory CacheFactory = null;
        private DataCache Cache = null;

        public static readonly Fabric Instance = new Fabric();

        private Fabric()
        {
            //DataCacheClientLogManager.ChangeLogLevel(TraceLevel.Off);

            CacheFactory = new DataCacheFactory();
            Cache = CacheFactory.GetCache("Emis5");
        }

        public T Get<T>(string key, string region = null)
        {
            return (T)Cache.Get(key, region);
        }

        public bool Put<T>(string key, T value, string region)
        {
            return Put(key, value, null, region);
        }

        public bool Put<T>(string key, T value, TimeSpan? validFor = null, string region = null)
        {
            try
            {
                if (validFor != null)
                    Cache.Put(key, value, validFor.Value, region);
                else
                    Cache.Put(key, value, region);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Debug.WriteLine(e.StackTrace);

                return false;
            }
        }

        public void Dispose()
        {
            if (CacheFactory != null)
            {
                CacheFactory.Dispose();
                CacheFactory = null;
            }
            if (Cache != null)
                Cache = null;
        }

        public bool Remove(string key, string region = null)
        {
            return Cache.Remove(key, region);
        }
    }
}
