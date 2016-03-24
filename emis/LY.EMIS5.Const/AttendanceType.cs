using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Const
{
    /// <summary>
    /// 考勤类别
    /// </summary>
    public enum AttendanceType
    {

        /// <summary>
        /// 出
        /// </summary>
        [Description("出")]
        Out,

        /// <summary>
        /// 入
        /// </summary>
        [Description("入")]
        In,

        /// <summary>
        /// 进出
        /// </summary>
        [Description("进出")]
        All
    }

    /// <summary>
    /// 终端状态
    /// </summary>
    public enum ClientState
    {

        /// <summary>
        /// 未初始化
        /// </summary>
        [Description("未初始化")]
        Initialize,

        /// <summary>
        /// 在线
        /// </summary>
        [Description("在线")]
        Online,
        /// <summary>
        /// 离线
        /// </summary>
        [Description("离线")]
        Offline,
    }
}
