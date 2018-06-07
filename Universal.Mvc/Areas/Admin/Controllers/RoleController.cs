using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Universal.Core;
using Universal.Entities;
using Universal.Entities.Dto;
using Universal.Framework.Controllers.Admin;
using Universal.Framework.Infrastructure;
using Universal.Framework.Menu;
using Universal.Services;

namespace Universal.Mvc.Areas.Admin.Controllers
{
    [Route("admin/role")]
    [Function("角色列表", true, "menu-icon fa fa-caret-right", FatherResource = "Universal.Mvc.Areas.Admin.Controllers.SystemManageController", Sort = 1)]
    public class RoleController : AdminPermissionController
    {
        private ISysRoleService _sysRoleService;
        private IWorkContext _workContext;
        private ICategoryService _categoryService;
        private ISysPermissionService _sysPermissionService;

        public RoleController(ISysRoleService sysRoleService, IWorkContext workContext, ICategoryService categoryService, ISysPermissionService sysPermissionService)
        {
            this._sysRoleService = sysRoleService;
            this._workContext = workContext;
            this._categoryService = categoryService;
            this._sysPermissionService = sysPermissionService;
        }

        /// <summary>
        /// 角色列表
        /// </summary>
        /// <returns></returns>
        [Route("index", Name = "roleIndex")]
        [Function("角色列表", true, "", FatherResource = "Universal.Mvc.Areas.Admin.Controllers.RoleController", Sort = 1)]
        public IActionResult Index()
        {
            return View(_sysRoleService.GetAllRoles());
        }


        /// <summary>
        /// 新增/修改角色
        /// </summary>
        /// <param name="id">角色id</param>
        /// <returns></returns>
        [Route("edit", Name = "editRole")]
        [Function("新增、修改角色", false, FatherResource = "Universal.Mvc.Areas.Admin.Controllers.RoleController.Index")]
        public IActionResult EditRole(Guid? id = null)
        {
            SysRole sysRole = null;
            if (id.HasValue && id.Value != Guid.Empty)
            {
                sysRole = _sysRoleService.GetRole(id.Value);
            }
            return View(sysRole);
        }

        [Route("edit")]
        [HttpPost]
        public IActionResult EditRole(SysRole model)
        {
            ModelState.Remove("Id");
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            model.Name = model.Name.Trim();
            if (model.Id == Guid.Empty)
            {
                model.Id = Guid.NewGuid();
                model.CreationTime = DateTime.Now;
                model.Creator = _workContext.CurrentUser().Id;
                _sysRoleService.InsertRole(model);
            }
            else
            {
                model.ModifiedTime = DateTime.Now;
                model.Modifier = _workContext.CurrentUser().Id;
                _sysRoleService.UpdateRole(model);
            }
            _sysRoleService.SaveChanges();
            return RedirectToAction("index", "role");
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id">角色Id</param>
        /// <returns></returns>
        [Route("delete/{id}", Name = "deleteRole")]
        [Function("删除角色", false, FatherResource = "Universal.Mvc.Areas.Admin.Controllers.RoleController.Index")]
        public IActionResult DeleteRole(Guid id)
        {
            _sysRoleService.DeleteRole(id);
            _sysRoleService.SaveChanges();
            AjaxData.Status = true;
            AjaxData.Message = "角色已删除";
            return Json(AjaxData);
        }

        [Route("permission", Name = "rolePermission")]
        [Function("角色权限设置", false, FatherResource = "Universal.Mvc.Areas.Admin.Controllers.RoleController.Index")]
        public IActionResult RolePermission(Guid id)
        {
            RolePermissionViewModel model = new RolePermissionViewModel();
            model.CategoryList = _categoryService.GetAll();
            var roleList = _sysRoleService.GetAllRoles();
            if (roleList != null && roleList.Any())
            {
                model.SysRole = roleList.FirstOrDefault(o => o.Id == id);
                model.RoleList = roleList;
                model.Permissions = _sysPermissionService.GetByRoleID(id);
            }
            return View(model);
        }

        [HttpPost]
        [Route("permission")]
        public IActionResult RolePermission(Guid id, List<int> sysResource)
        {
            _sysPermissionService.SaveRolePermission(id, sysResource, _workContext.CurrentUser().Id);
            AjaxData.Status = true;
            AjaxData.Message = "角色权限设置成功";
            return Json(AjaxData);
        }


        [Route("permissiontree", Name = "rolePermissionTree")]
        [Function("角色权限设置", false, FatherResource = "Universal.Mvc.Areas.Admin.Controllers.RoleController.Index")]
        public IActionResult RolePermissionTree(Guid id)
        {
            RolePermissionViewModel model = new RolePermissionViewModel();
            model.CategoryList = _categoryService.GetAll();
            var roleList = _sysRoleService.GetAllRoles();
            if (roleList != null && roleList.Any())
            {
                model.SysRole = roleList.FirstOrDefault(o => o.Id == id);
                model.RoleList = roleList;
                model.Permissions = _sysPermissionService.GetByRoleID(id);
                model.TreeList = GetTreeModel(model.CategoryList, model.Permissions);
            }
            return View(model);
        }

        [HttpPost]
        [Route("permissiontree")]
        public IActionResult RolePermissionTree(Guid id, List<int> sysResource)
        {
            _sysPermissionService.SaveRolePermission(id, sysResource, _workContext.CurrentUser().Id);
            AjaxData.Status = true;
            AjaxData.Message = "角色权限设置成功";
            return Json(AjaxData);
        }

        private static List<TreeModel> GetTreeModel(List<Category> categoryList,List<SysPermission> userSysPermissionList)
        {
            List<TreeModel> treeModelListWithOutAll = new List<TreeModel>();
            var fatherList = categoryList.Where(o => o.IsMenu && string.IsNullOrEmpty(o.FatherID)).ToList();
            foreach (var father in fatherList)
            {
                TreeModel fatherTree = new TreeModel()
                {
                    id = father.Id,
                    name = father.Name,
                    isCheck = userSysPermissionList.Exists(o=>o.CategoryId==father.Id),
                    open = true,
                    pId = 0,
                    children = new List<TreeModel>()
                };
                GetChildrenTree(categoryList, father, fatherTree, userSysPermissionList);
                treeModelListWithOutAll.Add(fatherTree);
            }

            List<TreeModel> treeModelListWithAll = new List<TreeModel>();
            treeModelListWithAll.Add(new TreeModel()
            {
                id = 0,
                name = "全选/取消全选",
                children = new List<TreeModel>(),
                isCheck = false,
                 open=true,
                  pId=-1
            });
            treeModelListWithAll[0].children.AddRange(treeModelListWithOutAll);

            return treeModelListWithAll;
        }

        private static void GetChildrenTree(List<Category> categoryList, Category father, TreeModel fatherTree, List<SysPermission> userSysPermissionList)
        {
            var chlidrenlist = categoryList.Where(o => o.FatherID == father.ResouceID).ToList();
            foreach (var child in chlidrenlist)
            {
                TreeModel childTree = new TreeModel()
                {
                    id = child.Id,
                    name = child.Name,
                    isCheck = userSysPermissionList.Exists(o => o.CategoryId == child.Id),
                    open = true,
                    pId = fatherTree.id,
                    children = new List<TreeModel>()
                };
                fatherTree.children.Add(childTree);
                GetChildrenTree(categoryList, child, childTree,userSysPermissionList);
            }
        }

        [Route("exception", Name = "throwException")]
        [Function("抛出异常", true, "", FatherResource = "Universal.Mvc.Areas.Admin.Controllers.RoleController", Sort = 1)]
        public IActionResult ThrowException()
        {
            var rep = EnginContext.Current.Resolve<IRepository<Category>>();

            throw new Exception("人为抛出异常");
        }
    }
}