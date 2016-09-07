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
    /// 业绩表
    /// </summary>
    public class Achievement : IEntityObject
    {
        /// <summary>
        /// 主键。自增
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public virtual string ProjectName { get; set; }

        /// <summary>
        /// 规模
        /// </summary>
        public virtual string Scale { get; set; }

        /// <summary>
        /// 开工时间
        /// </summary>
        public virtual DateTime StartDate { get; set; }

        /// <summary>
        /// 竣工时间
        /// </summary>
        public virtual DateTime EndDate { get; set; }

        /// <summary>
        /// 项目经理
        /// </summary>
        public virtual string ProjectManager { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public virtual string Type { get; set; }

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
