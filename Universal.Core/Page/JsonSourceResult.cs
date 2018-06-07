using System;
using System.Collections.Generic;
using System.Text;

namespace Universal.Core
{

    /// <summary>
    /// Json类型的分页返回数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JsonSourceResult<T> where T:class
    {
        /// <summary>
        /// Json类型的分页返回数据
        /// </summary>
        /// <param name="pageList"></param>
        public JsonSourceResult(IPagedList<T> pageList)
        {
            this.data = pageList;
            this.counts = pageList.TotalCount;
            this.page = pageList.PageIndex;
            this.prev = pageList.HasPreviousPage;
            this.next = pageList.HasNextPage;
            this.totalpage = pageList.TotalPages;
            this.size = pageList.PageSize;
        }


        /// <summary>
        /// shuj
        /// </summary>
        public IPagedList<T> data { get; set; }

        /// <summary>
        /// 总条数
        /// </summary>
        public int counts { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int totalpage { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int size { get; set; }
        /// <summary>
        /// 当前页
        /// </summary>
        public int page { get; set; }
        /// <summary>
        /// 是否有上一页
        /// </summary>
        public bool prev { get; set; }
        /// <summary>
        /// 是否有下一页
        /// </summary>
        public bool next { get; set; }
        


    }
}
