using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Const
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum MessageKinds
    {
        /// <summary>
        /// 教职工消息
        /// </summary>
        Teacher = 1,
        /// <summary>
        /// 日常消息
        /// </summary>
        Normal = 2,
        /// <summary>
        /// 考勤消息
        /// </summary>
        Attendance = 3,
        /// <summary>
        /// 系统消息
        /// </summary>
        System = 255
    }
}
