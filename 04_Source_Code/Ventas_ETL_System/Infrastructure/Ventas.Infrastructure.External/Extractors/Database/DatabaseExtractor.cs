using Microsoft.Data.SqlClient;
using Dapper;
using Ventas.Core.Domain.Interfaces;

namespace Ventas.Infrastructure.External.Extractors.Database;

public class DatabaseExtractor<T> : IExtractor<T> where T : class
{
    private readonly string _connectionString;

    // IMPORTANTE: Recibimos la cadena y la guardamos en la variable privada
    public DatabaseExtractor(string connectionString)
    {
        // Si la cadena llega nula, lanzamos un error claro aquí mismo
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public IEnumerable<T> Extract(string query)
    {
        // Aquí usamos la variable que ya tiene la cadena guardada
        using (var connection = new SqlConnection(_connectionString))
        {
            // Abrimos explícitamente para verificar la cadena
            connection.Open();

            // Usamos 'buffered: true' por ahora para estabilizar el error
            return connection.Query<T>(query, buffered: true);
        }
    }
}

