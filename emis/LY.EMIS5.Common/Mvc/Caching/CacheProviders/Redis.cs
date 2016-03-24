using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Caches.Redis;
using ServiceStack.Redis;

namespace LY.EMIS5.Common.Mvc.Caching.CacheProviders
{
    public class Redis : ICacheProvider
    {
        public static readonly Redis Instance = new Redis();

        private PooledRedisClientManager prcm;

        private Redis()
        {
            var redisServerAddress = ConfigurationManager.AppSettings["redisServerAddress"];
            prcm = new PooledRedisClientManager(redisServerAddress);
        }

        public T Get<T>(string key, string region = null)
        {
            using (var redis = prcm.GetClient())
            {
                return redis.Get<T>(region + ":" + key);
            }
        }

        public bool Put<T>(string key, T value, string region)
        {
            return Put(key, value, null, region);
        }

        public bool Put<T>(string key, T value, TimeSpan? validFor = null, string region = null)
        {
            using (var redis = prcm.GetClient())
            {
                if (validFor != null && validFor.HasValue)
                    return redis.Set<T>(region + ":" + key, value, validFor.Value);
                else
                    return redis.Set<T>(region + ":" + key, value);
            }
        }

        public bool Remove(string key, string region = null)
        {
            using (var redis = prcm.GetClient())
            {
                return redis.Remove(key);
            }
        }

        public long PublishMessage(string toChannel, string message)
        {
            using (var redis = prcm.GetClient())
            {
                return redis.PublishMessage(toChannel, message);
            }
        }

        public void Dispose()
        {
            if (prcm != null)
            {
                prcm.Dispose();
                prcm = null;
            }
        }
    }
}
