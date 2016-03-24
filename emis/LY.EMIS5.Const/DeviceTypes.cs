using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Const
{
    public enum DeviceTypes
    {
        /// <summary>
        /// 网页
        /// </summary>
        [Description("网页")]
        Web = 0,

        /// <summary>
        /// 手机短信
        /// </summary>
        Sms = 1,

        /// <summary>
        /// 手机客户端
        /// </summary>
        App = 2
    }
}
