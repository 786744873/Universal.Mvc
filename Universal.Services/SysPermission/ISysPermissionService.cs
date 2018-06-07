using System;
using System.Collections.Generic;
using System.Text;
using Universal.Core;
using Universal.Entities;

namespace Universal.Services
{
    public interface ISysPermissionService
    {
        /// <summary>
        /// 获得所有权限
        /// </summary>
        /// <returns></returns>
        List<SysPermission> GetAll();

        /// <summary>
        /// 移除缓存
        /// </summary>
        void RemoveCache();


        /// <summary>
        /// 根据RoleID获取权限
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<SysPermission> GetByRoleID(Guid id);

        /// <summary>
        /// 保存角色权限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="categoryIds">权限ID集合</param>
        /// <param name="creator">创建人ID</param>
        void SaveRolePermission(Guid roleId, List<int> categoryIds, Guid creator);

    }
}
