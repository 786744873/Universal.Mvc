using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Universal.Entities;
using Universal.Services;

namespace Universal.Framework.Menu
{
    public class RegisterMenuService : IRegisterMenuService
    {
        private ICategoryService _categoryService;
        public RegisterMenuService(ICategoryService categoryService)
        {
            this._categoryService = categoryService;
        }

        /// <summary>
        ///  初始化注册菜单
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void InitMenuRegister()
        {
            List<Category> list = new List<Category>();
            FunctionManager.GetFunctionLists().ForEach(item =>
            {
                list.Add(new Category
                {
                    Action = item.Action,
                    Controller = item.Controller,
                    CssClass = item.CssClass,
                    FatherResource = item.FatherResource,
                    IsMenu = item.IsMenu,
                    Name = item.Name,
                    RouteName = item.RouteName,
                    SysResource = item.SysResource,
                    Sort = item.Sort,
                    FatherID = item.FatherID,
                    IsDisabled = false,
                    ResouceID = item.ResouceID
                });
            });
           _categoryService.InitCategory(list);

        }
    }
}
