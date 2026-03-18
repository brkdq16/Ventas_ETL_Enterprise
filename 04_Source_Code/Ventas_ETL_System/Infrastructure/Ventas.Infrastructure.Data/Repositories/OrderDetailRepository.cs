using System.Data;
using Microsoft.Data.SqlClient;
using Ventas.Core.Domain.Entities;

namespace Ventas.Infrastructure.Data.Repositories;

public class OrderDetailRepository
{
    public void BulkInsert(IEnumerable<OrderDetail> details, SqlConnection conn, SqlTransaction trans)
    {
        using var bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, trans);
        bulkCopy.DestinationTableName = "OrderDetails";

        // ENVIAMOS SOLO LOS "INGREDIENTES"
        bulkCopy.ColumnMappings.Add(nameof(OrderDetail.OrderDetailID), "OrderDetailID");
        bulkCopy.ColumnMappings.Add(nameof(OrderDetail.OrderID), "OrderID");
        bulkCopy.ColumnMappings.Add(nameof(OrderDetail.ProductID), "ProductID");
        bulkCopy.ColumnMappings.Add(nameof(OrderDetail.Quantity), "Quantity");
        bulkCopy.ColumnMappings.Add(nameof(OrderDetail.UnitPrice), "UnitPrice");
        // ELIMINAMOS EL MAPEÓ DE LINETOTAL AQUÍ

        var table = new DataTable();
        table.Columns.Add("OrderDetailID", typeof(int));
        table.Columns.Add("OrderID", typeof(int));
        table.Columns.Add("ProductID", typeof(int));
        table.Columns.Add("Quantity", typeof(int));
        table.Columns.Add("UnitPrice", typeof(decimal));

        foreach (var d in details)
        {
            // Solo agregamos estos 5 campos al DataTable
            table.Rows.Add(d.OrderDetailID, d.OrderID, d.ProductID, d.Quantity, d.UnitPrice);
        }

        bulkCopy.WriteToServer(table);
    }


}
