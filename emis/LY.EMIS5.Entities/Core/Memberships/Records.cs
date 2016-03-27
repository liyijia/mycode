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
    /// 备案信息
    /// </summary>
    public class Records : IEntityObject
    {
        /// <summary>
        /// 主键。自增
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        public virtual string Area { get; set; }

        /// <summary>
        /// 是否备案
        /// </summary>
        public virtual bool IsRecord { get; set; }

        /// <summary>
        /// 网址
        /// </summary>
        public virtual string WebSite { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        public virtual string Username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public virtual string Password { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public virtual string Phone { get; set; }

        /// <summary>
        /// 备案情况
        /// </summary>
        public virtual string Situation { get; set; }

        /// <summary>
        /// 备案时间
        /// </summary>
        public virtual DateTime Date { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string Remarks { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateDate { get; set; }

        /// <summary>
        /// 管理员
        /// </summary>
        public virtual Manager Manager { get; set; }
    }
}
