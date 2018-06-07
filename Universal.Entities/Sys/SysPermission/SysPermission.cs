using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Universal.Entities
{
    /// <summary>
    /// 系统角色权限表
    /// </summary>
    [Table("SysPermission")]
    public partial class SysPermission
    {
        public virtual Guid Id { get; set; }
        /// <summary>
        /// 菜单ID
        /// </summary>
        public virtual int CategoryId { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public virtual Guid RoleId { get; set; }
        /// <summary>
        /// 角色权限创建人ID
        /// </summary>
        public virtual Guid Creator { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreationTime { get; set; }


        public virtual Category Category { get; set; }
        public virtual SysRole SysRole { get; set; }
        public virtual SysUser SysUser { get; set; }
    }
}
