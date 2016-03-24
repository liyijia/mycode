using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Const
{
    public enum ProductStates
    {
        [Description("下架")]
        Wait = 0,
        [Description("上架")]
        Grounding = 1
    }

    public enum OrderStates
    {
        [Description("等待发货")]
        Wait = 0,
        [Description("等待收货")]
        Receive = 1,
        [Description("已完成")]
        Complete = 2,
        [Description("已关闭")]
        Close = 3
    }

}
