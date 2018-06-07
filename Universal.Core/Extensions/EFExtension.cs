using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Universal.Core
{
    public static class EFExtension
    {
        #region EF扩展


        #region 插入
        /// <summary>
        /// 大批量插入
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <param name="destinationTableName"></param>
        public static void BulkInsert<T>(this DbContext dbContext, IList<T> entities, string destinationTableName = null) where T : class
        {
            if (entities == null || !entities.Any()) return;
            if (string.IsNullOrEmpty(destinationTableName))
            {
                var mappingTableName = typeof(T).GetCustomAttribute<TableAttribute>()?.Name;
                destinationTableName = string.IsNullOrEmpty(mappingTableName) ? typeof(T).Name : mappingTableName;
            }
            using (var dt = entities.ToDataTable())
            {
                using (var conn = new SqlConnection(dbContext.Database.GetDbConnection().ConnectionString))
                {
                    conn.Open();
                    using (var tran = conn.BeginTransaction())
                    {
                        try
                        {
                            var bulk = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, tran);
                            bulk.BatchSize = entities.Count;
                            bulk.DestinationTableName = destinationTableName;
                            bulk.EnableStreaming = true;
                            bulk.WriteToServerAsync(dt);
                            tran.Commit();
                        }
                        catch (Exception)
                        {
                            tran.Rollback();
                            throw;
                        }
                    }
                    conn.Close();
                }
            }
        }
        #endregion

        #region 删除

        #endregion

        #endregion
    }
}
