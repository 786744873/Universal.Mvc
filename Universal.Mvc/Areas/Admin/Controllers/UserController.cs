using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Universal.Entities.Dto;
using Universal.Framework.Controllers.Admin;
using Universal.Framework.Menu;
using Universal.Services;
using Universal.Core;
using Universal.Entities;
using Universal.Core.Helpers;
using Universal.Framework.Infrastructure;
using Universal.Framework;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Universal.Mvc.Areas.Admin.Controllers
{
    [Route("admin/users")]
    [Function("用户列表", true, "menu-icon fa fa-caret-right", FatherResource = "Universal.Mvc.Areas.Admin.Controllers.SystemManageController", Sort = 1)]
    public class UserController : AdminPermissionController
    {
        private ISysUserService _sysUserService;
        private IWorkContext _workContext;
        private ISysUserRoleService _sysUserRoleService;
        private ISysRoleService _sysRoleService;

        public UserController(ISysUserService sysUserService, IWorkContext workContext, ISysUserRoleService sysUserRoleService, ISysRoleService sysRoleService)
        {
            this._sysUserService = sysUserService;
            this._workContext = workContext;
            this._sysUserRoleService = sysUserRoleService;
            this._sysRoleService = sysRoleService;
        }


        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        [Route("index", Name = "userIndex")]
        [Function("用户列表", true, "", FatherResource = "Universal.Mvc.Areas.Admin.Controllers.UserController", Sort = 0)]
        public IActionResult UserIndex(SysUserSearchArg arg, int page = 1, int size = 20)
        {
            var pageList = _sysUserService.SearchUser(arg, page, size);
            var dataSource = pageList.ToDataSourceResult<SysUser, SysUserSearchArg>("userIndex", arg);
            return View(dataSource);
        }

        /// <summary>
        /// 新增或者修改用户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("edit", Name = "editUser")]
        [Function("编辑用户", false, FatherResource = "Universal.Mvc.Areas.Admin.Controllers.UserController.UserIndex")]
        public IActionResult EditUser(Guid? id, string returnUrl = null)
        {
            ViewBag.ReturnUrl = Url.IsLocalUrl(returnUrl) ? returnUrl : Url.RouteUrl("userIndex");
            ViewBag.SysRoleList = _sysRoleService.GetAllRoles().Select(o => new SelectListItem() { Value = o.Id.ToString(), Text = o.Name }).ToList();
            if (id != null)
            {
                var model = _sysUserService.GetById(id.Value);
                if (model == null)
                {
                    return Redirect(ViewBag.ReturnUrl);
                }
                var sysUserRole = _sysUserRoleService.GetAll().FirstOrDefault(o => o.UserId == id);
                if (!model.IsAdmin)
                {
                    ViewBag.SysRoleList = _sysRoleService.GetAllRoles().Select(o => new SelectListItem() { Value = o.Id.ToString(), Text = o.Name, Selected = (o.Id == sysUserRole.RoleId) }).ToList();
                }
                return View(model);
            }
            return View();
        }

        /// <summary>
        /// 新增或者修改用户
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("edit", Name = "editUser")]
        public IActionResult EditUser(SysUser model, string sysRole, string returnUrl = null)
        {
            ViewBag.ReturnUrl = Url.IsLocalUrl(returnUrl) ? returnUrl : Url.RouteUrl("userIndex");
            ViewBag.SysRoleList = _sysRoleService.GetAllRoles().Select(o => new SelectListItem() { Value = o.Id.ToString(), Text = o.Name,Selected=(o.Id.ToString()==sysRole) }).ToList();
            ModelState.Remove("Id");
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (!string.IsNullOrEmpty(model.MobilePhone))
            {
                model.MobilePhone = StringUitls.toDBC(model.MobilePhone);
            }
            model.Name = model.Name.Trim();
            if (model.Id == Guid.Empty)
            {
                model.Id = Guid.NewGuid();
                model.CreationTime = DateTime.Now;
                model.Salt = EncryptorHelper.CreateSaltKey();
                model.Account = StringUitls.toDBC(model.Account);
                model.Enabled = true;
                model.IsAdmin = false;
                model.Password = EncryptorHelper.GetMD5(model.Account + model.Salt);
                model.Creator = _workContext.CurrentUser().Id;
                _sysUserService.InsertSysUser(model);
            }
            else
            {
                model.ModifiedTime = DateTime.Now;
                model.Modifier = _workContext.CurrentUser().Id;
                _sysUserService.UpdateSysUser(model);
            }
            if (!string.IsNullOrEmpty(sysRole))
            {
                _sysUserRoleService.InsertOrUpdateSysUserRole(model.Id,new Guid(sysRole));
            }
            return Redirect(ViewBag.ReturnUrl);
        }

        /// <summary>
        /// 设置启用或禁用账号
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="enabled">启用（true）/禁用（faslse）</param>
        /// <returns></returns>
        [Route("enabled", Name = "enabled")]
        [Function("设置启用或者禁用账号", false, FatherResource = "Universal.Mvc.Areas.Admin.Controllers.UserController.UserIndex")]
        public IActionResult Enabled(Guid id, bool enabled)
        {
            _sysUserService.Enabled(id, enabled, _workContext.CurrentUser().Id);
            AjaxData.Status = true;
            AjaxData.Message = "启用/禁用设置完成";
            return Json(AjaxData);
        }


        [Route("loginLock", Name = "loginLock")]
        [Function("设置登录锁解锁与锁定", false, FatherResource = "Universal.Mvc.Areas.Admin.Controllers.UserController.UserIndex")]
        public IActionResult LoginLock(Guid id, bool loginLock)
        {
            _sysUserService.LoginLock(id, loginLock, _workContext.CurrentUser().Id);
            AjaxData.Status = true;
            AjaxData.Message = "登录锁状态设置完成";
            return Json(AjaxData);
        }

        [Route("delete/{id}", Name = "deleteUser")]
        [Function("删除用户", false, FatherResource = "Universal.Mvc.Areas.Admin.Controllers.UserController.UserIndex")]
        public IActionResult DeleteUser(Guid id)
        {
            _sysUserService.DeleteSysUser(id, _workContext.CurrentUser().Id);
            AjaxData.Status = true;
            AjaxData.Message = "删除完成";
            return Json(AjaxData);
        }

        [Route("existAccount", Name = "remoteAccount")]
        [PermissionActionFilter(false)]
        public IActionResult RemoteAccount(string account)
        {
            account = account.Trim();
            return Json(_sysUserService.ExistAccount(account));
        }

        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("resetPwd/{id}", Name = "reSetPassWord")]
        [Function("重置用户密码", false, FatherResource = "Universal.Mvc.Areas.Admin.Controllers.UserController.UserIndex")]
        public IActionResult ReSetPassWord(Guid id)
        {
            _sysUserService.ResetPassword(id, _workContext.CurrentUser().Id);
            AjaxData.Status = true;
            AjaxData.Message = "用户密码已重置为原始密码";
            return Json(AjaxData);
        }
    }
}