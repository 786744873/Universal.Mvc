using System;
using System.Collections.Generic;
using System.Text;
using Universal.Entities;

namespace Universal.Services
{
    public interface  ISysUserRoleService
    {
        /// <summary>
        /// 获取所有的用户角色数据
        /// (缓存)
        /// </summary>
        /// <returns></returns>
        List<SysUserRole> GetAll();

        /// <summary>
        /// 移除缓存
        /// </summary>
        void RemoveCahce();

        /// <summary>
        /// 新增一条用户权限数据
        /// </summary>
        /// <param name="sysUserId"></param>
        /// <param name="sysRoleId"></param>
        void InsertOrUpdateSysUserRole(Guid sysUserId,Guid sysRoleId);

        /// <summary>
        /// 判断权限是否已经存在
        /// </summary>
        /// <param name="sysUserId"></param>
        /// <param name="sysRoleId"></param>
        /// <returns></returns>
        bool Exist(Guid sysUserId);
    }
}
