using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Universal.Core;

namespace Universal.Entities
{
    public class EFRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private EFDbContext _dbContext;

        public EFRepository(EFDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 数据库上下文
        /// </summary>
        public DbContext DbContext
        {
            get
            {
                return _dbContext;
            }
        }

        /// <summary>
        /// T类型数据库上下文实体对象
        /// </summary>
        public DbSet<TEntity> Entities
        {
            get
            {
                return _dbContext.Set<TEntity>();
            }
        }

        /// <summary>
        /// T类型数据库上下文实体对象
        /// </summary>
        public IQueryable<TEntity> Table
        {
            get {
                return Entities;
            }
        }

        /// <summary>
        /// 根据ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TEntity GetById(object id)
        {
            return Entities.Find(id);
        }

        /// <summary>
        /// 插入。默认自动保存（后期可优化）
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isSave"></param>
        public void Insert(TEntity entity,bool isSave=true)
        {
            Entities.Add(entity);
            if (isSave)
            {
                DbContext.SaveChanges();
            }
        }

        /// <summary>
        /// 更新。默认自动保存（后期可优化）
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isSave"></param>
        public void Update(TEntity entity, bool isSave = true)
        {
            Entities.Update(entity);
            if (isSave)
            {
                DbContext.SaveChanges();
            }
        }

        /// <summary>
        /// 删除。默认自动保存（后期可优化）
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="isSave"></param>
        public void Delete(TEntity entity, bool isSave = true)
        {
            Entities.Remove(entity);
            if (isSave)
            {
                DbContext.SaveChanges();
            }
        }

        #region 补充



        //#region 增加
        ///// <summary>
        ///// 单条增加
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <returns></returns>
        //public virtual int Add(TEntity entity)
        //{
        //    Entities.Add(entity);
        //    return Save();
        //}

        ///// <summary>
        ///// 批量增加
        ///// </summary>
        ///// <param name="entities"></param>
        ///// <returns></returns>
        //public virtual int AddRange(ICollection<TEntity> entities)
        //{
        //    Entities.AddRange(entities);
        //    return Save();
        //}

        ///// <summary>
        ///// 大批量增加
        ///// </summary>
        ///// <param name="entities"></param>
        ///// <param name="destinationTableName"></param>
        //public virtual void BulkInsert(IList<TEntity> entities,string destinationTableName=null)
        //{
        //    DbContext.BulkInsert(entities, destinationTableName);
        //}
        //#endregion

        //#region 删除
        ///// <summary>
        ///// 根据主键删除
        ///// </summary>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //public virtual int Delete(object key)
        //{
        //    var entity = Entities.Find(key);
        //    if (entity == null) return 0;
        //    Entities.Remove(entity);
        //    return Save();
        //}

        ///// <summary>
        ///// 按条件删除
        ///// </summary>
        ///// <param name="expression"></param>
        ///// <returns></returns>
        //public virtual int Delete(Expression<Func<TEntity, bool>> expression)
        //{
        //    Entities.RemoveRange(Entities.Where(expression));
        //    return Save();
        //}
        //#endregion

        //#region 修改
        ///// <summary>
        ///// 更新一条数据
        ///// </summary>
        ///// <param name="entity"></param>
        ///// <returns></returns>
        //public virtual int Update(TEntity entity)
        //{
        //    Entities.Update(entity);
        //    return Save();
        //}

        ///// <summary>
        ///// 批量更新
        ///// </summary>
        ///// <param name="entities"></param>
        ///// <returns></returns>
        //public virtual int Update(ICollection<TEntity> entities)
        //{
        //    Entities.UpdateRange(entities);
        //    return Save();
        //}
        //#endregion

        //#region 查询
        ///// <summary>
        ///// 查询满足条件的总数
        ///// </summary>
        ///// <param name="expression"></param>
        ///// <returns></returns>
        //public virtual int Count(Expression<Func<TEntity, bool>> expression = null)
        //{
        //    return expression == null ? Entities.Count() : Entities.Count(expression);
        //}
        //#endregion

        //#region 保存
        ///// <summary>
        ///// 保存
        ///// </summary>
        ///// <returns></returns>
        //public virtual int Save()
        //{
        //    return this.DbContext.SaveChanges();
        //}
        //#endregion

        #endregion
    }
}
