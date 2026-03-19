using CsvHelper.Configuration;
using Ventas.Core.Domain.Entities;

namespace Ventas.Infrastructure.External.Extractors.Csv;

public class CustomerMap : ClassMap<Customer>
{
    public CustomerMap()
    {
        Map(m => m.CustomerID).Name("CustomerID");
        Map(m => m.FirstName).Name("FirstName");
        Map(m => m.LastName).Name("LastName");
        Map(m => m.Email).Name("Email");
        Map(m => m.Phone).Name("Phone");
        Map(m => m.City).Name("City");
        Map(m => m.Country).Name("Country");
    }
}
