using System;
using NHibernate.Extensions;

namespace LY.EMIS5.Entities.Core.Memberships
{
    /// <summary>
    /// 业绩表
    /// </summary>
    public class Work : IEntityObject
    {
        /// <summary>
        /// 主键。自增
        /// </summary>
        public virtual int Id { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public virtual string Content { get; set; }

        public virtual DateTime Date { get; set; }

      
        public virtual Manager WorkManager { get; set; }

        /// <summary>
        /// 管理员
        /// </summary>
        public virtual Manager CreateManager { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreateDate { get; set; }

        public virtual int State { get; set; }

    }
}
