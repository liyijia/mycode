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
    public class StorageSupplier : IEntityObject
    {
        /// <summary>
        /// 主键。自增
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 入库单
        /// </summary>
        public virtual Storage Storage { get; set; }

        /// <summary>
        /// 已支付
        /// </summary>
        public virtual Decimal Payment { get; set; }

        /// <summary>
        /// 欠款
        /// </summary>
        public virtual Decimal Debt { get; set; }

        /// <summary>
        /// 是否出具发票
        /// </summary>
        public virtual bool IsInvoice { get; set; }

        public virtual IList<StorageDetail> Details { get; set; }

        /// <summary>
        /// 供应商
        /// </summary>
        public virtual Supplier Supplier { get; set; }

        /// <summary>
        /// 总结
        /// </summary>
        public virtual Decimal Total { get; set; }
    }
}
