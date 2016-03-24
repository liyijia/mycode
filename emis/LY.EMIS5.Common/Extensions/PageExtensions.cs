using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using NHibernate.Extensions;
using System.Linq.Expressions;
using System;

namespace LY.EMIS5.Common.Extensions
{
    /// <summary>
    /// 分页扩展类
    /// </summary>
    public static class PageExtensions
    {
        /// <summary>
        /// Html的拓展方法,引用javasctipt文件
        /// </summary>
        /// <param name="html"></param>
        /// <param name="scriptName">javascript文件名</param>
        /// <returns>html编码的字符串</returns>
        public static MvcHtmlString Javascript(this HtmlHelper html, string scriptName)
        {
            var path = string.Format("/Scripts/{0}", scriptName);
            return MvcHtmlString.Create(string.Format("<script src='{0}.js' type='text/javascript'></script>", path));
        }

        /// <summary>
        /// Html的拓展方法,引用javasctipt文件
        /// </summary>
        /// <param name="html"></param>
        /// <param name="path">javasctipt文件路径</param>
        /// <param name="scriptName">javasctipt文件名</param>
        /// <returns>html编码的字符串</returns>
        public static MvcHtmlString Javascript(this HtmlHelper html, string path, string scriptName)
        {
            return MvcHtmlString.Create(string.Format("<script src='/Scripts/{0}/{1}.js' type='text/javascript'></script>", path, scriptName));
        }

        /// <summary>
        /// Html的拓展方法,引用css文件
        /// </summary>
        /// <param name="html"></param>
        /// <param name="cssName">样式名</param>
        /// <returns></returns>
        public static MvcHtmlString Css(this HtmlHelper html, string cssName)
        {
            var path = string.Format("/Content/{0}.css", cssName);
            return MvcHtmlString.Create(string.Format("<link href='{0}.css' rel='stylesheet' type='text/css' />", path));
        }

        /// <summary>
        /// Html的拓展方法,引用css文件
        /// </summary>
        /// <param name="html"></param>
        /// <param name="path"></param>
        /// <param name="cssName"></param>
        /// <returns></returns>
        public static MvcHtmlString Css(this HtmlHelper html, string path, string cssName)
        {
            return MvcHtmlString.Create(string.Format("<link href='/Content/{0}/{1}.css' rel='stylesheet' type='text/css'/>", path, cssName));
        }

        /// <summary>
        /// IEnumerable扩展方法，转换为下拉列表项，包含自定义的第一选项
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="items">IEnumerable对象</param>
        /// <param name="strDisplayMember">显示字段</param>
        /// <param name="strValueMember">值字段</param>
        /// <param name="objSelectedValue">选择的值</param>
        /// <param name="strOptionLabel">自定义第一选项，可选</param>
        /// <returns>SelectList</returns>
        public static List<SelectListItem> ToDropDownListItemsIncludeAll<T>(this IEnumerable<T> items, string strDisplayMember, string strValueMember, object objSelectedValue, string strOptionLabel = "所有") where T : IEntityObject
        {
            var listResult = items.ToDropDownListItems(strDisplayMember, strValueMember, objSelectedValue);
            var selectListItem = new SelectListItem { Text = strOptionLabel, Value = "" };
            listResult.Insert(0, selectListItem);
            return listResult;
        }

        /// <summary>
        /// IEnumerable扩展方法，转换为下拉列表项
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="items">IEnumerable对象</param>
        /// <param name="strDisplayMember">显示字段</param>
        /// <param name="strValueMember">值字段</param>
        /// <param name="objSelectedValue">选择的值</param>
        /// <returns>SelectList</returns>
        public static List<SelectListItem> ToDropDownListItems<T>(this IEnumerable<T> items, string strDisplayMember, string strValueMember, object objSelectedValue) where T : IEntityObject
        {
            var listSelect = new SelectList(items, strValueMember, strDisplayMember, objSelectedValue);
            return listSelect.ToList();
        }
    }
}