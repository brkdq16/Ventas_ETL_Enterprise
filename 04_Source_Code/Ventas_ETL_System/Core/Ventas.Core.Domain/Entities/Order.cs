namespace Ventas.Core.Domain.Entities;

public class Order
{
    public int OrderID { get; set; }
    public DateTime OrderDate { get; set; }
    public string? Status { get; set; }
    public int CustomerID { get; set; }
    public int SourceID { get; set; } // FK hacia DataSource
}

