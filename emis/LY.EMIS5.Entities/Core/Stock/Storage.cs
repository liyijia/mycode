using LY.EMIS5.Common;
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
using LY.EMIS5.Entities.Core.Memberships;

namespace LY.EMIS5.Entities.Core.Stock
{
    /// <summary>
    /// 入库单
    /// </summary>
    public class Storage : IEntityObject
    {
        /// <summary>
        /// 主键。自增
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 入库单单号
        /// </summary>
        public virtual string No { get; set; }

        /// <summary>
        /// 材料类型
        /// </summary>
        public virtual string Type { get; set; }

        /// <summary>
        /// 材料种类
        /// </summary>
        public virtual string Kind { get; set; }

        /// <summary>
        /// 总结
        /// </summary>
        public virtual Decimal Total { get; set; }

        /// <summary>
        /// 已支付
        /// </summary>
        public virtual Decimal Payment { get; set; }

        /// <summary>
        /// 欠款
        /// </summary>
        public virtual Decimal Debt { get; set; }

        /// <summary>
        /// 开单人
        /// </summary>
        public virtual Manager Creator { get; set; }

        /// <summary>
        /// 开单时间
        /// </summary>
        public virtual DateTime CreateDate { get; set; }

        /// <summary>
        /// 采购员
        /// </summary>
        public virtual Manager Buyer { get; set; }

        /// <summary>
        /// 是否出具发票
        /// </summary>
        public virtual bool IsInvoice { get; set; }

        /// <summary>
        /// 审批人
        /// </summary>
        public virtual Manager Auditor { get; set; }

        /// <summary>
        /// 审批时间
        /// </summary>
        public virtual DateTime AuditDate { get; set; }

        /// <summary>
        /// 状态  0审批中 1已通过  2未通过
        /// </summary>
        public virtual int Status { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public virtual Supplier Supplier { get; set; }

    }
}
