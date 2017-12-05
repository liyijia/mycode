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
    /// 采购申请
    /// </summary>
    public class Purchase : IEntityObject
    {
        /// <summary>
        /// 主键。自增
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 申请人
        /// </summary>
        public virtual Manager Creator { get; set; }

        /// <summary>
        /// 最新入库时间
        /// </summary>
        public virtual DateTime CreateDate { get; set; }

        /// <summary>
        /// 申请内容
        /// </summary>
        public virtual IList<PurchaseMaterial> Materials { get; set; }

        /// <summary>
        /// 项目
        /// </summary>
        public virtual Dictionary Dictionary { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }

        /// <summary>
        /// 状态 0待审核  1待受理  2已受理 3已完成  99未通过
        /// </summary>
        public virtual int Status { get; set; }

        /// <summary>
        /// 项目经理
        /// </summary>
        public virtual Manager Manager { get; set; }

        /// <summary>
        /// 项目经理内容
        /// </summary>
        public virtual string ManagerContent { get; set; }

        /// <summary>
        /// 项目经理时间
        /// </summary>
        public virtual DateTime ManagerDate { get; set; }

        /// <summary>
        /// 项目经理
        /// </summary>
        public virtual Manager Buyer { get; set; }

        /// <summary>
        /// 项目经理内容
        /// </summary>
        public virtual string BuyerContent { get; set; }

        /// <summary>
        /// 项目经理时间
        /// </summary>
        public virtual DateTime BuyerDate { get; set; }

        /// <summary>
        /// 项目经理时间
        /// </summary>
        public virtual DateTime AcceptDate { get; set; }

    }
}
