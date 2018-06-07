using System;
using System.Collections.Generic;
using System.Text;

namespace Universal.Core
{
    /// <summary>
    /// 分页接口
    /// </summary>
    /// <typeparam name="T">T</typeparam>
    public interface IPagedList<T>:IList<T>
    {
        /// <summary>
        /// 当前页
        /// </summary>
        int PageIndex { get; }

        /// <summary>
        /// 页条数 最高100页，最低10页
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// 总条数
        /// </summary>
        int TotalCount { get; }

        /// <summary>
        /// 总页数
        /// </summary>
        int TotalPages { get; }

        /// <summary>
        /// 是否有前一页
        /// </summary>
        bool HasPreviousPage { get; }

        /// <summary>
        /// 是否有下一页
        /// </summary>
        bool HasNextPage { get; }
    }
}
