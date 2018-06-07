using System;
using System.Collections.Generic;
using System.Text;

namespace Universal.Core
{
    /// <summary>
    /// 页面分页信息
    /// </summary>
    public class Pagination : Pagination<dynamic>
    {
        public Pagination(int pageIndex, int pageSize, int totalCount, int totalPages, bool hasPreviousPage, bool hasNextPage, string routeName, object param = null)
            :base(pageIndex, pageSize, totalCount, totalPages, hasPreviousPage, hasNextPage, routeName, param)
        {

        }
    }


    public class Pagination<A> where A:class
    {
        /// <summary>
        /// 页面分页信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="totalPages"></param>
        /// <param name="hasPreviousPage"></param>
        /// <param name="hasNextPage"></param>
        /// <param name="routeName"></param>
        /// <param name="param"></param>
        public Pagination(int pageIndex, int pageSize, int totalCount, int totalPages, bool hasPreviousPage, bool hasNextPage, string routeName, A param = null )
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.TotalCount = totalCount;
            this.TotalPages = totalPages;
            this.HasPreviousPage = hasPreviousPage;
            this.HasNextPage = hasNextPage;
            this.RouteName = routeName;
            this.RouteArg = param;
        }

        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 分页数
        /// </summary>
        public int TotalPages { get; set; }
        /// <summary>
        /// 是否有上一页
        /// </summary>
        public bool HasPreviousPage { get; set; }
        /// <summary>
        /// 是否有下一页
        /// </summary>
        public bool HasNextPage { get; set; }
        /// <summary>
        /// 路由名称
        /// </summary>
        public string RouteName { get; set; }
        /// <summary>
        /// 查询路由参数
        /// </summary>
        public A RouteArg { get; set; }
    }
}
