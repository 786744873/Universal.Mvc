using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Universal.Core
{
    public interface IRepository<TEntity> where TEntity:class
    {
        /// <summary>
        /// 数据库上下文
        /// </summary>
        DbContext DbContext { get; }

        /// <summary>
        /// T类型数据库上下文实体对象
        /// </summary>
        DbSet<TEntity> Entities { get; }

        /// <summary>
        /// T类型数据库上下文实体对象
        /// </summary>
        IQueryable<TEntity> Table { get; }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity GetById(object id);

        /// <summary>
        /// 插入。默认自动保存（后期可优化）
        /// </summary>
        /// <param name="entity">T</param>
        /// <param name="isSave">是否立即保存  默认true</param>
        void Insert(TEntity entity, bool isSave = true);
        /// <summary>
        /// 更新。默认自动保存（后期可优化）
        /// </summary>
        /// <param name="entity">T</param>
        /// <param name="isSave">是否立即保存  默认true</param>
        void Update(TEntity entity, bool isSave = true);
        /// <summary>
        /// 删除。默认自动保存（后期可优化）
        /// </summary>
        /// <param name="entity">T</param>
        /// <param name="isSave">是否立即保存  默认true</param>
        void Delete(TEntity entity, bool isSave = true);
    }
}
