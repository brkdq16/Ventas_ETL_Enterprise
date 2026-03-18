using System.Data;
using Microsoft.Data.SqlClient;
using Ventas.Core.Domain.Entities;

namespace Ventas.Infrastructure.Data.Repositories;

public class CategoryRepository
{
    // Inserta las categorías únicas descubiertas en el pipeline
    public void BulkInsert(IEnumerable<Category> categories, SqlConnection conn, SqlTransaction trans)
    {
        using var bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, trans);
        bulkCopy.DestinationTableName = "Categories";

        bulkCopy.ColumnMappings.Add(nameof(Category.CategoryID), "CategoryID");
        bulkCopy.ColumnMappings.Add(nameof(Category.CategoryName), "CategoryName");

        var table = new DataTable();
        table.Columns.Add("CategoryID", typeof(int));
        table.Columns.Add("CategoryName", typeof(string));

        foreach (var c in categories)
            table.Rows.Add(c.CategoryID, c.CategoryName);

        bulkCopy.WriteToServer(table);
    }
}

