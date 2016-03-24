using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LY.EMIS5.Common.Mvc;
using NHibernate.Extensions;

namespace LY.EMIS5.Common.Extensions
{
    /// <summary>
    /// IQueryable扩展方法
    /// </summary>
    public static partial class IQueryableExtensions
    {
        /// <summary>
        /// 将IQueryable&lt;T&gt;转化为PagedQueryResult&lt;T&gt;
        /// </summary>
        /// <typeparam name="T">类型T</typeparam>
        /// <param name="query">IQueryable接口</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <returns></returns>
        public static PagedQueryResult<T> AsPageQueryResult<T>(this IQueryable<T> query, int pageIndex, int pageSize) where T : IEntityObject
        {
            return new PagedQueryResult<T>(pageSize, pageIndex, query.LongCount(), query.Skip(pageSize * pageIndex).Take(pageSize).ToList());
        }

        /// <summary>
        /// 将IQueryable&lt;T&gt;转化为PagedQueryResult&lt;T&gt;
        /// </summary>
        /// <typeparam name="T">类型T</typeparam>
        /// <typeparam name="K">类型T中的成员属性的类型</typeparam>
        /// <param name="query">IQueryable接口</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="orderExp">排序的lambda表达式 如:m=>m.id</param>
        /// <param name="ascending">是否为升序 默认为升序</param>
        /// <returns></returns>
        public static PagedQueryResult<T> AsPageQueryResult<T, K>(this IQueryable<T> query, int pageIndex, int pageSize, Expression<Func<T, K>> orderExp, bool ascending = true) where T : IEntityObject
        {
            if (ascending)
            {
                return new PagedQueryResult<T>(pageSize, pageIndex, query.LongCount(), query.OrderBy(orderExp).Skip(pageSize * pageIndex).Take(pageSize).ToList());
            }
            else
            {
                return new PagedQueryResult<T>(pageSize, pageIndex, query.LongCount(), query.OrderByDescending(orderExp).Skip(pageSize * pageIndex).Take(pageSize).ToList());
            }            
        }
    }
}
