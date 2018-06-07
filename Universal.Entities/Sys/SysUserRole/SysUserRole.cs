using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Universal.Entities
{
    /// <summary>
    /// 系统用户角色表
    /// </summary>
    [Table("SysUserRole")]
    public class SysUserRole
    {
        public virtual Guid Id { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public virtual Guid RoleId { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public virtual Guid UserId { get; set; }


        public virtual SysRole SysRole { get; set; }
        public virtual SysUser SysUser { get; set; }
    }
}
