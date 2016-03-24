using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Const
{
    public enum FeeTypes
    {
        /// <summary>
        /// 激活
        /// </summary>
        [Description("激活")]
        Active = 0,
        
        /// <summary>
        /// 试用
        /// </summary>
        [Description("试用")]
        Trial = 1,

        /// <summary>
        /// 免费
        /// </summary>
        [Description("免费")]
        Free = 2,
        
        /// <summary>
        /// 收费
        /// </summary>
        [Description("收费")]
        Fee = 3,

        /// <summary>
        /// 过期
        /// </summary>
        [Description("过期")]
        Overdue = 4
    }
}
