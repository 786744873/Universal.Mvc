using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Universal.Core;
using Universal.Entities;

namespace Universal.Services
{
    public class SysUserRoleService : ISysUserRoleService
    {
        private const string MODEL_KEY = "Universal.Services.SysUserRole";

        private IMemoryCache _memoryCache;
        private IRepository<SysUserRole> _sysUserRoleRepository;

        public SysUserRoleService(IMemoryCache memoryCache, IRepository<SysUserRole> sysUserRoleRepository)
        {
            _memoryCache = memoryCache;
            _sysUserRoleRepository = sysUserRoleRepository;
        }


        /// <summary>
        /// 获取所有的用户角色数据
        /// (缓存)
        /// </summary>
        /// <returns></returns>
        public List<SysUserRole> GetAll()
        {
            List<SysUserRole> list = null;
            _memoryCache.TryGetValue(MODEL_KEY,out list);
            if (list!=null)
            {
                return list;
            }
            list = _sysUserRoleRepository.Table.ToList();
            _memoryCache.Set(MODEL_KEY, list);
            return list;
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        public void RemoveCahce()
        {
            _memoryCache.Remove(MODEL_KEY);
        }

        /// <summary>
        /// 新增一条用户权限数据
        /// </summary>
        /// <param name="sysUserId"></param>
        /// <param name="sysRoleId"></param>
        public void InsertOrUpdateSysUserRole(Guid sysUserId, Guid sysRoleId)
        {
            if (Exist(sysUserId))
            {
                var sysUserRole= _sysUserRoleRepository.Entities.FirstOrDefault(o => o.UserId == sysUserId);
                sysUserRole.RoleId = sysRoleId;
                _sysUserRoleRepository.Update(sysUserRole);
            }
            else
            {
                _sysUserRoleRepository.Insert(new SysUserRole { UserId = sysUserId, RoleId = sysRoleId });
            }
        }

        /// <summary>
        /// 判断权限是否已经存在
        /// </summary>
        /// <param name="sysUserId"></param>
        /// <returns></returns>
        public bool Exist(Guid sysUserId)
        {
            return _sysUserRoleRepository.Entities.Any(o => o.UserId == sysUserId);
        }
    }
}
