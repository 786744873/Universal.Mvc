using System;
using System.Collections.Generic;
using System.Text;
using Universal.Core;

namespace Universal.Entities.Dto
{
    /// <summary>
    /// 角色权限视图模型
    /// </summary>
    public class RolePermissionViewModel
    {
        public SysRole SysRole { get; set; }

        /// <summary>
        /// 角色select下拉菜单
        /// </summary>
        public List<SysRole> RoleList { get; set; }

        /// <summary>
        /// 角色的权限数据
        /// </summary>
        public List<SysPermission> Permissions { get; set; }

        /// <summary>
        /// 菜单列表
        /// </summary>
        public List<Category> CategoryList { get; set; }

        /// <summary>
        /// 树菜单
        /// </summary>
        public List<TreeModel> TreeList { get; set; }
    }
}
