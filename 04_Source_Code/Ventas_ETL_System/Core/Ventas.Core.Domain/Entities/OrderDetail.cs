namespace Ventas.Core.Domain.Entities;

public class OrderDetail
{
    public int OrderDetailID { get; set; }
    public int OrderID { get; set; }
    public int ProductID { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    // Aquí aplicamos lógica de negocio: el campo derivado
    public decimal LineTotal => Quantity * UnitPrice;
}
