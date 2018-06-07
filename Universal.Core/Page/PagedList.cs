using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Universal.Core
{
    /// <summary>
    /// 分页接口实现类
    /// </summary>
    /// <typeparam name="T">T</typeparam>
    public class PagedList<T> : List<T>, IPagedList<T> where T : class
    {
        public PagedList()
        { }

        public PagedList(IQueryable<T> source, int pageIndex, int pageSize)
        {
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            pageSize = pageSize < 1 ? 1 : pageSize;
            this.TotalCount = source.Count();
            this.TotalPages = TotalCount / pageSize;
            if (this.TotalCount % pageSize > 0)
            {
                this.TotalPages++;
            }
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.AddRange(source.Skip((this.PageIndex - 1) * this.PageSize).Take(this.PageSize).ToList());
        }

        public PagedList(IList<T> source, int pageIndex, int pageSize)
        {
            pageSize = pageSize < 10 ? 10 : pageSize > 100 ? 100 : pageSize;
            this.TotalCount = source.Count();
            this.TotalPages = TotalCount / pageSize;

            if (TotalCount % pageSize > 0)
                this.TotalPages++;

            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.AddRange(source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList());
        }

        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; }

        /// <summary>
        /// 页条数 最高100页，最低10页
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        /// 总条数
        /// </summary>
        public int TotalCount { get; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages { get; }

        /// <summary>
        /// 是否有前一页
        /// </summary>
        public bool HasPreviousPage
        {
            get
            {
                return this.PageIndex > 1;
            }
        }

        /// <summary>
        /// 是否有下一页
        /// </summary>
        public bool HasNextPage
        {
            get
            {
                return this.PageIndex < TotalPages;
            }
        }
    }
}
