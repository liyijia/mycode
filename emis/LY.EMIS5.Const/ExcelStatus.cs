using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Const
{
    /// <summary>
    /// 用户类型
    /// </summary>
    public enum ExcelStatus
    {
        /// <summary>
        /// 已发送
        /// </summary>
        [Description("已发送")]
        Sended = 1,

        /// <summary>
        /// 草稿
        /// </summary>
        [Description("草稿")]
        Draft = 2


    }
}
