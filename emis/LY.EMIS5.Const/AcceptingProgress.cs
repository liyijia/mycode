using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Const
{
    /// <summary>
    /// 受理进度
    /// </summary>
    public enum AcceptingProgress
    {
        /// <summary>
        /// 未受理
        /// </summary>
        [Description("未受理")]
        UnAccepted = 0,

        /// <summary>
        /// 已受理
        /// </summary>
        [Description("已受理")]
        Accepted = 1,

        /// <summary>
        /// 处理中
        /// </summary>
        [Description("处理中")]
        Processing = 2,

        /// <summary>
        /// 已完成
        /// </summary>
        [Description("已完成")]
        Complete = 3
    }
}
