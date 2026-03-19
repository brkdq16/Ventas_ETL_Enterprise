using CsvHelper;
using System.Globalization;
using System.Reflection;
using Ventas.Core.Domain.Interfaces;

namespace Ventas.Infrastructure.External.Extractors.Csv;

public class CsvExtractor<T> : IExtractor<T> where T : class
{
    public IEnumerable<T> Extract(string filePath)
    {
        // 1. Abrimos el archivo permitiendo lectura compartida (evita el error de "archivo en uso")
        var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        var reader = new StreamReader(stream);
        var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        // 2. REGISTRO DINÁMICO DE MAPAS (SOLID)
        // Busca automáticamente en este proyecto un mapa que corresponda a <T>
        var mapType = Assembly.GetExecutingAssembly()
            .GetTypes()
            .FirstOrDefault(t => t.BaseType != null &&
                                 t.BaseType.IsGenericType &&
                                 t.BaseType.GetGenericTypeDefinition() == typeof(CsvHelper.Configuration.ClassMap<>) &&
                                 t.BaseType.GetGenericArguments()[0] == typeof(T));

        if (mapType != null)
        {
            csv.Context.RegisterClassMap(mapType);
        }

        // 3. RETORNO POR STREAMING (Big Data)
        // Quitamos el .ToList(). Ahora los datos fluyen uno a uno.
        // CsvHelper se encargará de cerrar el reader al terminar la enumeración.
        foreach (var record in csv.GetRecords<T>())
        {
            yield return record;
        }
    }
}
