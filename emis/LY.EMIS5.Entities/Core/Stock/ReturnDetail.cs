using System;
using NHibernate.Extensions;

namespace LY.EMIS5.Entities.Core.Stock
{
    /// <summary>
    /// 退货明细
    /// </summary>
    public class ReturnDetail : IEntityObject
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
        /// 数量
        /// </summary>
        public virtual int Number { get; set; }

        /// <summary>
        /// 退货单
        /// </summary>
        public virtual int ReturnGoodsId { get; set; }

    }
}
