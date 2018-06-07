using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Universal.Core
{
    public static class IPHelper
    {
        /// <summary>
        /// 获取IP信息
        /// </summary>
        /// <param name="httpContextAccessor">HttpContext上下文</param>
        /// <returns></returns>
        public static string GetIPContent(IHttpContextAccessor httpContextAccessor)
        {
            return httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
        }
    }
}
