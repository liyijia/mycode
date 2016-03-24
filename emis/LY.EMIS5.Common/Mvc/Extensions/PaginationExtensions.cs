using NHibernate.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Diagnostics.Contracts;

namespace LY.EMIS5.Common.Mvc.Extensions
{
    /// <summary>
    /// HtmlHelper 分页扩展方法
    /// </summary>
    public static class PaginationExtensions
    {
        #region 分页所用的一些私有方法
        
        /// <summary>
        /// 合并路由键值对集合
        /// </summary>
        /// <param name="dictionaries">路由键值对集合 要替换的值应先放到前面</param>
        /// <returns>路由键值对集合</returns>
        private static RouteValueDictionary MergeDictionaries(params RouteValueDictionary[] dictionaries)
        {
            var result = new RouteValueDictionary();

            if (dictionaries == null || !dictionaries.Any())
            {
                return result;
            }

            foreach (RouteValueDictionary dictionary in dictionaries.Where(d => d != null))
            {
                foreach (KeyValuePair<string, object> kvp in dictionary)
                {
                    if (!result.ContainsKey(kvp.Key))
                    {
                        result.Add(kvp.Key, kvp.Value);
                    }
                }
            }
            return result;
        }
        
        /// <summary>
        /// 创建分页跳转的Url
        /// </summary>
        /// <param name="htmlHelper">HtmlHelper</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="actionName">操作名</param>
        /// <param name="controllerName">控制器名</param>
        /// <param name="routeValues">路由参数集合</param>
        /// <returns>Url字符串</returns>
        private static string ActionUrl(HtmlHelper htmlHelper, int pageIndex, string actionName, string controllerName, RouteValueDictionary routeValues)
        {
            var url = UrlHelper.GenerateUrl(null, actionName, controllerName, MergeDictionaries(new RouteValueDictionary(new { page = pageIndex }), routeValues, htmlHelper.ViewContext.RouteData.Values), RouteTable.Routes, htmlHelper.ViewContext.RequestContext, false);
            return url;
        }

        /// <summary>
        /// 将TagBuilder实例包装进 li 列表中
        /// </summary>
        /// <param name="inner">Html标签</param>
        /// <param name="classes">css</param>
        /// <returns>li标签</returns>
        public static TagBuilder WrapInListItem(TagBuilder inner, params string[] classes)
        {
            var li = new TagBuilder("li");
            if (classes != null)
            {
                foreach (var @class in classes)
                    li.AddCssClass(@class);
            }

            li.InnerHtml = inner.ToString();
            return li;
        }

        /// <summary>
        /// 创建分页中的第一页标签
        /// </summary>
        /// <typeparam name="T">类型T</typeparam>
        /// <param name="htmlHelper">HtmlHelper</param>
        /// <param name="queryResult">分页查询结果实体</param>
        /// <param name="action">操作名</param>
        /// <param name="controller">控制器名</param>
        /// <param name="routeValues">路由参数集合</param>
        /// <returns>A标签</returns>
        public static TagBuilder First<T>(HtmlHelper htmlHelper, PagedQueryResult<T> queryResult, string action, string controller, RouteValueDictionary routeValues) where T : IEntityObject
        {
            Contract.Requires(queryResult != null);
            int targetPageNumber = 0;
            var first = new TagBuilder("a") { InnerHtml = "首页" };

            if (queryResult.PageIndex == 0)
            {
                return WrapInListItem(first, "PagedList-skipToFirst", "disabled");
            }

            first.Attributes["href"] = ActionUrl(htmlHelper, targetPageNumber, action, controller, routeValues);

            return WrapInListItem(first, "PagedList-skipToFirst");
        }

        /// <summary>
        /// 创建分页中的上一页标签
        /// </summary>
        /// <typeparam name="T">类型T</typeparam>
        /// <param name="htmlHelper">HtmlHelper</param>
        /// <param name="queryResult">分页查询结果实体</param>
        /// <param name="action">操作名</param>
        /// <param name="controller">控制器名</param>
        /// <param name="routeValues">路由参数集合</param>
        /// <returns>A标签</returns>
        private static TagBuilder Previous<T>(HtmlHelper htmlHelper, PagedQueryResult<T> queryResult, string action, string controller, RouteValueDictionary routeValues) where T : IEntityObject
        {
            var targetPageNumber = queryResult.PageIndex - 1;
            var previous = new TagBuilder("a")
            {
                InnerHtml = "上一页"
            };

            if (!(queryResult.PageCount > 1))
                return WrapInListItem(previous, "PagedList-skipToPrevious", "disabled");

            previous.Attributes["href"] = ActionUrl(htmlHelper, targetPageNumber > 0 ? targetPageNumber : 0, action, controller, routeValues);
            return WrapInListItem(previous, "PagedList-skipToPrevious");
        }

        /// <summary>
        /// 创建分页中的页标签
        /// </summary>
        /// <typeparam name="T">类型T</typeparam>
        /// <param name="htmlHelper">HtmlHelper</param>
        /// <param name="i">页索引</param>
        /// <param name="queryResult">分页查询结果实体</param>
        /// <param name="action">操作名</param>
        /// <param name="controller">控制器名</param>
        /// <param name="routeValues">路由参数集合</param>
        /// <returns>A标签</returns>
        private static TagBuilder Page<T>(HtmlHelper htmlHelper, int i, PagedQueryResult<T> queryResult, string action, string controller, RouteValueDictionary routeValues) where T : IEntityObject
        {
            var targetPageNumber = i;
            var page = new TagBuilder("a");
            page.SetInnerText(targetPageNumber.ToString());

            if (i - 1 == queryResult.PageIndex)
                return WrapInListItem(page, "active");

            page.Attributes["href"] = ActionUrl(htmlHelper, targetPageNumber - 1, action, controller, routeValues);
            return WrapInListItem(page, null);
        }

        /// <summary>
        /// 创建分页中的下一页标签
        /// </summary>
        /// <typeparam name="T">类型T</typeparam>
        /// <param name="htmlHelper">HtmlHelper</param>
        /// <param name="queryResult">分页查询结果实体</param>
        /// <param name="action">操作名</param>
        /// <param name="controller">控制器名</param>
        /// <param name="routeValues">路由参数集合</param>
        /// <returns>A标签</returns>
        private static TagBuilder Next<T>(HtmlHelper htmlHelper, PagedQueryResult<T> queryResult, string action, string controller, RouteValueDictionary routeValues) where T : IEntityObject
        {
            var targetPageNumber = queryResult.PageIndex + 1;
            var next = new TagBuilder("a")
            {
                InnerHtml = "下一页"
            };

            if (!(queryResult.PageIndex < queryResult.PageCount))
                return WrapInListItem(next, "PagedList-skipToNext", "disabled");

            next.Attributes["href"] = ActionUrl(htmlHelper, targetPageNumber < queryResult.PageCount - 1 ? targetPageNumber : queryResult.PageCount - 1, action, controller, routeValues);
            return WrapInListItem(next, "PagedList-skipToNext");
        }

        /// <summary>
        /// 创建分页中的最后一页标签
        /// </summary>
        /// <typeparam name="T">类型T</typeparam>
        /// <param name="htmlHelper">HtmlHelper</param>
        /// <param name="queryResult">分页查询结果实体</param>
        /// <param name="action">操作名</param>
        /// <param name="controller">控制器名</param>
        /// <param name="routeValues">路由参数集合</param>
        /// <returns>A标签</returns>
        private static TagBuilder Last<T>(HtmlHelper htmlHelper, PagedQueryResult<T> queryResult, string action, string controller, RouteValueDictionary routeValues) where T : IEntityObject
        {
            var targetPageNumber = queryResult.PageCount - 1;
            var last = new TagBuilder("a")
            {
                InnerHtml = "尾页"
            };

            if (queryResult.PageIndex >= queryResult.PageCount)
                return WrapInListItem(last, "PagedList-skipToLast", "disabled");

            last.Attributes["href"] = ActionUrl(htmlHelper, targetPageNumber, action, controller, routeValues);
            return WrapInListItem(last, "PagedList-skipToLast");
        }

        /// <summary>
        /// 创建分页中的省略号...
        /// </summary>
        /// <returns>A标签</returns>
        private static TagBuilder Ellipses()
        {
            var a = new TagBuilder("a")
            {
                //InnerHtml = "..."
                InnerHtml = "&#8230;"
            };

            return WrapInListItem(a, "PagedList-ellipses", "disabled");
        }

        #endregion

        /// <summary>
        /// 显示分页
        /// </summary>
        /// <typeparam name="T">类型T</typeparam>
        /// <param name="htmlHelper">强类型视图的HtmlHelper</param>
        /// <param name="action">操作名</param>
        /// <param name="routeValues">路由参数对象 注:分页页名为page</param>
        /// <returns>html</returns>
        public static IHtmlString PaginationFor<T>(this HtmlHelper<PagedQueryResult<T>> htmlHelper, string action, object routeValues)
            where T : IEntityObject
        {
            return PaginationFor(htmlHelper, action, null, routeValues);
        }

        /// <summary>
        /// 显示分页
        /// </summary>
        /// <typeparam name="T">类型T</typeparam>
        /// <param name="htmlHelper">强类型视图的HtmlHelper</param>
        /// <param name="action">操作名</param>
        /// <param name="controller">控制器名</param>
        /// <param name="routeValues">路由参数对象 默认值为null 注:分页页名为page</param>
        /// <returns>html</returns>
        public static IHtmlString PaginationFor<T>(this HtmlHelper<PagedQueryResult<T>> htmlHelper, string action, string controller = null, object routeValues = null)
             where T : IEntityObject
        {
            return PaginationFor(htmlHelper, action, controller, routeValues == null ? null : new RouteValueDictionary(routeValues));
        }

        /// <summary>
        /// 显示分页
        /// </summary>
        /// <typeparam name="T">类型T</typeparam>
        /// <param name="htmlHelper">强类型视图的HtmlHelper</param>
        /// <param name="action">操作名</param>
        /// <param name="controller">控制器名</param>
        /// <param name="routeValues">路由参数集合 注:分页页名为page</param>
        /// <returns>html</returns>
        public static IHtmlString PaginationFor<T>(this HtmlHelper<PagedQueryResult<T>> htmlHelper, string action, string controller, RouteValueDictionary routeValues)
            where T : IEntityObject
        {
            return Pagination(htmlHelper, htmlHelper.ViewData.Model, action, controller, routeValues);
        }

        /// <summary>
        /// 显示分页
        /// </summary>
        /// <typeparam name="T">类型T</typeparam>
        /// <param name="htmlHelper">HtmlHelper</param>
        /// <param name="queryResult">分页查询结果实体</param>
        /// <param name="action">操作名</param>
        /// <param name="routeValues">路由参数对象 注:分页页名为page</param>
        /// <returns>html</returns>
        public static IHtmlString Pagination<T>(this HtmlHelper htmlHelper, PagedQueryResult<T> queryResult, string action, object routeValues)
            where T : IEntityObject
        {
            return Pagination(htmlHelper, queryResult, action, null, routeValues);
        }

        /// <summary>
        /// 显示分页
        /// </summary>
        /// <typeparam name="T">类型T</typeparam>
        /// <param name="htmlHelper">HtmlHelper</param>
        /// <param name="queryResult">分页查询结果实体</param>
        /// <param name="action">操作名</param>
        /// <param name="controller">控制器名 默认值为null</param>
        /// <param name="routeValues">路由参数对象 默认值为null 注:分页页名为page</param>
        /// <returns>html</returns>
        public static IHtmlString Pagination<T>(this HtmlHelper htmlHelper, PagedQueryResult<T> queryResult, string action, string controller = null, object routeValues = null)
             where T : IEntityObject
        {
            return Pagination(htmlHelper, queryResult, action, controller, routeValues == null ? null : new RouteValueDictionary(routeValues));
        }

        /// <summary>
        /// 显示分页
        /// </summary>
        /// <typeparam name="T">类型T</typeparam>
        /// <param name="htmlHelper">HtmlHelper</param>
        /// <param name="queryResult">分页查询结果实体</param>
        /// <param name="action">操作名</param>
        /// <param name="controller">控制器名</param>
        /// <param name="routeValues">路由参数集合 注:分页页名为page</param>
        /// <returns>html</returns>
        public static IHtmlString Pagination<T>(this HtmlHelper htmlHelper, PagedQueryResult<T> queryResult, string action, string controller, RouteValueDictionary routeValues)
            where T : IEntityObject
        {
            TagBuilder div = new TagBuilder("div");
            div = WrapInListItem(div, "pagination", "pagination-centered");
            if (queryResult == null || queryResult.QueryResult == null || ! queryResult.QueryResult.Any())
            {
                div.InnerHtml = "未检索到数据";
                return new MvcHtmlString(div.ToString());
            }
            
            var listItemLinks = new List<TagBuilder>();
            var firstPageToDisplay = 1;
            var lastPageToDisplay = queryResult.PageCount;
            var pageNumbersToDisplay = lastPageToDisplay;
            var maxPageNumbersToDisplay = 5;

            if (queryResult.PageCount > maxPageNumbersToDisplay)
            {
                firstPageToDisplay = queryResult.PageIndex - maxPageNumbersToDisplay / 2;
                if(firstPageToDisplay < 1)
                {
                    firstPageToDisplay = 1;
                }
                pageNumbersToDisplay = maxPageNumbersToDisplay;
                lastPageToDisplay = firstPageToDisplay + pageNumbersToDisplay - 1;
                if (lastPageToDisplay > queryResult.PageCount)
                {
                    firstPageToDisplay = queryResult.PageCount - maxPageNumbersToDisplay + 1;
                }
            }

            if (firstPageToDisplay > 1)
            {
                listItemLinks.Add(First(htmlHelper, queryResult, action, controller, routeValues));
            }

            if (queryResult.PageIndex - 1 >= 0)
            {
                listItemLinks.Add(Previous(htmlHelper, queryResult, action, controller, routeValues));
            }

            if (firstPageToDisplay > 1)
            {
                listItemLinks.Add(Ellipses());
            }
            foreach (var i in Enumerable.Range(firstPageToDisplay, pageNumbersToDisplay))
            {
                listItemLinks.Add(Page(htmlHelper, i, queryResult, action, controller, routeValues));
            }
            if ((firstPageToDisplay + pageNumbersToDisplay) < queryResult.PageCount)
            {
                listItemLinks.Add(Ellipses());
            }

            if (!(queryResult.PageIndex + 1 >= queryResult.PageCount))
            {
                listItemLinks.Add(Next(htmlHelper, queryResult, action, controller, routeValues));
            }
            if (lastPageToDisplay < queryResult.PageCount)
            {
                listItemLinks.Add(Last(htmlHelper, queryResult, action, controller, routeValues));
            }

            var listItemLinksString = listItemLinks.Aggregate(
                new StringBuilder(),
                    (sb, listItem) => sb.Append(listItem.ToString()),
                        sb => sb.ToString()
                );

            var ul = new TagBuilder("ul")
            {
                InnerHtml = listItemLinksString
            };

            div.InnerHtml = ul.ToString();

            return new MvcHtmlString(div.ToString());
        }
    }
}
