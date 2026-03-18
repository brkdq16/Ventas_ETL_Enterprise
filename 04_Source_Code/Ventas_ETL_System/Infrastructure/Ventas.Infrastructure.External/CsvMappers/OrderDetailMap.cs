using CsvHelper.Configuration;

public class OrderDetailMap : ClassMap<OrderDetail>
{
    public OrderDetailMap()
    {
        Map(m => m.OrderDetailID).Ignore();
        Map(m => m.OrderID).Name("OrderID");
        Map(m => m.ProductID).Name("ProductID");
        Map(m => m.Quantity).Name("Quantity");

        // 1. Leemos el Total directamente del CSV
        Map(m => m.LineTotal).Name("TotalPrice");

        // 2. TRANSFORMACIÓN: Calculamos el UnitPrice (Precio de Unidad)
        // Lógica: UnitPrice = Total / Cantidad
        Map(m => m.UnitPrice).Convert(args => {
            decimal total = args.Row.GetField<decimal>("TotalPrice");
            int cantidad = args.Row.GetField<int>("Quantity");
            return cantidad > 0 ? total / cantidad : 0;
        });
    }
}
