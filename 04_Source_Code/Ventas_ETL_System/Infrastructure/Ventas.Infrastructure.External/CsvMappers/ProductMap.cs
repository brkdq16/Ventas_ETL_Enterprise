using CsvHelper.Configuration;
using Ventas.Core.Domain.Entities;

namespace Ventas.Infrastructure.External.CsvMappers;

public class ProductMap : ClassMap<Product>
{
    public ProductMap()
    {
        Map(m => m.ProductID).Name("ProductID");
        Map(m => m.ProductName).Name("ProductName");
        Map(m => m.Price).Name("Price");
        Map(m => m.Stock).Name("Stock");

        // ESTA LÍNEA ES LA CLAVE: 
        // Lee la columna "Category" del CSV y la mete en la propiedad temporal
        Map(m => m.CategoryNameFromCsv).Name("Category");

        // El ID real lo ignoramos porque el DiscoveryService lo creará después
        Map(m => m.CategoryID).Ignore();
    }
}




