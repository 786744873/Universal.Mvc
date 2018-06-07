using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Universal.Core;
using Universal.Framework.Controllers.Admin;
using Universal.Framework.Security.Admin;
using Universal.Mvc.Areas.Admin.Models;
using Universal.Services;

namespace Universal.Mvc.Areas.Admin.Controllers
{
    [Route("admin")]
    public class LoginController : AdminAreaController
    {
        private const string Login_Key = "Login_key";

        private IMemoryCache _memoryCache;
        private ISysUserService _sysUserService;
        private IAdminAuthService _adminAuthService;

        public LoginController(IMemoryCache memoryCache, ISysUserService sysUserService, IAdminAuthService adminAuthService)
        {
            this._memoryCache = memoryCache;
            this._sysUserService = sysUserService;
            this._adminAuthService = adminAuthService;
        }



        [Route("login",Name ="adminLogin")]
        [HttpGet]
        public IActionResult LoginIndex()
        {
            string r = EncryptorHelper.GetMD5(Guid.NewGuid().ToString());
            HttpContext.Session.SetString(Login_Key, r);
            LoginModel loginModel = new LoginModel { R = r };
            return View(loginModel);
        }

        [HttpPost]
        [Route("login")]
        public IActionResult LoginIndex(LoginModel model)
        {
            string r = HttpContext.Session.GetString(Login_Key);
            r = r ?? "";
            if (!ModelState.IsValid)
            {
                AjaxData.Message = "请输入用户账号和密码";
                return Json(AjaxData);
            }

            var result = _sysUserService.ValidateUser(model.Account, model.Password, r);
            AjaxData.Status = result.Status;
            AjaxData.Message = result.Message;
            if (result.Status)
            {
                _adminAuthService.SignIn(result.Token, result.User.Name);
            }
            return Json(AjaxData);
        }

        [Route("getsalt")]
        [HttpGet]
        public IActionResult GetSalt(string account)
        {
            var user = _sysUserService.GetByAccount(account);
            return Json(user?.Salt);
        }
    }
}