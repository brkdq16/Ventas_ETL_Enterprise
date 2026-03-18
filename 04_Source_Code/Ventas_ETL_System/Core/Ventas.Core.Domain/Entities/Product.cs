namespace Ventas.Core.Domain.Entities;

public class Product
{
    public int ProductID { get; set; } // PK
    public string ProductName { get; set; } = string.Empty;
    public int CategoryID { get; set; } // FK
    public decimal Price { get; set; }
    public int Stock { get; set; }
}

