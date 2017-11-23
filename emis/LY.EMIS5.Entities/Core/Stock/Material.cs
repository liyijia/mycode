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
    /// 材料表
    /// </summary>
    public class Material : IEntityObject
    {
        /// <summary>
        /// 主键。自增
        /// </summary>
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public virtual string Type { get; set; }

        /// <summary>
        /// 规格/型号
        /// </summary>
        public virtual string Spec { get; set; }

        /// <summary>
        /// 品牌
        /// </summary>
        public virtual string Brand { get; set; }

        /// <summary>
        /// 生产厂家
        /// </summary>
        public virtual string Manufactor { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Rmark { get; set; }

        /// <summary>
        /// 库存数量
        /// </summary>
        public virtual int Stock { get; set; }

        /// <summary>
        /// 最新入库时间
        /// </summary>
        public virtual DateTime InDate { get; set; }

    }
}
