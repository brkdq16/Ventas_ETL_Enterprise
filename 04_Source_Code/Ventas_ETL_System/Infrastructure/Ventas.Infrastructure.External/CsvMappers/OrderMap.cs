using CsvHelper.Configuration;
using Ventas.Core.Domain.Entities;

public class OrderMap : ClassMap<Order>
{
    public OrderMap()
    {
        Map(m => m.OrderID).Name("OrderID");
        Map(m => m.OrderDate).Name("OrderDate"); // Aquí C# normaliza el formato de fecha solo
        Map(m => m.Status).Name("Status");
        Map(m => m.CustomerID).Name("CustomerID");
        Map(m => m.SourceID).Ignore(); // Integridad: El SourceID lo pone el código, no el CSV
    }
}
