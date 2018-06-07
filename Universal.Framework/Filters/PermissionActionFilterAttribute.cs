using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Universal.Core;
using Universal.Core.Extensions;
using Universal.Framework.Security.Admin;

namespace Universal.Framework
{
    /// <summary>
    /// 权限验证过滤器
    /// </summary>
    public class PermissionActionFilterAttribute : Attribute, IActionFilter
    {
        private readonly bool _notDoValidate;

        public PermissionActionFilterAttribute():this(false)
        { }

        public PermissionActionFilterAttribute(bool notDoValidate)
        {
            this._notDoValidate = notDoValidate;
        }


        /// <summary>
        /// 允许匿名访问
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private bool GetAllowAttributes(ActionExecutingContext context)
        {
            return context.ActionDescriptor.FilterDescriptors.Any(o => o.Filter.GetType().Name.Equals("AllowAnonymousAttribute"));
        }

        /// <summary>
        /// 只有添加此特性的控制器方法才启用权限验证
        /// 未添加的不判断权限
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private bool IsPermissionPageRequested(ActionExecutingContext context)
        {
            return context.ActionDescriptor.FilterDescriptors.Any(o => o.Filter.GetType().Name.Equals(this.GetType().Name));
        }

        /// <summary>
        /// 处理结果，跳转登录界面
        /// </summary>
        /// <param name="context"></param>
        private void HandlerRequest(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.IsAjaxRequest())
            {
                context.Result = new JsonResult(new AjaxResult { Status = false, Message = "您没有权限" });
            }
            else
            {
                context.Result = new ViewResult() { ViewName = "NoPermission" };
            }
        }


        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        /// <summary>
        /// 权限验证
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (_notDoValidate)
            {
                return;
            }
            if (GetAllowAttributes(context))
            {
                return;
            }
            if (IsPermissionPageRequested(context))
            {
                if (!EnginContext.Current.Resolve<IAdminAuthService>().Authorize(context))
                {
                    HandlerRequest(context);
                }
            }
        }
    }
}
