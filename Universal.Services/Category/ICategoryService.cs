using System;
using System.Collections.Generic;
using System.Text;
using Universal.Core;
using Universal.Entities;
using Universal.Entities.Dto;

namespace Universal.Services
{
    public interface ICategoryService
    {
        /// <summary>
        /// 初始化菜单
        /// </summary>
        /// <param name="list"></param>
        void InitCategory(List<Category> list);

        /// <summary>
        /// 获取所有菜单并缓存
        /// </summary>
        /// <returns></returns>
        List<Category> GetAll();

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="arg"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        IPagedList<Category> SearchUser(CategorySearchArg arg, int page, int size);
    }
}
