using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Universal.Entities
{
    /// <summary>
    /// 系统日志
    /// </summary>
    [Table("SysLog")]
    public class SysLog
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public int? Level { get; set; }
        /// <summary>
        /// 内容简介
        /// </summary>
        public string ShortMessage { get; set; }
        /// <summary>
        /// 完整内容
        /// </summary>
        public string FullMessage { get; set; }
        /// <summary>
        /// IP地址
        /// </summary>
        public string IpAddress { get; set; }
        /// <summary>
        /// 目标url
        /// </summary>
        public string PageUrl { get; set; }
        /// <summary>
        /// 源url
        /// </summary>
        public string ReferrerUrl { get; set; }
        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime? CreationTime { get; set; }
    }
}
