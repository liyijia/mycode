using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Common.Const
{
    /// <summary>
    /// 级联
    /// </summary>
    public class CascadeStyle
    {
        /// <summary>
        /// 所有情况下均进行关联操作
        /// </summary>
        public const string All = "all";

        /// <summary>
        /// 所有情况下均不进行关联操作。这是默认值
        /// </summary>
        public const string None = "none";

        /// <summary>
        /// 在执行delete 时进行关联操作，但不删除已经解除关系的子对象
        /// </summary>
        public const string Delete = "delete";

        /// <summary>
        /// 在执行save/update时进行关联操作
        /// </summary>
        public const string SaveUpdate = "save-update";

        /// <summary>
        /// 删除所有和当前对象解除关联的对象
        /// </summary>
        public const string DeleteOrphan = "delete-orphan";

        /// <summary>
        /// 在解除父子关系时,自动删除不属于父对象的子对象, 也支持级联删除和级联保存更新
        /// </summary>
        public const string AllDeleteOrphan = "all-delete-orphan";
    }
}
