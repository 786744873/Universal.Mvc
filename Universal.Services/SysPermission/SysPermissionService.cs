using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Universal.Core;
using Universal.Entities;

namespace Universal.Services.SysPermissionService
{
    public class SysPermissionService : ISysPermissionService
    {
        private const string MODEL_KEY = "Universal.Services.SysPermission";

        private IMemoryCache _memoryCache;
        private IRepository<SysPermission> _sysPermissionRepository;

        public SysPermissionService(IMemoryCache memoryCache, IRepository<SysPermission> sysPermissionRepository)
        {
            this._memoryCache = memoryCache;
            this._sysPermissionRepository = sysPermissionRepository;
        }

        /// <summary>
        /// 获取所有的菜单角色信息，并缓存
        /// </summary>
        /// <returns></returns>
        public List<SysPermission> GetAll()
        {
            List<SysPermission> list = null;
            _memoryCache.TryGetValue(MODEL_KEY, out list);
            if (list!=null)
            {
                return list;
            }
            list = _sysPermissionRepository.Table.ToList();
            _memoryCache.Set(MODEL_KEY, list,DateTime.Now.AddDays(1));
            return list;
        }

        /// <summary>
        /// 根据角色ID获取所有的菜单角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<SysPermission> GetByRoleID(Guid id)
        {
            var list = GetAll();
            if (list==null)
            {
                return null;
            }
            return list.Where(o => o.RoleId == id).ToList();
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        public void RemoveCache()
        {
            _memoryCache.Remove(MODEL_KEY);
        }

        /// <summary>
        /// 保存角色权限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="categoryIds">权限ID集合</param>
        /// <param name="creator">创建人ID</param>
        public void SaveRolePermission(Guid roleId, List<int> categoryIds, Guid creator)
        {
            //移除全选
            categoryIds.Remove(0);
            //查询角色所有角色权限
            var list = _sysPermissionRepository.Table.Where(o => o.RoleId == roleId);
            if (categoryIds==null||!categoryIds.Any())
            {//（取消）角色所有权限
                foreach (var del in list)
                {
                    _sysPermissionRepository.Entities.Remove(del);
                }
            }
            else
            {
                //新权限增加
                foreach (var categoryId in categoryIds)
                {
                    var item = list.FirstOrDefault(o => o.CategoryId == categoryId);
                    if (item==null)
                    {
                        _sysPermissionRepository.Entities.Add(new SysPermission
                        {
                            Id = Guid.NewGuid(),
                            RoleId = roleId,
                            CreationTime = DateTime.Now,
                            Creator = creator,
                            CategoryId = categoryId
                        });
                    }
                }
                //不存在的旧权限删除
                foreach (var del in list)
                {
                    if (!categoryIds.Any(o=>o==del.CategoryId))
                    {
                        _sysPermissionRepository.Entities.Remove(del);
                    }
                }
                
            }
            _sysPermissionRepository.DbContext.SaveChanges();
            //清除缓存
            RemoveCache();
        }

    }
}
