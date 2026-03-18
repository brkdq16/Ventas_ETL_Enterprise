using System.Data;
using Microsoft.Data.SqlClient;
using Ventas.Core.Domain.Entities;

namespace Ventas.Infrastructure.Data.Repositories;

public class ProductRepository
{
    // Carga masiva de productos con integridad referencial (CategoryID)
    public void BulkInsert(IEnumerable<Product> products, SqlConnection conn, SqlTransaction trans)
    {
        using var bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, trans);
        bulkCopy.DestinationTableName = "Products";

        bulkCopy.ColumnMappings.Add(nameof(Product.ProductID), "ProductID");
        bulkCopy.ColumnMappings.Add(nameof(Product.ProductName), "ProductName");
        bulkCopy.ColumnMappings.Add(nameof(Product.CategoryID), "CategoryID");
        bulkCopy.ColumnMappings.Add(nameof(Product.Price), "Price");
        bulkCopy.ColumnMappings.Add(nameof(Product.Stock), "Stock");

        var table = new DataTable();
        table.Columns.Add("ProductID", typeof(int));
        table.Columns.Add("ProductName", typeof(string));
        table.Columns.Add("CategoryID", typeof(int));
        table.Columns.Add("Price", typeof(decimal));
        table.Columns.Add("Stock", typeof(int));

        foreach (var p in products)
            table.Rows.Add(p.ProductID, p.ProductName, p.CategoryID, p.Price, p.Stock);

        bulkCopy.WriteToServer(table);
    }
}
