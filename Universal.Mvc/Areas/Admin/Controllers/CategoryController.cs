using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Universal.Core;
using Universal.Entities;
using Universal.Entities.Dto;
using Universal.Framework.Controllers.Admin;
using Universal.Framework.Menu;
using Universal.Services;

namespace Universal.Mvc.Areas.Admin.Controllers
{
    [Route("admin/category")]
    public class CategoryController : AdminPermissionController
    {
        private ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [Route("index",Name ="categoryIndex")]
        [Function("菜单列表",true, "menu-icon fa fa-caret-right",FatherResource = "Universal.Mvc.Areas.Admin.Controllers.SystemManageController", Sort = 0)]
        public IActionResult Index(CategorySearchArg arg,int page = 1, int size = 20)
        {
            var pageList = _categoryService.SearchUser(arg, page, size);
            var dataSource = pageList.ToDataSourceResult<Category, CategorySearchArg>("categoryIndex", arg);
            return View(dataSource);
        }


    }
}