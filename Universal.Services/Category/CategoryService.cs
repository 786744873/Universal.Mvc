using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Universal.Core;
using Universal.Entities;
using Universal.Entities.Dto;

namespace Universal.Services
{
    public class CategoryService : ICategoryService
    {
        private const string MODEL_KEY = "Universal.Services.Category";

        private IMemoryCache _memoryCache;
        private IRepository<Category> _categoryRepository;
        private IRepository<SysPermission> _sysPermissionRepository;

        public CategoryService(IMemoryCache memoryCache, IRepository<Category> categoryRepository, IRepository<SysPermission> sysPermissionRepository)
        {
            this._memoryCache = memoryCache;
            this._categoryRepository = categoryRepository;
            this._sysPermissionRepository = sysPermissionRepository;
        }

        /// <summary>
        /// 初始化菜单
        /// </summary>
        /// <param name="list">新菜单list</param>
        public void InitCategory(List<Category> list)
        {
            //获取旧的菜单
            var oldList = this._categoryRepository.Table.ToList();
            oldList.ForEach(del =>
            {
                /*
                 * 先清理数据库已有部分
                 * （list中不存在，数据库中有该菜单，那么把该菜单的所有权限删除）
                 * （list中存在，数据库中有该菜单,不处理）
                 */
                var item = list.FirstOrDefault(o => o.SysResource == del.SysResource);
                if (item==null)
                {
                    var permissionList = _sysPermissionRepository.Entities.Where(o => o.CategoryId == del.Id).ToList();
                    permissionList.ForEach(delPrm =>
                    {
                        _sysPermissionRepository.Entities.Remove(delPrm);
                    });
                    _categoryRepository.Entities.Remove(del);
                }
            });

            list.ForEach(entity =>
            {
                /*
                 * 新增新菜单，更新已存在的菜单。
                 */
                var item = oldList.FirstOrDefault(o => o.SysResource == entity.SysResource);
                if (item == null)
                {
                    _categoryRepository.Entities.Add(entity);
                }
                else
                {
                    item.Action = entity.Action;
                    item.Controller = entity.Controller;
                    item.CssClass = entity.CssClass;
                    item.FatherResource = entity.FatherResource;
                    item.IsMenu = entity.IsMenu;
                    item.Name = entity.Name;
                    item.RouteName = entity.RouteName;
                    item.SysResource = entity.SysResource;
                    item.Sort = entity.Sort;
                    item.FatherID = entity.FatherID;
                    item.ResouceID = entity.ResouceID;
                }
            });
            if (_categoryRepository.DbContext.ChangeTracker.HasChanges())
            {
                _categoryRepository.DbContext.SaveChanges();
            }
        }

        /// <summary>
        /// 获取所有菜单并缓存
        /// </summary>
        /// <returns></returns>
        public List<Category> GetAll()
        {
            List<Category> list = null;
            _memoryCache.TryGetValue(MODEL_KEY, out list);
            if (list!=null)
            {
                return list;
            }
            list= this._categoryRepository.Table.ToList();
            _memoryCache.Set(MODEL_KEY, list,DateTime.Now.AddDays(1));
            return list;
        }

        public IPagedList<Category> SearchUser(CategorySearchArg arg, int page, int size)
        {
            var query = _categoryRepository.Table;
            if (arg!=null)
            {
                if (!string.IsNullOrEmpty(arg.Name))
                {
                    query = query.Where(o => o.Name.Contains(arg.Name));
                }
                if (arg.IsMenu.HasValue)
                {
                    query = query.Where(o => o.IsMenu==true);
                }
                if (!string.IsNullOrEmpty(arg.RouteName))
                {
                    query = query.Where(o => o.RouteName.Contains(arg.RouteName));
                }
                if (arg.IsDisabled.HasValue)
                {
                    query = query.Where(o => o.IsDisabled==true);
                }
            }
            query = query.OrderBy(o => o.Sort).ThenBy(o => o.RouteName).ThenBy(o => o.Controller).ThenBy(o => o.Action);
            return new PagedList<Category>(query, page, size);

        }
    }
}
