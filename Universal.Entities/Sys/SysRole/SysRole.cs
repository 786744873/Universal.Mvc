using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Universal.Entities
{
    /// <summary>
    /// 系统角色表
    /// </summary>
    [Table("SysRole")]
    public class SysRole
    {
        public SysRole()
        {
            this.SysPermission = new HashSet<SysPermission>();
            this.SysUserRole = new HashSet<SysUserRole>();
        }

        [Key]
        public Guid Id { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        [Required(ErrorMessage ="请输入角色名")]
        [StringLength(500)]
        public string Name { get; set; }
        /// <summary>
        /// 角色创建人ID
        /// </summary>
        public Guid Creator { get; set; }
        /// <summary>
        /// 角色创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }
        /// <summary>
        /// 角色修改人ID
        /// </summary>
        public Guid? Modifier { get; set; }
        /// <summary>
        /// 角色修改时间
        /// </summary>
        public DateTime? ModifiedTime { get; set; }


        public virtual ICollection<SysUserRole> SysUserRole { get; set; }

        public virtual ICollection<SysPermission> SysPermission { get; set; }
    }
}
