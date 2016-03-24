using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Common.Mvc.Caching
{
    public interface ICacheProvider : IDisposable
    {
        T Get<T>(string key, string region = null);

        bool Put<T>(string key, T value, string region);

        bool Put<T>(string key, T value, TimeSpan? validFor = null, string region = null);

        bool Remove(string key, string region = null);
    }
}
