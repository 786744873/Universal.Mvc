using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Universal.Entities
{
    /// <summary>
    /// 系统菜单表
    /// </summary>
    [Table("Category")]
    public class Category
    {
        public int Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 是否是菜单
        /// </summary>
        public bool IsMenu { get; set; }
        /// <summary>
        /// 资源标识
        /// </summary>
        public string SysResource { get; set; }
        /// <summary>
        /// 资源标识简写
        /// </summary>
        public string ResouceID { get; set; }
        /// <summary>
        /// 父菜单资源
        /// </summary>
        public string FatherResource { get; set; }
        /// <summary>
        /// 父菜单ID
        /// </summary>
        public string FatherID { get; set; }
        /// <summary>
        /// 控制器名称
        /// </summary>
        public string Controller { get; set; }
        /// <summary>
        /// Action名称
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// 路由名称
        /// </summary>
        public string RouteName { get; set; }
        /// <summary>
        /// 图标样式
        /// </summary>
        public string CssClass { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsDisabled { get; set; }
    }
}
