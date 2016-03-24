using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LY.EMIS5.Common.Extensions
{
    /// <summary>
    /// Enum的扩展方法
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// 获取枚举值
        /// </summary>
        /// <param name="obj">枚举</param>
        /// <returns>枚举值</returns>
        public static int ToInt(this Enum obj)
        {
            Contract.Requires(obj != null);

            return Convert.ToInt32(obj);
        }

        /// <summary>
        /// 将枚举转换为Dictionary&lt;int, string&gt; Key为枚举值 Value为枚举名称
        /// </summary>
        /// <param name="obj">枚举</param>
        /// <returns>Dictionary&lt;int, string&gt;</returns>
        public static Dictionary<int, string> EnumAsDictionary(this Enum obj)
        {
            Contract.Requires(obj != null);
            Dictionary<int, string> dic = new Dictionary<int, string>();

            foreach (int item in Enum.GetValues(obj.GetType()))
            {
                dic.Add(item, Enum.GetName(obj.GetType(), item));
            }

            return dic;
        }

        /// <summary>
        /// 将枚举转换为Dictionary&lt;int, string&gt; Key为枚举值 Value为枚举描述或名称
        /// </summary>
        /// <param name="obj">枚举</param>
        /// <param name="isGetDescription">是否获取枚举描述</param>
        /// <returns>Dictionary&lt;int, string&gt;</returns>
        public static Dictionary<int, string> EnumAsDictionary(this Enum obj, bool isGetDescription)
        {
            Contract.Requires(obj != null);
            Type type = obj.GetType();

            if (isGetDescription)
            {
                Dictionary<int, string> dic = new Dictionary<int, string>();

                foreach (int item in Enum.GetValues(type))
                {
                    object[] atts = type.GetField(Enum.GetName(type, item)).GetCustomAttributes(typeof(DescriptionAttribute), false);

                    if (atts != null && atts.Any())
                    {
                        if (!string.IsNullOrWhiteSpace(((DescriptionAttribute)atts[0]).Description))
                        {
                            dic.Add(item, ((DescriptionAttribute)atts[0]).Description);
                        }
                        else
                        {
                            dic.Add(item, Enum.GetName(type, item));
                        }
                    }
                    else
                    {
                        dic.Add(item, Enum.GetName(type, item));
                    }
                }

                return dic;
            }
            else
            {
                return EnumAsDictionary(obj);
            }
        }

        /// <summary>
        /// 获取枚举的描述 如果值有多个则用顿号进行分割
        /// </summary>
        /// <param name="obj">枚举</param>
        /// <returns>描述信息</returns>
        public static string GetDescription(this Enum obj)
        {
            Contract.Requires(obj != null);

            List<string> list = obj.ToString().Replace("\u0020", string.Empty).Split('\u002c').ToList();
            Type type = obj.GetType();
            string result = string.Empty;

            foreach (var item in list)
            {
                string name = Enum.GetName(type, Enum.Parse(type, item));

                if (string.IsNullOrWhiteSpace(name))
                {
                    continue;
                }

                object[] atts = type.GetField(name).GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (atts != null && atts.Length > 0)
                {
                    result = result + ((DescriptionAttribute)atts[0]).Description + "\u3001";
                }
            }

            return result.Trim('\u3001');
        }

        /// <summary>
        /// 获取枚举的描述列表
        /// 注:无描述(Description)的枚举项不会添加进列表中
        /// </summary>
        /// <param name="obj">枚举</param>
        /// <returns>描述信息列表</returns>
        public static List<string> GetDescriptions(this Enum obj)
        {
            Contract.Requires(obj != null);

            List<string> source = obj.ToString().Replace("\u0020", string.Empty).Split('\u002c').ToList();
            Type type = obj.GetType();
            List<string> result = new List<string>();

            foreach (var item in source)
            {
                string name = Enum.GetName(type, Enum.Parse(type, item));

                if (string.IsNullOrWhiteSpace(name))
                {
                    continue;
                }

                object[] atts = type.GetField(name).GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (atts != null && atts.Length > 0)
                {
                    if (!string.IsNullOrWhiteSpace(((DescriptionAttribute)atts[0]).Description))
                    {
                        result.Add(((DescriptionAttribute)atts[0]).Description);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 获取枚举常数名称
        /// </summary>
        /// <param name="obj">枚举</param>
        /// <returns>枚举名称</returns>
        public static string GetName(this Enum obj)
        {
            Contract.Requires(obj != null);

            return Enum.GetName(obj.GetType(), obj);
        }

        /// <summary>
        /// 获取枚举常数名称的数组
        /// </summary>
        /// <param name="obj">枚举</param>
        /// <returns>名称数组</returns>
        public static string[] GetNames(this Enum obj)
        {
            Contract.Requires(obj != null);

            return Enum.GetNames(obj.GetType());
        }
    }
}
