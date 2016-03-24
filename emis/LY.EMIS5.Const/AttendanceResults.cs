using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Const
{
    public enum AttendanceResults
    {
        [Description("迟到")]
        ArriveLate = 1,
        [Description("早退")]
        LeaveEarly = 2,
        [Description("正常")]
        Normal = 3
    }
}
