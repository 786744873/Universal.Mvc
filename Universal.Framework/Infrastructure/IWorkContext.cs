using System;
using System.Collections.Generic;
using System.Text;
using Universal.Entities;

namespace Universal.Framework.Infrastructure
{
    public interface IWorkContext
    {
        /// <summary>
        /// 当前登录用户
        /// </summary>
        /// <returns></returns>
        SysUser CurrentUser();

        /// <summary>
        /// 当前登录用户的菜单
        /// </summary>
        /// <returns></returns>
        List<Category> Categories();

        /// <summary>
        /// 当前登录用户的权限
        /// </summary>
        /// <returns></returns>
        bool CheckUserPermission(string routeName);
    }
}
