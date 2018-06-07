using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Universal.Framework.Controllers
{
    public class BaseContoller : Controller
    {
        private AjaxResult _ajaxResult;

        public BaseContoller()
        {
            _ajaxResult = new AjaxResult();
        }

        /// <summary>
        /// Ajax请求结果
        /// </summary>
        public AjaxResult AjaxData {
            get {
                return _ajaxResult;
            }
        }
    }
}
