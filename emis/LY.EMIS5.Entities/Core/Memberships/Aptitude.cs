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

namespace LY.EMIS5.Entities.Core.Memberships
{
    /// <summary>
    /// 资质表
    /// </summary>
    public class Aptitude : IEntityObject
    {
        /// <summary>
        /// 主键。自增
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 资质名
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public virtual string Level { get; set; }

        /// <summary>
        /// 管理员
        /// </summary>
        public virtual Manager Manager { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateDate { get; set; }

        public virtual string Company { get; set; }
    }
}
