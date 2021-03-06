﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Const
{
    public enum ProjectProgresses
    {
        [Description("登记项目")]
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
        [Description("财务审核")]
        FinanceAudit = 10,
        [Description("项目结束")]
        Success = 8,
        [Description("不能投标")]
        Cancel = 9
        
    }

    public enum BidProjectProgresses
    {
        [Description("中标公示")]
        Public = 1,
        [Description("合同准备")]
        Ready = 2,
        [Description("进场施工")]
        In = 3,
            [Description("项目完工")]
        Over = 4,
        [Description("质量保证金退回")]
        Retreat = 5
    }
}
