using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Universal.Entities;
using Universal.Framework.Security.Admin;

namespace Universal.Framework.Infrastructure
{
    public class WorkContext : IWorkContext
    {
        private IAdminAuthService _adminAuthService;

        public WorkContext(IAdminAuthService adminAuthService)
        {
            _adminAuthService = adminAuthService;
        }

        /// <summary>
        /// 获取当前登录用户
        /// </summary>
        /// <returns></returns>
        public SysUser CurrentUser()
        {
            return _adminAuthService.GetCurrentUser();
        }

        /// <summary>
        /// 当前登录用户的菜单
        /// </summary>
        /// <returns></returns>
        public List<Category> Categories()
        {
            return _adminAuthService.GetMyCategory();
        }

        /// <summary>
        /// 检查用户的权限
        /// </summary>
        /// <param name="routeName"></param>
        /// <returns></returns>
        public bool CheckUserPermission(string routeName)
        {
            var permissions = _adminAuthService.GetMyCategory();
            return permissions.Any(x => x.RouteName.Equals(routeName, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
