using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Const
{ 
    /// <summary>
    /// 管理员类型
    /// </summary>
    [Flags]
    public enum ManagerKinds
    {
        /// <summary>
        /// 普通员工
        /// </summary>
        [Description("普通员工")]
        Employee = 0,

        /// <summary>
        /// 系统管理员
        /// </summary>
        [Description("系统管理员")]
        SystemManager = 8,
    }
}
