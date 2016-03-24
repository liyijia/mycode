using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Const
{
    /// <summary>
    /// 手机卡类型
    /// </summary>
    public enum PhoneType
    {
        /// <summary>
        /// 未知
        /// </summary>
        [Description("未知")]
        Unknown = 0,
        /// <summary>
        /// 移动
        /// </summary>
        [Description("移动")]
        CMPP = 1,
        /// <summary>
        /// 联通
        /// </summary>
        [Description("联通")]
        SGIP = 2,
        /// <summary>
        /// 电信
        /// </summary>
        [Description("电信")]
        SMGP = 3
    }
}
