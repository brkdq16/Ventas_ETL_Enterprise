using Ventas.Core.Domain.Entities;

namespace Ventas.Core.Application.Services;

public class DataNormalizationService
{
    public void NormalizeProduct(Product product, List<Category> categories)
    {
        // Normalizamos el nombre
        product.ProductName = product.ProductName?.Trim().ToUpper() ?? "SIN_NOMBRE";

        // Vínculo de Integridad Referencial (Punto 2.2 SRS)
        var match = categories.FirstOrDefault(c => c.CategoryName == product.CategoryNameFromCsv);
        product.CategoryID = match?.CategoryID ?? 0;
    }


    public void NormalizeCustomer(Customer customer)
    {
        customer.Email = customer.Email?.ToLower().Trim();
        customer.FirstName = customer.FirstName.Trim();
    }
}
