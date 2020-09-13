using DataSynchronizer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DataSynchronizer.Infra.Utils
{
    public static class Extension
    {
        private static IEnumerable<string> GetColumnsNames(this DataColumnCollection columnCollection)
        {
            for (int i = 0; i < columnCollection.Count; i++)
                yield return columnCollection[i].ColumnName;
        }

        public static ColumnsModel GetColumnsModel(this DataTable dataTable)
        {
            int columnFilter = dataTable.Columns.Cast<DataColumn>()
                 .Where(column => column.ColumnName.ToUpper() == "ID")
                 .Select(column => column.Ordinal).FirstOrDefault();

            var columns = dataTable.Columns.GetColumnsNames().ToList();
            columns.RemoveAt(columnFilter);

            var values = dataTable.Rows[0].ItemArray.ToList();
            values.RemoveAt(columnFilter);

            return new ColumnsModel
            {
                Names = columns,
                Values = values,
            };
        }
    }
}
