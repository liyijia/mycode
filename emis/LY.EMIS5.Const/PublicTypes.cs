using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Const
{
    public enum PublicTypes
    {
        /// <summary>
        /// 公开
        /// </summary>
        [Description("公开")]
        Public  = 0,
        /// <summary>
        /// 部分公开
        /// </summary>
        [Description("部分公开")]
        Visit = 1,
        /// <summary>
        /// 私有
        /// </summary>
        [Description("私有")]
        Private = 2,
    }
}
