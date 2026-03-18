using System.Data;
using Microsoft.Data.SqlClient;
using Ventas.Core.Domain.Entities;

namespace Ventas.Infrastructure.Data.Repositories;

public class OrderRepository
{
    // Inserta órdenes asegurando la integridad del SourceID (Auditoría)
    public void BulkInsert(IEnumerable<Order> orders, SqlConnection conn, SqlTransaction trans)
    {
        using var bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, trans);
        bulkCopy.DestinationTableName = "Orders";

        bulkCopy.ColumnMappings.Add(nameof(Order.OrderID), "OrderID");
        bulkCopy.ColumnMappings.Add(nameof(Order.OrderDate), "OrderDate");
        bulkCopy.ColumnMappings.Add(nameof(Order.Status), "Status");
        bulkCopy.ColumnMappings.Add(nameof(Order.CustomerID), "CustomerID");
        bulkCopy.ColumnMappings.Add(nameof(Order.SourceID), "SourceID");

        var table = new DataTable();
        table.Columns.Add("OrderID", typeof(int));
        table.Columns.Add("OrderDate", typeof(DateTime));
        table.Columns.Add("Status", typeof(string));
        table.Columns.Add("CustomerID", typeof(int));
        table.Columns.Add("SourceID", typeof(int));

        foreach (var o in orders)
            table.Rows.Add(o.OrderID, o.OrderDate, o.Status, o.CustomerID, o.SourceID);

        bulkCopy.WriteToServer(table);
    }
}

