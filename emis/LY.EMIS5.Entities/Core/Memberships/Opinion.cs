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
    /// 意见表
    /// </summary>
    public class Opinion : IEntityObject
    {
        /// <summary>
        /// 主键。自增
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 项目
        /// </summary>
        public virtual Project Project { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateDate { get; set; }

        /// <summary>
        /// 管理员
        /// </summary>
        public virtual Manager Manager { get; set; }

        /// <summary>
        /// 意见
        /// </summary>
        public virtual string Content { get; set; }

        /// <summary>
        /// 进度
        /// </summary>
        public virtual string ProjectProgress { get; set; }

        public virtual bool Done { get; set; }

        public virtual DateTime DoneDate { get; set; }

        public virtual bool Agree { get; set; }

        public virtual string Kind { get; set; }
    }
}
