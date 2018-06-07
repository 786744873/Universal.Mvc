using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using Universal.Core;
using Universal.Framework.Security.Admin;

namespace Universal.Framework
{
    public static class HtmlHelperExtension
    {

        public static bool OwnPermission(this IHtmlHelper helper, string routeName)
        {
            var _adminAuthService = EnginContext.Current.Resolve<IAdminAuthService>();
            return _adminAuthService.Authorize(routeName);
        }
    }
}
