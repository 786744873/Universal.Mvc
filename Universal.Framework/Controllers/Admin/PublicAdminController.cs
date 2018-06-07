using System;
using System.Collections.Generic;
using System.Text;

namespace Universal.Framework.Controllers.Admin
{
    /// <summary>
    /// 需要登录验证的控制器继承
    /// </summary>
    [AdminAuthFilter]
    public class PublicAdminController: AdminAreaController
    {
    }
}
