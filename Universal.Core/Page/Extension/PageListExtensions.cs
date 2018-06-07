using System;
using System.Collections.Generic;
using System.Text;

namespace Universal.Core
{
    public static class PageListExtensions
    {
        public static DataSourceResult<T> ToDataSourceResult<T>(this IPagedList<T> pageList, dynamic param = null) where T : class
        {
            return new DataSourceResult<T>(pageList,null,param);
        }

        public static DataSourceResult<T> ToDataSourceResult<T>(this IPagedList<T> pageList, string routeName, dynamic param = null) where T : class
        {
            return new DataSourceResult<T>(pageList, routeName, param);
        }

        public static DataSourceResult<T, A> ToDataSourceResult<T, A>(this IPagedList<T> pageList, string routeName, A param) where T : class where A :class
        {
            return new DataSourceResult<T, A>(pageList,routeName,param);
        }

        public static DataSourceResult<T, A, M1> ToDataSourceResult<T, A, M1>(this IPagedList<T> pageList, string routeName, A param, M1 m1) where T : class where A : class where M1 : class
        {
            return new DataSourceResult<T, A, M1>(pageList, routeName, param, m1);
        }

        public static DataSourceResult<T, A, M1, M2> ToDataSourceResult<T, A, M1, M2>(this IPagedList<T> pageList, string routeName, A param, M1 m1, M2 m2)
            where T : class where A : class where M1 : class where M2 : class
        {
            return new DataSourceResult<T, A, M1, M2>(pageList, routeName, param, m1, m2);
        }

        public static DataSourceResult<T, A, M1, M2, M3> ToDataSourceResult<T, A, M1, M2, M3>(this IPagedList<T> pageList, string routeName, A param, M1 m1, M2 m2, M3 m3)
            where T : class where A : class where M1 : class where M2 : class where M3 : class
        {
            return new DataSourceResult<T, A, M1, M2, M3>(pageList, routeName, param, m1, m2, m3);
        }

        /// <summary>
        /// 返回Json格式数据列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pageList"></param>
        /// <returns></returns>
        public static JsonSourceResult<T> ToJsonSourceResult<T>(this IPagedList<T> pageList) where T : class
        {
            return new JsonSourceResult<T>(pageList);
        }
    }
}
