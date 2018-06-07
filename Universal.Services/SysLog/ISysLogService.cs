using System;
using System.Collections.Generic;
using System.Text;
using Universal.Core;
using Universal.Entities;
using Universal.Entities.Dto;

namespace Universal.Services
{
    public interface ISysLogService
    {
        /// <summary>
        /// 分页查询系统日志
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        IPagedList<SysLog> SearchSysLog(SysLogSearchArg arg, int page, int size);

        /// <summary>
        /// 插入一条系统日志
        /// </summary>
        /// <param name="model"></param>
        void InsertSysLog(SysLog model);

    }
}
