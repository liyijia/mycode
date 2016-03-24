using RazorEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace LY.EMIS5.Common.Mvc.Caching
{
    /// <summary>
    /// 需要级联更新缓存的页面
    /// </summary>
    internal class DbOutputCacheCascade
    {
        /// <summary>
        /// 控制器名称
        /// </summary>
        public string Controller { get; set; }

        /// <summary>
        /// 动作名称
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// 对动作的约束
        /// </summary>
        public IDictionary<string, object> Constraint { get; set; }

        public static implicit operator string(DbOutputCacheCascade cascade)
        {
            if (cascade == null)
                return null;
            if (cascade.Constraint == null)
                throw new ArgumentNullException("Constraint");

            var stringBuilder = new StringBuilder();
            foreach (var parameter in new RouteValueDictionary(cascade.Constraint))
            {
                stringBuilder.AppendFormat("{0}={1},", parameter.Key, parameter.Value);
            }
            return stringBuilder.ToString();
        }

        public static implicit operator DbOutputCacheCascade(string cascade)
        {
            if (cascade == null)
                return null;

            var retVal = new DbOutputCacheCascade() { Constraint = new RouteValueDictionary() };
            foreach (var constraint in cascade.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var temp = constraint.Split('=');
                if (temp.Length != 2)
                    throw new FormatException("cascade的字符串格式不正确");
                var parameterName = temp[0].Trim().ToLower();
                var parameterValue = temp[1].Trim().ToLower();
                if (parameterName == "controller")
                    retVal.Controller = parameterValue;
                else if (parameterName == "action")
                    retVal.Action = parameterValue;
                else
                    retVal.Constraint.Add(parameterName, parameterValue);
            }
            if (string.IsNullOrWhiteSpace(retVal.Controller) || string.IsNullOrWhiteSpace(retVal.Action))
                throw new FormatException("cascade的字符串格式不正确");
            return retVal;
        }
    }
}
