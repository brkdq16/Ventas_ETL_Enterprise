using Ventas.Core.Domain.Entities;

namespace Ventas.Core.Application.Services;

public class CategoryDiscoveryService
{
    public List<Category> GetUniqueCategories(IEnumerable<Product> products)
    {
        // Extraemos los nombres que el Map guardó en 'CategoryNameFromCsv'
        var uniqueNames = products
            .Select(p => p.CategoryNameFromCsv)
            .Where(name => !string.IsNullOrWhiteSpace(name)) // Filtramos vacíos
            .Distinct() // Quitamos duplicados
            .ToList();

        // Convertimos esos nombres en objetos de Categoría con ID
        return uniqueNames.Select((name, index) => new Category
        {
            CategoryID = index + 1,
            CategoryName = name
        }).ToList();
    }
}
