using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Universal.Core.Extensions
{
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// 判断是否是Ajax请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            if (request==null)
            {
                throw new ArgumentNullException("request");
            }
            if (request.Headers!=null)
            {
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            }
            return false;
        }
    }
}
