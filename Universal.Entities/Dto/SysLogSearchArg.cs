using System;
using System.Collections.Generic;
using System.Text;

namespace Universal.Entities.Dto
{
    public class SysLogSearchArg
    {
        /// <summary>
        /// 等级
        /// </summary>
        public int? Level { get; set; }
        /// <summary>
        /// 内容简介
        /// </summary>
        public string ShortMessage { get; set; }
    }
}
