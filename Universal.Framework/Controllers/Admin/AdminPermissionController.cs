using System;
using System.Collections.Generic;
using System.Text;

namespace Universal.Framework.Controllers.Admin
{
    /// <summary>
    /// 需要权限验证的控制器继承
    /// (前提要在登录状态下)
    /// </summary>
    [PermissionActionFilter]
    public class AdminPermissionController:PublicAdminController
    {
    }
}
