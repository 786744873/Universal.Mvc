using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Universal.Entities
{
    /// <summary>
    /// 系统用户登录日志
    /// </summary>
    [Table("SysUserLoginLog")]
    public class SysUserLoginLog
    {
        [Key]
        public Guid Id { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// 登录地址
        /// </summary>
        public string IpAddress { get; set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime LoginTime { get; set; }
        /// <summary>
        /// 日志内容
        /// </summary>
        public string Message { get; set; }

        public virtual SysUser SysUser { get; set; }
    }
}
