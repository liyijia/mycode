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
        /// 总结
        /// </summary>
        public virtual Decimal Total { get; set; }

        /// <summary>
        /// 总结
        /// </summary>
        public virtual DateTime CreateDate { get; set; }


        /// <summary>
        /// 采购员
        /// </summary>
        public virtual Manager Buyer { get; set; }

        public virtual IList<StorageSupplier> Suppliers { get; set; }

    }
}
