using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Universal.Core;
using Universal.Entities;

namespace Universal.Services
{
    public class SysRoleService : ISysRoleService
    {
        private const string MODEL_KEY = "Universal.Services.SysRole";


        private IMemoryCache _memoryCache;
        private IRepository<SysRole> _sysRoleRepository;
        private IRepository<SysPermission> _sysPermissionRepository;
        private IRepository<SysUserRole> _sysUserRoleRepository;
        private ISysPermissionService _sysPermissionService;


        public SysRoleService(IMemoryCache memoryCache,
            IRepository<SysRole> sysRoleRepository,
            IRepository<SysPermission> sysPermissionRepository,
            IRepository<SysUserRole> sysUserRoleRepository,
            ISysPermissionService sysPermissionService)
        {
            _memoryCache = memoryCache;
            _sysRoleRepository = sysRoleRepository;
            _sysPermissionRepository = sysPermissionRepository;
            _sysUserRoleRepository = sysUserRoleRepository;
            _sysPermissionService = sysPermissionService;
        }

        /// <summary>
        /// 新增角色并清除相关缓存
        /// </summary>
        /// <param name="roleId">角色ID</param>
        public void DeleteRole(Guid roleId)
        {
            var item = _sysRoleRepository.GetById(roleId);
            if (item == null)
            {
                return;
            }
            foreach (var del in item.SysPermission.ToList())
            {//删除角色权限
                _sysPermissionRepository.Entities.Remove(del);
            }
            foreach (var del in item.SysUserRole.ToList())
            {//删除用户角色
                _sysUserRoleRepository.Entities.Remove(del);
            }
            _sysRoleRepository.Entities.Remove(item);//删除角色
            RemoveCache();//清除角色缓存
            _sysPermissionService.RemoveCache();//清除角色权限缓存
            //_sysUserRoleService.removeCache();
        }

        /// <summary>
        /// 获取所有角色并缓存
        /// </summary>
        /// <returns></returns>
        public List<SysRole> GetAllRoles()
        {
            List<SysRole> list = null;
            _memoryCache.TryGetValue(MODEL_KEY, out list);
            if (list!=null)
            {
                return list;
            }
            list = _sysRoleRepository.Table.ToList();
            _memoryCache.Set(MODEL_KEY, list, DateTime.Now.AddDays(1));
            return list;
        }

        /// <summary>
        /// 获取角色详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SysRole GetRole(Guid id)
        {
            return _sysRoleRepository.GetById(id);
        }

        /// <summary>
        /// 新增角色并清除相关缓存
        /// </summary>
        /// <param name="sysRole"></param>
        public void InsertRole(SysRole sysRole)
        {
            _sysRoleRepository.Insert(sysRole);
            RemoveCache();
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="sysRole"></param>
        public void UpdateRole(SysRole sysRole)
        {
            var item = _sysRoleRepository.GetById(sysRole.Id);
            if (item==null)
            {
                return;
            }
            item.Name = sysRole.Name;
            item.ModifiedTime = DateTime.Now;
            item.Modifier = sysRole.Modifier;
            _sysRoleRepository.Update(item);
            RemoveCache();
        }

        /// <summary>
        /// 清除角色缓存
        /// </summary>
        public void RemoveCache()
        {
            _memoryCache.Remove(MODEL_KEY);
        }

        /// <summary>
        /// 保存更新
        /// </summary>
        public void SaveChanges()
        {
            _sysRoleRepository.DbContext.SaveChanges();
        }

        /// <summary>
        /// 是否存在数据
        /// </summary>
        /// <returns></returns>
        public bool ExistRole()
        {
            return _sysRoleRepository.Entities.Any();
        }
    }
}
