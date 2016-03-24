using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Const
{
    public enum BankStates
    {
        [Description("等待复核")]
        Wait = 0,
        [Description("复核成功")]
        Success = 1,
        [Description("复核失败")]
        Failure = 2
    }
}
