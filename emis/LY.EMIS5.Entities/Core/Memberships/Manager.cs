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
    /// 普通员工
    /// </summary>
    public class Manager : IEntityObject, IIdentity, IPrincipal
    {
        /// <summary>
        /// 主键。自增
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 管理员类型 管理员  业务员  资料员  财务  总经理   开标人
        /// </summary>
        public virtual string Kind { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public virtual string Phone { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public virtual Sex Sex { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public virtual Cipher Password { get; set; }


        /// <summary>
        /// 是否禁用
        /// </summary>
        public virtual bool IsEnabled { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateTime { get; set; }

        public Manager()
        {
            this.CreateTime = DateTime.Now;
        }

        string IIdentity.AuthenticationType
        {
            get { return "Admin"; }
        }

        bool IIdentity.IsAuthenticated
        {
            get { return this.Id > 0; }
        }

        string IIdentity.Name
        {
            get { return this.Id.ToString(); }
        }

        IIdentity IPrincipal.Identity
        {
            get { return this; }
        }

        bool IPrincipal.IsInRole(string role)
        {
            if (string.IsNullOrWhiteSpace(role))
                return true;
            var roles = role.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            return roles.Contains(this.Kind);
        }
    }
}
