using System.Data;
using Microsoft.Data.SqlClient;
using Ventas.Core.Domain.Entities;

namespace Ventas.Infrastructure.Data.Repositories;

public class CustomerRepository
{
    // Carga masiva de clientes vinculada a la transacción activa
    public void BulkInsert(IEnumerable<Customer> customers, SqlConnection conn, SqlTransaction trans)
    {
        using var bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.Default, trans);
        bulkCopy.DestinationTableName = "Customers";

        // Mapeo: Propiedad C# -> Columna SQL
        bulkCopy.ColumnMappings.Add(nameof(Customer.CustomerID), "CustomerID");
        bulkCopy.ColumnMappings.Add(nameof(Customer.FirstName), "FirstName");
        bulkCopy.ColumnMappings.Add(nameof(Customer.LastName), "LastName");
        bulkCopy.ColumnMappings.Add(nameof(Customer.Email), "Email");
        bulkCopy.ColumnMappings.Add(nameof(Customer.Phone), "Phone");
        bulkCopy.ColumnMappings.Add(nameof(Customer.City), "City");
        bulkCopy.ColumnMappings.Add(nameof(Customer.Country), "Country");

        var table = CreateDataTable(customers);
        bulkCopy.WriteToServer(table);
    }

    private DataTable CreateDataTable(IEnumerable<Customer> customers)
    {
        var table = new DataTable();
        table.Columns.Add("CustomerID", typeof(int));
        table.Columns.Add("FirstName", typeof(string));
        table.Columns.Add("LastName", typeof(string));
        table.Columns.Add("Email", typeof(string));
        table.Columns.Add("Phone", typeof(string));
        table.Columns.Add("City", typeof(string));
        table.Columns.Add("Country", typeof(string));

        foreach (var c in customers)
            table.Rows.Add(c.CustomerID, c.FirstName, c.LastName, c.Email, c.Phone, c.City, c.Country);

        return table;
    }
}
