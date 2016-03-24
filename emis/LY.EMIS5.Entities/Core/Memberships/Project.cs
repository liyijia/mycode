﻿using LY.EMIS5.Common;
using LY.EMIS5.Common.Const;
using NHibernate.Extensions.Data;
using LY.EMIS5.Common.Security;
using LY.EMIS5.Const;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Extensions;
using System.Security.Principal;

namespace LY.EMIS5.Entities.Core.Memberships
{
    /// <summary>
    /// 项目表
    /// </summary>
    public class Project : IEntityObject
    {
        /// <summary>
        /// 主键。自增
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public virtual string ProjectName { get; set; }

        /// <summary>
        /// 下载链接
        /// </summary>
        public virtual string Link { get; set; }

        /// <summary>
        /// 规模
        /// </summary>
        public virtual string Scale { get; set; }

        /// <summary>
        /// 业主
        /// </summary>
        public virtual string Owner { get; set; }

        /// <summary>
        /// 保证金来源   自打/ 代打
        /// </summary>
        public virtual string Source { get; set; }

        /// <summary>
        /// 保证金金额
        /// </summary>
        public virtual Decimal Money { get; set; }

        /// <summary>
        /// 保证金截至时间
        /// </summary>
        public virtual DateTime EndDate { get; set; }

        /// <summary>
        /// 保证金打款方式  网银支付/转账
        /// </summary>
        public virtual string Type { get; set; }

        /// <summary>
        /// 保证金账户名
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// 保证金账号
        /// </summary>
        public virtual string Account { get; set; }

        /// <summary>
        /// 保证金开户行
        /// </summary>
        public virtual string Bank { get; set; }

        /// <summary>
        /// 开标时间
        /// </summary>
        public virtual DateTime OpenDate { get; set; }

        /// <summary>
        /// 业务员
        /// </summary>
        public virtual Manager Sale { get; set; }

        /// <summary>
        /// 业务员意见
        /// </summary>
        public virtual string SalesOpinion { get; set; }

        /// <summary>
        /// 资料员
        /// </summary>
        public virtual Manager Documenter { get; set; }

        /// <summary>
        /// 资料员意见
        /// </summary>
        public virtual string DocumenterOpinion { get; set; }

        /// <summary>
        /// 财务
        /// </summary>
        public virtual Manager Finance { get; set; }

        /// <summary>
        /// 财务意见
        /// </summary>
        public virtual string FinanceOpinion { get; set; }

        /// <summary>
        /// 总经理意见
        /// </summary>
        public virtual Manager CEO { get; set; }

        /// <summary>
        /// 总经理意见
        /// </summary>
        public virtual string CEOOpinion { get; set; }

        /// <summary>
        /// 项目进度  1、未上网2、已上网3、做资料4、打保证金5、开标结束、6、保证金已退，7、不能投标
        /// </summary>
        public virtual string ProjectProgress { get; set; }

        /// <summary>
        /// 项目开标情况
        /// </summary>
        public virtual string Situation { get; set; }

        /// <summary>
        /// 开标人
        /// </summary>
        public virtual Manager OpenPeople { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateDate { get; set; }

        /// <summary>
        /// 资料费
        /// </summary>
        public virtual Decimal MaterialFee { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public virtual string CompanyName { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        public virtual int State { get; set; }

    }
}
