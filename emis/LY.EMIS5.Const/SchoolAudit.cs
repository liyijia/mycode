using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Const
{
    /// <summary>
    /// 学校审核状态
    /// </summary>
    public enum SchoolAudit
    {
        /// <summary>
        /// 不审核
        /// </summary>
        NoAudit = 0,

        /// <summary>
        /// 学校审核
        /// </summary>
        SchoolAudit = 1,

        /// <summary>
        /// 后台审核
        /// </summary>
        AdminAudit = 2
    }
}
