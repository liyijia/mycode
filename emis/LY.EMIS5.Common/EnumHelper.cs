using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LY.EMIS5.Common
{
    public class EnumHelper<T> where T : struct
    {
        private static Dictionary<T, string> _List = new Dictionary<T, string>();

        public static List<T> Values { get { return _List.Keys.ToList(); } }


        static EnumHelper()
        {
            foreach (var field in typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                var attrs = field.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
                if (attrs == null || attrs.Length == 0)
                    continue;
                _List.Add((T)field.GetValue(null), attrs[0].Description);
            }

        }

        public static string EnumToString(T enumValue)
        {
            return _List.ContainsKey(enumValue) ? _List[enumValue] : null;
        }

        public static T EnumFromString(string enumString)
        {
            if (enumString != null)
            {
                foreach (var keyValue in _List)
                {
                    if (keyValue.Value == enumString)
                        return keyValue.Key;
                }
            }
            return default(T);
        }

        /// <summary>
        /// 装载枚举的值
        /// </summary>
        public static SelectList EnumToSelectList(T? selectedItem = null)
        {
            var list = new Dictionary<object, string>();
            foreach (int i in Enum.GetValues(typeof(T)))
            {
                string name = Enum.GetName(typeof(T), i);

                //取显示名称
                string showName = string.Empty;
                object[] atts = typeof(T).GetField(name).GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (atts.Length > 0) showName = ((DescriptionAttribute)atts[0]).Description;

                list.Add(i, string.IsNullOrEmpty(showName) ? name : showName);
            }
            if (selectedItem != null)
                return new SelectList(list, "Key", "Value", _List.FirstOrDefault(c => c.Value == selectedItem.Value.ToString()));
            else
                return new SelectList(list, "Key", "Value");
        }

        /// <summary>
        /// 获取枚举的描述
        /// </summary>
        /// <param name="obj">枚举</param>
        /// <returns>描述信息</returns>
        public static string GetDescription(Enum obj)
        {
            Contract.Requires(obj != null);

            List<string> list = obj.ToString().Replace(" ", string.Empty).Split(',').ToList();
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
                    result = result + ((DescriptionAttribute)atts[0]).Description + "、";
                }
            }

            return result.Trim('、');
        }
    }
}
