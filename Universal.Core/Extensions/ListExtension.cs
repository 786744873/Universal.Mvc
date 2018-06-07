using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace Universal.Core
{
    public static class ListExtension
    {
        public static DataTable ToDataTable<T>(this IEnumerable<T> source)
        {
            DataTable dtReturn = new DataTable();

            // column names 
            PropertyInfo[] oProps = null;

            if (source == null) return dtReturn;

            foreach (var rec in source)
            {
                // Use reflection to get property names, to create table, Only first time, others will follow 
                if (oProps == null)
                {
                    oProps = rec.GetType().GetProperties();
                    foreach (var pi in oProps)
                    {
                        var colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                var dr = dtReturn.NewRow();

                foreach (var pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null
                        ? DBNull.Value
                        : pi.GetValue
                            (rec, null);
                }

                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }
    }
}
