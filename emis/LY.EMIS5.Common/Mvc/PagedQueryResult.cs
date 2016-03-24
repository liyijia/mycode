using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LY.EMIS5.Common.Mvc
{
    public class PagedQueryResult<T>
    {
        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        /// 当前页码
        /// </summary>
        public int PageIndex { get; private set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public Int64 Total { get; private set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; private set; }

        /// <summary>
        /// 内容
        /// </summary>
        public List<T> QueryResult { get; private set; }

        public PagedQueryResult(int pageSize, int pageIndex, Int64 total, List<T> queryResult)
        {
            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.Total = total;
            this.QueryResult = queryResult ?? new List<T>();
            this.PageCount = (int)Math.Ceiling(this.Total / (float)this.PageSize);
        }

        public string ToDataTablesResult(string sEcho)
        {
            return JsonConvert.SerializeObject(new { sEcho = sEcho, iTotalRecords = this.Total, iTotalDisplayRecords = this.Total, aaData = this.QueryResult });
        }
    }
}
