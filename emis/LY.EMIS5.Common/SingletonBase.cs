using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Common
{
    public class SingletonBase<T> where T : new()
    {
        private static readonly Lazy<T> _instance = new Lazy<T>(() => new T());

        public static T Instance
        {
            get { return _instance.Value; }
        }
    }
}
