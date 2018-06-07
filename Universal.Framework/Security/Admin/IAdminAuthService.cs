using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using Universal.Entities;

namespace Universal.Framework.Security.Admin
{
    public interface IAdminAuthService
    {
        /// <summary>
        /// 保存登录状态
        /// </summary>
        /// <param name="token">登录token</param>
        /// <param name="name">用户名</param>
        void SignIn(string token, string name);

        /// <summary>
        /// 获取当前登录用户
        /// </summary>
        /// <returns></returns>
        SysUser GetCurrentUser();

        /// <summary>
        /// 退出登录
        /// </summary>
        void SignOut();

        /// <summary>
        /// 获取权限内的菜单列表
        /// </summary>
        /// <returns></returns>
        List<Category> GetMyCategory();


        /// <summary>
        /// 权限验证
        /// </summary>
        /// <param name="routeName"></param>
        /// <returns></returns>
        bool Authorize(string routeName);

        /// <summary>
        /// 权限验证
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        bool Authorize(ActionExecutingContext context);
    }
}
