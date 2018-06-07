using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Universal.Mvc.Areas.Admin.Models
{
    public class LoginModel
    {
        /// <summary>
        /// 账号
        /// </summary>
        [Required(ErrorMessage ="请输入账号")]
        public string Account { get; set; }


        /// <summary>
        /// 账号
        /// </summary>
        [Required(ErrorMessage ="请输入密码")]
        //[MinLength(6,ErrorMessage ="密码最短为6位")]
        //[MaxLength(32,ErrorMessage ="密码最长为32位")]
        public string Password { get; set; }

        /// <summary>
        /// 随机值
        /// </summary>
        public string R { get; set; }
    }
}
