using System;
using NHibernate.Extensions;
using LY.EMIS5.Entities.Core.Memberships;

namespace LY.EMIS5.Entities.Core.Stock
{
    /// <summary>
    /// 出库单
    /// </summary>
    public class Placing : IEntityObject
    {
        /// <summary>
        /// 主键。自增
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 出库单单号
        /// </summary>
        public virtual string No { get; set; }

        /// <summary>
        /// 开单人
        /// </summary>
        public virtual Manager Creator { get; set; }

        /// <summary>
        /// 开单时间
        /// </summary>
        public virtual DateTime CreateDate { get; set; }

        /// <summary>
        /// 领取人
        /// </summary>
        public virtual Manager Receiptor { get; set; }

        /// <summary>
        /// 班组
        /// </summary>
        public virtual string Group { get; set; }

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

    }
}
