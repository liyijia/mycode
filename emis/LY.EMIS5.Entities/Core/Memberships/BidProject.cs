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
    /// 项目表
    /// </summary>
    public class BidProject : IEntityObject
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
        /// 项目类型
        /// </summary>
        public virtual string ProjectType { get; set; }

        /// <summary>
        /// 规模
        /// </summary>
        public virtual string Scale { get; set; }

        /// <summary>
        /// 中标价格(万元)
        /// </summary>
        public virtual Decimal Money { get; set; }

        /// <summary>
        /// 中标时间
        /// </summary>
        public virtual DateTime BidDate { get; set; }

        /// <summary>
        /// 项目所在地
        /// </summary>
        public virtual string Address { get; set; }

        /// <summary>
        /// 项目实施人
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public virtual string TeletePhone { get; set; }

        /// <summary>
        /// 计划开工时间
        /// </summary>
        public virtual DateTime OpenDate { get; set; }

        /// <summary>
        /// 计划竣工时间
        /// </summary>
        public virtual DateTime EndDate { get; set; }

        /// <summary>
        /// 进场时间
        /// </summary>
        public virtual DateTime InDate { get; set; }


        /// <summary>
        /// 工期
        /// </summary>
        public virtual string TimeLimit { get; set; }

        /// <summary>
        /// 项目经理
        /// </summary>
        public virtual string ProjectManager { get; set; }

        /// <summary>
        /// 班组成员
        /// </summary>

        public virtual string Members { get; set; }

        /// <summary>
        /// 项目进度
        /// </summary>
        public virtual BidProjectProgresses ProjectProgress { get;set;}

        /// <summary>
        /// 项目部递交分公司资料
        /// </summary>
        public virtual string Data { get; set; }

        /// <summary>
        /// 项目部递交分公司资料
        /// </summary>
        public virtual string Company { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateDate { get; set; }

        /// <summary>
        /// 项目部配备资料
        /// </summary>
        public virtual string ProjectData { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public virtual Manager CreateManager { get; set; }


    }
}
