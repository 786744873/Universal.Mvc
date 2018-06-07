using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Universal.Core;
using Universal.Entities;
using Universal.Entities.Dto;

namespace Universal.Services
{
    public class SysLogService: ISysLogService
    {
        private IRepository<SysLog> _sysLogRepository;

        public SysLogService(IRepository<SysLog> sysLogRepository)
        {
            _sysLogRepository = sysLogRepository;
        }


        public IPagedList<SysLog> SearchSysLog(SysLogSearchArg arg, int page, int size)
        {
            var query = _sysLogRepository.Table;
            if (arg!=null)
            {
                if (arg.Level>0)
                {
                    query = query.Where(o => o.Level == arg.Level);
                }
                if (!string.IsNullOrEmpty(arg.ShortMessage))
                {
                    query = query.Where(o => o.ShortMessage.Contains(arg.ShortMessage));
                }
            }
            query = query.OrderByDescending(o => o.CreationTime);
            return new PagedList<SysLog>(query, page, size);
        }

        /// <summary>
        /// 插入一条系统信息
        /// </summary>
        /// <param name="model"></param>
        public void InsertSysLog(SysLog model)
        {
            _sysLogRepository.Insert(model);
        }
    }
}
