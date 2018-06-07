using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Universal.Entities
{
    /// <summary>
    /// 系统用户表
    /// </summary>
    [Table("SysUser")]
    public class SysUser
    {
        public SysUser()
        {
            this.SysUserRoles = new HashSet<SysUserRole>();
            this.SysUserTokens = new HashSet<SysUserToken>();
            this.SysUserLoginLogs = new HashSet<SysUserLoginLog>();
        }

        [Key]
        public System.Guid Id { get; set; }
        /// <summary>
        /// 用户账户
        /// </summary>
        [Required(ErrorMessage ="请输入账号，支持5~18位数字、字母组合")]
        [RegularExpression("^[1-9a-zA-Z]{5,18}$", ErrorMessage = "5~18数字、字母组合")]
        public string Account { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = "请输入真实姓名")]
        public string Name { get; set; }
        /// <summary>
        /// 用户邮箱
        /// </summary>
        [RegularExpression(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "邮箱格式错误")]
        public string Email { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        [RegularExpression(@"^1[345678]\d{9}$", ErrorMessage = "请输入11位手机号")]
        public string MobilePhone { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 密码盐值
        /// </summary>
        public string Salt { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [StringLength(2)]
        public string Sex { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }
        /// <summary>
        /// 是否是管理员
        /// </summary>
        public bool IsAdmin { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public System.DateTime CreationTime { get; set; }
        /// <summary>
        /// 登录失败次数
        /// </summary>
        public int LoginFailedNum { get; set; }
        /// <summary>
        /// 允许登录时间
        /// </summary>
        public DateTime? AllowLoginTime { get; set; }
        /// <summary>
        /// 是否被锁定
        /// </summary>
        public bool LoginLock { get; set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }
        /// <summary>
        /// 最后登录IP地址
        /// </summary>
        [StringLength(50)]
        public string LastIpAddress { get; set; }
        /// <summary>
        /// 最后活动时间
        /// </summary>
        public DateTime? LastActivityTime { get; set; }
        /// <summary>
        /// 是否被删除
        /// </summary>
        public bool IsDeleted { get; set; }
        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? DeletedTime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifiedTime { get; set; }
        /// <summary>
        /// 修改人ID
        /// </summary>
        public Guid? Modifier { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        public Guid? Creator { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public byte[] Avatar { get; set; }


        public virtual ICollection<SysUserRole> SysUserRoles { get; set; }

        public virtual ICollection<SysUserToken> SysUserTokens { get; set; }

        public virtual ICollection<SysUserLoginLog> SysUserLoginLogs { get; set; }

    }
}
