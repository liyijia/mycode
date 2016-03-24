using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Const
{
    /// <summary>
    /// 考勤对象
    /// </summary>
    public enum AttendanceTargets
    {
        /// <summary>
        /// 走读生
        /// </summary>
        [Description("走读")]
        Day = 1,
        /// <summary>
        /// 住读生
        /// </summary>
        [Description("住读")]
        Resident = 2,
        /// <summary>
        /// 全部
        /// </summary>
        [Description("全部")]
        All = 3
    }
}
