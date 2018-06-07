using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using Universal.Core;
using Universal.Framework.Security.Admin;

namespace Universal.Framework
{
    /// <summary>
    /// 登录验证过滤器
    /// </summary>
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method,AllowMultiple =true,Inherited =true)]
    public class AdminAuthFilterAttribute : Attribute, IResourceFilter
    {
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            var _adminAuthService = EnginContext.Current.Resolve<IAdminAuthService>();
            var user = _adminAuthService.GetCurrentUser();
            if (user==null||!user.Enabled)
            {
                context.Result = new RedirectToRouteResult("adminLogin", new { returnUrl = context.HttpContext.Request.Path });
            }
        }
    }
}
