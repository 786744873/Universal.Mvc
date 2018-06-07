using System;
using System.Collections.Generic;
using System.Text;
using Universal.Entities;

namespace Universal.Services
{
    public interface ISysRoleService
    {
        /// <summary>
        /// 获取所有角色并缓存
        /// </summary>
        List<SysRole> GetAllRoles();

        /// <summary>
        /// 获取角色详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        SysRole GetRole(Guid id);

        /// <summary>
        /// 新增角色并清除相关缓存
        /// </summary>
        /// <param name="sysRole"></param>
        void InsertRole(SysRole sysRole);

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="sysRole"></param>
        void DeleteRole(Guid roleId);


        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="sysRole"></param>
        void UpdateRole(SysRole sysRole);

        /// <summary>
        /// 保存
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// 是否存在数据
        /// </summary>
        /// <returns></returns>
        bool ExistRole();

    }
}
