using System;
using System.Collections.Generic;
using System.Text;

namespace Universal.Entities.Dto
{
    public class CategorySearchArg
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 是否是菜单
        /// </summary>
        public bool? IsMenu { get; set; }
        /// <summary>
        /// 路由名称
        /// </summary>
        public string RouteName { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool? IsDisabled { get; set; }
    }
}
