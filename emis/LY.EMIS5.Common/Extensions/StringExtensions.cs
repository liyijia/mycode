using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LY.EMIS5.Common.Extensions
{
    /// <summary>
    /// String扩展方法
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 清除掉字条串中的空格
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <returns></returns>
        public static string CleanString(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return string.Empty;
            }

            return str.Replace("\u3000", string.Empty).Replace("\u0020", string.Empty);
        }

        /// <summary>
        /// 检查是否是正确的Email地址
        /// </summary>
        public static bool IsEmail(this string obj)
        {
            if (string.IsNullOrWhiteSpace(obj))
            {
                return false;
            }
            return Regex.IsMatch(obj,
               @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" +
               @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");
        }

        /// <summary>
        /// 检查是否是英文
        /// </summary>
        public static bool IsEnglish(this string obj)
        {
            if (string.IsNullOrWhiteSpace(obj))
            {
                return false;
            }
            return Regex.IsMatch(obj, @"^[A-Za-z]+$");
        }

        /// <summary>
        /// 检查是否是中文
        /// </summary>
        public static bool IsChinese(this string obj)
        {
            if (string.IsNullOrWhiteSpace(obj))
            {
                return false;
            }
            return Regex.IsMatch(obj, @"^[\u4e00-\u9fa5]{1,9}$");
        }

        /// <summary>
        /// 检查是否是数字
        /// </summary>
        public static bool IsNumeric(this string obj)
        {
            if (string.IsNullOrWhiteSpace(obj))
            {
                return false;
            }

            return Regex.IsMatch(obj, @"^[+-]?\d*[.]?\d*$");
        }
    }
}
