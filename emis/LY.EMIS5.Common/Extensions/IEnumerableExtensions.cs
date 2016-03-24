using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using NHibernate.Extensions;

namespace LY.EMIS5.Common.Extensions
{
    public static partial class IEnumerableExtensions
    {
        #region 私有方法

        /// <summary>
        /// 获取容器中只定表达式的值
        /// </summary>
        /// <param name="container">容器</param>
        /// <param name="expression">表达式</param>
        /// <returns></returns>
        private static string Eval(object container, string expression)
        {
            object value = container;
            if (!String.IsNullOrEmpty(expression))
            {
                value = DataBinder.Eval(container, expression);
            }
            return Convert.ToString(value, CultureInfo.CurrentCulture);
        }

        #endregion

        /// <summary>
        /// 将IEnumerable&lt;T&gt;转化为SelectList
        /// </summary>
        /// <typeparam name="TEntity">类型T</typeparam>
        /// <typeparam name="TValue">类型T中的成员类型</typeparam>
        /// <typeparam name="TText">类型T中的成员类型</typeparam>
        /// <param name="items">数据源 不能为null</param>
        /// <param name="valueField">数据值字段 lambda表达式 eg.m=>m.Name</param>
        /// <param name="textField">数据文本字段 lambda表达式 eg.m=>m.Id</param>
        /// <returns></returns>
        public static SelectList AsSelectList<TEntity, TValue, TText>(this IEnumerable<TEntity> items, Expression<Func<TEntity, TValue>> valueField, Expression<Func<TEntity, TText>> textField) // where TEntity : IEntityObject
        {
            Contract.Requires(items != null);

            return new SelectList(items, ExpressionHelper.GetExpressionText(valueField), ExpressionHelper.GetExpressionText(textField));
        }

        /// <summary>
        /// 将IEnumerable&lt;T&gt;转化为SelectList
        /// </summary>
        /// <typeparam name="TEntity">类型T</typeparam>
        /// <typeparam name="TValue">类型T中的成员类型</typeparam>
        /// <typeparam name="TText">类型T中的成员类型</typeparam>
        /// <param name="items">数据源 不能为null</param>
        /// <param name="valueField">数据值字段 lambda表达式 eg.m=>m.Name</param>
        /// <param name="textField">数据文本字段 lambda表达式 eg.m=>m.Id</param>
        /// <param name="selectedValue">默认值</param>
        /// <returns></returns>
        public static SelectList AsSelectList<TEntity, TValue, TText>(this IEnumerable<TEntity> items, Expression<Func<TEntity, TValue>> valueField, Expression<Func<TEntity, TText>> textField, object selectedValue) // where TEntity : IEntityObject
        {
            Contract.Requires(items != null);

            return new SelectList(items, ExpressionHelper.GetExpressionText(valueField), ExpressionHelper.GetExpressionText(textField), selectedValue);
        }

        /// <summary>
        /// 将IEnumerable&lt;T&gt;转化为IList&lt;SelectListItem&gt;
        /// </summary>
        /// <typeparam name="TEntity">类型T</typeparam>
        /// <typeparam name="TValue">类型T中的成员类型</typeparam>
        /// <typeparam name="TText">类型T中的成员类型</typeparam>
        /// <param name="items">数据源 不能为null</param>
        /// <param name="valueField">数据值字段 lambda表达式 eg.m=>m.Name</param>
        /// <param name="textField">数据文本字段 lambda表达式 eg.m=>m.Id</param>
        /// <returns>SelectList列表项集合</returns>
        public static IList<SelectListItem> AsSelectItemList<TEntity, TValue, TText>(this IEnumerable<TEntity> items, Expression<Func<TEntity, TValue>> valueField, Expression<Func<TEntity, TText>> textField)
        {
            Contract.Requires(items != null);
            Contract.Requires(textField != null);
            Contract.Requires(valueField != null);

            var dataTextField = ExpressionHelper.GetExpressionText(textField);
            var dataValueField = ExpressionHelper.GetExpressionText(valueField);

            var selectItemList = from obj in items
                                 select new SelectListItem {
                                     Text = Eval(obj, dataTextField),
                                     Value = Eval(obj, dataValueField),
                                 };

            return selectItemList.ToList();
        }

        /// <summary>
        /// 将IEnumerable&lt;T&gt;转化为IList&lt;SelectListItem&gt;
        /// </summary>
        /// <typeparam name="TEntity">类型T</typeparam>
        /// <typeparam name="TValue">类型T中的成员类型</typeparam>
        /// <typeparam name="TText">类型T中的成员类型</typeparam>
        /// <param name="items">数据源 不能为null</param>
        /// <param name="valueField">数据值字段 lambda表达式 eg.m=>m.Name</param>
        /// <param name="textField">数据文本字段 lambda表达式 eg.m=>m.Id</param>
        /// <param name="selectedValue">默认值</param>
        /// <returns>SelectList列表项集合</returns>
        public static IList<SelectListItem> AsSelectItemList<TEntity, TValue, TText>(this IEnumerable<TEntity> items, Expression<Func<TEntity, TValue>> valueField, Expression<Func<TEntity, TText>> textField, object selectedValue)
        {
            Contract.Requires(items != null);
            Contract.Requires(textField != null);
            Contract.Requires(valueField != null);

            var dataTextField = ExpressionHelper.GetExpressionText(textField);
            var dataValueField = ExpressionHelper.GetExpressionText(valueField);
            
            var selectItemList = from obj in items
                                 select new SelectListItem
                                 {
                                     Text = Eval(obj, dataTextField),
                                     Value = Eval(obj, dataValueField),
                                     Selected = selectedValue == null ? false : selectedValue.Equals(DataBinder.Eval(obj, dataValueField))
                                 };

            return selectItemList.ToList();
        }

        public static string ItemToString<TEntity, TKey>(this IEnumerable<TEntity> items, Expression<Func<TEntity, TKey>> keyField, char splitChar) where TEntity : IEntityObject
        {
            Contract.Requires(keyField != null);

            if (items == null || !items.Any())
            {
                return string.Empty;
            }

            StringBuilder txt = new StringBuilder();
            var dataKeyField = ExpressionHelper.GetExpressionText(keyField);

            foreach (var obj in items)
            {
                txt.AppendFormat("{0}{1}", Eval(obj, dataKeyField), splitChar.ToString());
            }

            
           return txt.ToString().Trim(splitChar);
        }
    }
}
