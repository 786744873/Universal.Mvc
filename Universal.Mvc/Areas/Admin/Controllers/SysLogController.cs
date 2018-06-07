using Microsoft.AspNetCore.Mvc;
using Universal.Entities;
using Universal.Services;
using Universal.Core;
using Universal.Entities.Dto;
using Universal.Framework.Menu;
using Universal.Framework.Controllers.Admin;

namespace Universal.Mvc.Areas.Admin.Controllers
{
    [Route("admin/syslog")]
    [Function("系统日志", true, "menu-icon fa fa-caret-right", FatherResource = "Universal.Mvc.Areas.Admin.Controllers.SystemManageController", Sort = 0)]
    public class SysLogController :AdminAreaController

    {

        private ISysLogService _sysLogService;

        public SysLogController(ISysLogService sysLogService)
        {
            this._sysLogService = sysLogService;
        }

        [Function("系统日志",true, "", FatherResource = "Universal.Mvc.Areas.Admin.Controllers.SysLogController", Sort = 0)]
        [Route("index",Name = "sysLogIndex")]
        public IActionResult sysLogIndex(SysLogSearchArg arg, int page = 1, int size = 20)
        {
            var pageList= _sysLogService.SearchSysLog(arg,page,size);
            var dataSource= pageList.ToDataSourceResult<SysLog, SysLogSearchArg>("sysLogIndex",arg);
            return View(dataSource);
        }
    }
}