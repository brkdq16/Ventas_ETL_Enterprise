using System.Net.Http.Json;
using Ventas.Core.Domain.Interfaces;

namespace Ventas.Infrastructure.External.Extractors.Api;

public class ApiExtractor<T> : IExtractor<T> where T : class
{
    private readonly HttpClient _httpClient;

    public ApiExtractor(HttpClient httpClient) => _httpClient = httpClient;

    public IEnumerable<T> Extract(string url)
    {
        // 1. Bajamos los datos de internet (JSON)
        var response = _httpClient.GetFromJsonAsync<List<T>>(url).GetAwaiter().GetResult();

        if (response != null)
        {
            foreach (var item in response)
            {
                yield return item; // Entregamos los datos al Worker sin imprimir basura
            }
        }
    }
}
