using CsvHelper;
using System.Globalization;
using Ventas.Core.Domain.Entities;
using Ventas.Core.Domain.Interfaces;

namespace Ventas.Infrastructure.External.CsvMappers;

// Esta es tu "Máquina Universal" de lectura de archivos
public class CsvExtractor<T> : IExtractor<T> where T : class
{
    public IEnumerable<T> Extract(string filePath)
    {
        // 1. Abrimos el archivo en la pc
        using var reader = new StreamReader(filePath);

        // 2. Configuramos el lector con cultura invariante (punto para decimales)
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        // 3. REGISTRO DE MAPAS (El cerebro de traducción)
        // Según la T que reciba, el extractor elige el mapa correcto

        if (typeof(T) == typeof(Category))
            csv.Context.RegisterClassMap<CategoryMap>();

        else if (typeof(T) == typeof(Customer))
            csv.Context.RegisterClassMap<CustomerMap>();

        else if (typeof(T) == typeof(Product))
            csv.Context.RegisterClassMap<ProductMap>();

        else if (typeof(T) == typeof(Order))
            csv.Context.RegisterClassMap<OrderMap>();

        else if (typeof(T) == typeof(OrderDetail))
            csv.Context.RegisterClassMap<OrderDetailMap>();

        // 4. Ejecutamos la lectura y convertimos a lista
        // .ToList() carga los datos en RAM para procesarlos en el Service
        return csv.GetRecords<T>().ToList();
    }
}
