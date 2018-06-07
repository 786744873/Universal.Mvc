using System;
using System.Collections.Generic;
using System.Text;

namespace Universal.Core
{
    /// <summary>
    /// 返回分页内容（数据和页面数据）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataSourceResult<T> : DataSourceResult<T, dynamic> where T : class
    {
        public DataSourceResult(IPagedList<T> pageList,string routeName):base(pageList,routeName,null)
        {

        }

        public DataSourceResult(IPagedList<T> pageList, string routeName, object param) : base(pageList, routeName, param)
        {

        }
    }

    public class DataSourceResult<T, A> : DataSourceResult<T, A, dynamic> where T : class where A : class
    {
        public DataSourceResult(IPagedList<T> pageList, string routeName, A param) : base(pageList, routeName, param, null)
        {
        }
    }

    public class DataSourceResult<T, A, M1> : DataSourceResult<T, A, M1, dynamic> where T : class where A : class where M1 : class
    {
        public DataSourceResult(IPagedList<T> pageList, string routeName, A param, M1 m1) : base(pageList, routeName, param, m1, null)
        {
        }
    }

    public class DataSourceResult<T, A, M1, M2> : DataSourceResult<T, A, M1, M2, dynamic> where T : class where A : class where M1 : class where M2 : class
    {
        public DataSourceResult(IPagedList<T> pageList, string routeName, A param, M1 m1, M2 m2):base(pageList, routeName, param, m1, m2, null)
        {

        }
    }



    /// <summary>
    /// 返回分页内容（数据和页面数据）
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="A"></typeparam>
    /// <typeparam name="M1"></typeparam>
    /// <typeparam name="M2"></typeparam>
    /// <typeparam name="M3"></typeparam>
    public class DataSourceResult<T, A, M1, M2, M3> where T : class where A : class where M1 : class where M2 : class where M3 : class
    {

        public DataSourceResult(IPagedList<T> pageList, string routeName, A param, M1 m1, M2 m2, M3 m3)
        {
            this.Data = pageList;
            this.Paging = new Pagination(pageList.PageIndex, pageList.PageSize, pageList.TotalCount, pageList.TotalPages, pageList.HasPreviousPage, pageList.HasNextPage, routeName, param);
            this.Item1 = m1;
            this.Item2 = m2;
            this.Item3 = m3;
        }


        /// <summary>
        /// 数据
        /// </summary>
        public IPagedList<T> Data { get; }

        /// <summary>
        /// 分页参数
        /// </summary>
        public Pagination Paging { get; }

        /// <summary>
        /// 其他数据1
        /// </summary>
        public M1 Item1 { get; }

        /// <summary>
        /// 其他数据2
        /// </summary>
        public M2 Item2 { get; }

        /// <summary>
        /// 其他数据3
        /// </summary>
        public M3 Item3 { get; }
    }
}
