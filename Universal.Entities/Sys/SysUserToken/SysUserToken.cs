using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Universal.Entities
{
    /// <summary>
    /// 系统用户Token
    /// </summary>
    [Table("SysUserToken")]
    public class SysUserToken
    {
        [Key]
        public Guid Id { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid SysUserId { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpireTime { get; set; }


        public virtual SysUser SysUser { get; set; }
    }
}
