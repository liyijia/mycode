using Couchbase;
using Enyim.Caching.Memcached;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace LY.EMIS5.Common.Mvc.Caching.CacheProviders
{
    public sealed class Couchbase : ICacheProvider
    {
        private static readonly Regex regexRemoveEmptyChars = new Regex(@"\s");
        public static readonly Couchbase Instance = new Couchbase();

        private Couchbase()
        {
        }

        public T Get<T>(string key, string region = null)
        {
            using (var client = new CouchbaseClient())
            {
                key = regexRemoveEmptyChars.Replace(key, "");
                return client.Get<T>(region + ":" + key);
            }
        }

        public bool Put<T>(string key, T value, string region )
        {
            key = regexRemoveEmptyChars.Replace(key, "");
            return Put(key, value, null, region);
        }

        public bool Put<T>(string key, T value, TimeSpan? validFor = null, string region = null)
        {
            key = regexRemoveEmptyChars.Replace(key, "");
            using (var client = new CouchbaseClient())
            {
                if (validFor != null)
                    return client.Store(StoreMode.Set, region + ":" + key, value, validFor.Value);
                else
                    return client.Store(StoreMode.Set, region + ":" + key, value);
            }
        }

        public bool Remove(string key, string region = null)
        {
            key = regexRemoveEmptyChars.Replace(key, "");
            using (var client = new CouchbaseClient())
            {
                return client.Remove(region + ":" + key);
            }
        }

        public void Dispose()
        {
        }
    }
}
