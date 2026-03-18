using Ventas.Core.Domain.Entities;

namespace Ventas.Core.Domain.Interfaces;

public interface IExtractor<T> where T : class
{
    // "Dime la ruta del archivo y te devolveré los datos limpios"
    IEnumerable<T> Extract(string filePath);
}