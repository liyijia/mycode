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

namespace LY.EMIS5.Entities.Core.Stock
{
    /// <summary>
    /// 供应商
    /// </summary>
    public class Supplier : IEntityObject
    {
        /// <summary>
        /// 主键。自增
        /// </summary>
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public virtual string Pnone { get; set; }

        /// <summary>
        /// 收款账户名
        /// </summary>
        public virtual string Account { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public virtual string AccountNumber { get; set; }

        /// <summary>
        /// 开户银行
        /// </summary>
        public virtual string Bank { get; set; }

        /// <summary>
        /// 是否出具发票
        /// </summary>
        public virtual bool IsInvoice { get; set; }

        /// <summary>
        /// 优先级
        /// </summary>
        public virtual int Grade { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remark { get; set; }

    }
}
