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
    /// 入库明细
    /// </summary>
    public class StorageDetail : IEntityObject
    {
        /// <summary>
        /// 主键。自增
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 材料
        /// </summary>
        public virtual Material Material { get; set; }

        /// <summary>
        /// 入库单
        /// </summary>
        public virtual StorageSupplier StorageSupplier { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public virtual int Number { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public virtual Decimal Price { get; set; }

       


    }
}
