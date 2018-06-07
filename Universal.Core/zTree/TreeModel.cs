using System;
using System.Collections.Generic;
using System.Text;

namespace Universal.Core
{
    public class TreeModel
    {
        public int id { get; set; }
        /// <summary>
        /// 权限名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 上级ID
        /// </summary>
        public int pId { get; set; }
        /// <summary>
        /// 子节点
        /// </summary>
        public List<TreeModel> children { get; set; }
        /// <summary>
        /// 是否有复选框
        /// </summary>
        
        public bool isCheck { get; set; }
        /// <summary>
        /// 是否展开节点
        /// </summary>
        public bool open { get; set; }
    }
}
