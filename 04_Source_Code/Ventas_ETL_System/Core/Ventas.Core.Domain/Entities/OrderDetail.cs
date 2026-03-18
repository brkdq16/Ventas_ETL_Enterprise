public class OrderDetail
{
    public int OrderDetailID { get; set; }
    public int OrderID { get; set; }
    public int ProductID { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; } // Lo calcularemos nosotros
    public decimal LineTotal { get; set; }  // Lo leeremos del CSV
}

