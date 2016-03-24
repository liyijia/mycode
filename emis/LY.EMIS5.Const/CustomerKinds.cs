using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Const
{
    /// <summary>
    /// 用户类型
    /// </summary>
    public enum CustomerKinds
    {
        /// <summary>
        /// 学生
        /// </summary>
        [Description("学生")]
        Student = 1,

        /// <summary>
        /// 家长
        /// </summary>
        [Description("家长")]
        Parent = 2,

        /// <summary>
        /// 教职工
        /// </summary>
        [Description("教职工")]
        Teacher = 3,

        /// <summary>
        /// 科任教师
        /// </summary>
        [Description("科任教师")]
        SubjectTeacher = 4,

        /// <summary>
        /// 班主任
        /// </summary>
        [Description("班主任")]
        ClazzTeacher = 5,

        /// <summary>
        /// 年级主任
        /// </summary>
        [Description("年级主任")]
        GradeTeacher = 6,

        /// <summary>
        /// 校长
        /// </summary>
        [Description("校长")]
        SchoolMaster = 7,

        /// <summary>
        /// OA用户
        /// </summary>
        [Description("OA用户")]
        OAUser = 8,

        /// <summary>
        /// IC卡管理员
        /// </summary>
        [Description("IC卡管理员")]
        IcCardTeacher = 9,
    }


}
