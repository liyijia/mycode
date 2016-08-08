using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Const
{
    public enum ProjectProgresses
    {
        [Description("未上网")]
        NotOnline = 1,
        [Description("做资料")]
        DoData = 2,
        [Description("打保证金")]
        HitDeposit = 3,
        [Description("等待退款")]
        Arrange = 4,
        //[Description("开标结束")]
        //End = 5,
        [Description("退保证金")]
        RetreatDeposit = 6,
        [Description("经理审核")]
        ManagerAudit = 7,
        [Description("项目结束")]
        Success = 8,
        [Description("不能投标")]
        Cancel = 9
    }
}
