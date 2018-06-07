using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Universal.Core;
using Universal.Entities;
using Universal.Services;

namespace Universal.Framework.Security.Admin
{
    public class AdminAuthService : IAdminAuthService
    {
        private IHttpContextAccessor _httpContextAccessor;
        private ISysUserService _sysUserService;
        private ICategoryService _categoryService;
        private ISysUserRoleService _sysUserRoleService;
        private ISysPermissionService _sysPermissionService;

        public AdminAuthService(IHttpContextAccessor httpContextAccessor,
            ISysUserService sysUserService, 
            ICategoryService categoryService, 
            ISysUserRoleService sysUserRoleService, 
            ISysPermissionService sysPermissionService)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._sysUserService = sysUserService;
            this._categoryService = categoryService;
            this._sysUserRoleService = sysUserRoleService;
            this._sysPermissionService = sysPermissionService;
        }

        /// <summary>
        /// 保存登录状态
        /// </summary>
        /// <param name="token">登录token</param>
        /// <param name="name">用户名</param>
        public void SignIn(string token, string name)
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Sid, token));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, name));
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            // 往客户端写cookies
            this._httpContextAccessor.HttpContext.SignInAsync(CookieAdminAuthInfo.AuthenticationScheme, claimsPrincipal);
        }

        /// <summary>
        /// 获取当前登录用户
        /// </summary>
        /// <returns></returns>
        public SysUser GetCurrentUser()
        {
            var result= _httpContextAccessor.HttpContext.AuthenticateAsync(CookieAdminAuthInfo.AuthenticationScheme).Result;
            if (result.Principal==null)
            {
                return null;
            }
            var token= result.Principal.FindFirstValue(ClaimTypes.Sid);
            return _sysUserService.GetLogged(token);
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        public void SignOut()
        {
            _httpContextAccessor.HttpContext.SignOutAsync(CookieAdminAuthInfo.AuthenticationScheme);
        }

        /// <summary>
        /// 获取权限内的菜单列表
        /// </summary>
        /// <returns></returns>
        public List<Category> GetMyCategory()
        {
            var user = GetCurrentUser();
            return GerMyCategoryList(user);
        }

        /// <summary>
        /// 权限认证
        /// </summary>
        /// <param name="routeName"></param>
        /// <returns></returns>
        public bool Authorize(string routeName)
        {
            var user = GetCurrentUser();
            if (user==null)
            {
                return false;
            }
            //如果是管理员
            if (user.IsAdmin)
            {
                return true;
            }
            var list = GetMyCategory();
            if (list==null)
            {
                return false;
            }
            return list.Any(o => o.RouteName != null && o.RouteName.Equals(routeName, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        ///  权限验证
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool Authorize(ActionExecutingContext context)
        {
            var user = GetCurrentUser();
            if (user == null)
            {
                return false;
            }
            //如果是管理员
            if (user.IsAdmin)
            {
                return true;
            }
            string actiopn = context.ActionDescriptor.RouteValues["action"];
            string controller = context.ActionDescriptor.RouteValues["controller"];
            return Authorize(actiopn, controller);

        }

        /// <summary>
        /// 私有方法，判断权限
        /// </summary>
        /// <param name="action"></param>
        /// <param name="controller"></param>
        /// <returns></returns>
        private bool Authorize(string action, string controller)
        {
            var user = GetCurrentUser();
            if (user == null)
                return false;
            //如果是超级管理员
            if (user.IsAdmin) return true;
            var list = GetMyCategory();
            if (list==null)
            {
                return false;
            }
            return list.Any(o => o.Controller != null && o.Action != null && 
            o.Controller.Equals(controller, StringComparison.InvariantCultureIgnoreCase) && o.Action.Equals(action, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// 获取权限内的所有的菜单列表
        /// </summary>
        /// <param name="sysUser"></param>
        /// <returns></returns>
        private List<Category> GerMyCategoryList(SysUser sysUser)
        {
            var list = this._categoryService.GetAll();
            if (list==null)
            {
                return null;
            }
            if (sysUser.IsAdmin)
            {
                return list;
            }
            //获取权限数据
            var sysUserRoles = _sysUserRoleService.GetAll();//获取所有的用户角色信息
            if (sysUserRoles==null||!sysUserRoles.Any())
            {
                return null;
            }
            //获取当前用户所有的角色信息
            var roleIds = sysUserRoles.Where(o => o.UserId == sysUser.Id).Select(x=>x.RoleId).Distinct().ToList();

            var sysPermissions= _sysPermissionService.GetAll();//获取所有的用户角色权限信息
            if (sysPermissions==null||!sysPermissions.Any())
            {
                return null;
            }

            var categoryIds = sysPermissions.Where(o => roleIds.Contains(o.RoleId)).Select(o => o.CategoryId).Distinct().ToList();
            if (!categoryIds.Any())
            {
                return null;
            }
            list = list.Where(o => categoryIds.Contains(o.Id)).ToList();
            return list;
        }
    }
}
