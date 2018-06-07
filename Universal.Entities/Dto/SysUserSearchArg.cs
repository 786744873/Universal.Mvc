using System;
using System.Collections.Generic;
using System.Text;

namespace Universal.Entities.Dto
{
    /// <summary>
    /// 查询参数
    /// </summary>
    public class SysUserSearchArg
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string keyWord { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool? enabled { get; set; }

        /// <summary>
        /// 是否登录锁
        /// </summary>
        public bool? unlock { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public Guid? suid { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public Guid? roleId { get; set; }
    }
}
