using CsvHelper.Configuration;
using Ventas.Core.Domain.Entities;

namespace Ventas.Infrastructure.External.Extractors.Csv;

public class CategoryMap : ClassMap<Category>
{
    public CategoryMap()
    {
        Map(m => m.CategoryID).Name("CategoryID");
        Map(m => m.CategoryName).Name("CategoryName");
    }
}
