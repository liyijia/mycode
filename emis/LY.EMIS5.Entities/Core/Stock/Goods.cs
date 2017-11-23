using System;
using NHibernate.Extensions;

namespace LY.EMIS5.Entities.Core.Stock
{
    /// <summary>
    /// 商品
    /// </summary>
    public class Goods : IEntityObject
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
        /// 入库时间
        /// </summary>
        public virtual DateTime InDate { get; set; }

        /// <summary>
        /// 出库时间
        /// </summary>
        public virtual DateTime OutDate { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public virtual Decimal Price { get; set; }

        /// <summary>
        /// 状态 0未出库  1已出库
        /// </summary>
        public virtual int Status { get; set; }

        /// <summary>
        /// 入库单
        /// </summary>
        public virtual Storage Storage { get; set; }

        /// <summary>
        /// 出库单
        /// </summary>
        public virtual Placing Placing { get; set; }

    }
}
