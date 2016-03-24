using LY.EMIS5.Common.Mvc.Caching;
using LY.EMIS5.Common.Mvc.Caching.CacheProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Common.Security
{
    /// <summary>
    /// 安全沙箱，防止暴力破解
    /// </summary>
    public class AuthenticationSandBox
    {
        private static readonly string CacheKeyFormat = "UserName:{0},IP:{1}";
        private string CacheRegion;
        private ICacheProvider Cache = WebCache.Instance;

        public string Name { get; private set; }
        public int MaxAuthenticateFailedTimes { get; private set; }
        public int FreezingMinute { get; private set; }

        /// <summary>
        /// 创建安全沙箱的实例
        /// </summary>
        /// <param name="name">给沙箱取个名字，用以区分多个沙箱</param>
        /// <param name="cache">保存用户登录信息的缓存对象</param>
        /// <param name="maxAuthenticateFailedTimes">最大连续登录失败次数</param>
        /// <param name="freezingMinute">达到最大连续登录失败次数后账户被冻结的分钟数</param>
        public AuthenticationSandBox(string name, int maxAuthenticateFailedTimes = 5, int freezingMinute = 20)
        {
            this.Name = name;
            this.CacheRegion = this.GetType().Name + "." + name;
            this.MaxAuthenticateFailedTimes = maxAuthenticateFailedTimes;
            this.FreezingMinute = freezingMinute;
        }

        /// <summary>
        /// 登录开始时调用
        /// </summary>
        /// <param name="userName">登录用户名</param>
        /// <param name="userIP">登录者的IP</param>
        public virtual void BeforeAuthenticate(string userName, string userIP)
        {
            var cacheKey = string.Format(CacheKeyFormat, userName, userIP);
            var failedTimes = Cache.Get<int>(cacheKey, CacheRegion);
            if (failedTimes >= MaxAuthenticateFailedTimes)
                throw new SecurityException(string.Format("您已经连续{0}次登录失败，您的账号已被冻结，{1}分钟后您可以尝试重新登录", MaxAuthenticateFailedTimes, FreezingMinute));
        }

        /// <summary>
        /// 登录失败时调用
        /// </summary>
        /// <param name="userName">登录用户名</param>
        /// <param name="userIP">登录者的IP</param>
        public virtual int AfterAuthenticate(string userName, string userIP, bool failed)
        {
            var cacheKey = string.Format(CacheKeyFormat, userName, userIP);
            if (failed)
            {
                var failedTimes = Cache.Get<int>(cacheKey, CacheRegion);
                Cache.Put(cacheKey, ++failedTimes, TimeSpan.FromMinutes(FreezingMinute), CacheRegion);
                return MaxAuthenticateFailedTimes - failedTimes;
            }
            else
            {
                Cache.Remove(cacheKey, CacheRegion);
                return MaxAuthenticateFailedTimes;
            }
        }
    }
}
