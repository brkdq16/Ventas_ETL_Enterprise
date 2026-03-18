using Microsoft.Data.SqlClient;
using Ventas.Core.Domain.Entities;

namespace Ventas.Infrastructure.Data.Repositories;

public class DataSourceRepository
{
    // Inserta el origen y retorna el ID generado (Identity)
    public int Insert(DataSource source, SqlConnection conn, SqlTransaction trans)// Recibe la conexión y transacción para participar en el mismo contexto de la carga
    {
        var sql = @"INSERT INTO DataSources (SourceType, LoadDate) 
                    VALUES (@type, @date); 
                    SELECT CAST(SCOPE_IDENTITY() as int);";

        using var cmd = new SqlCommand(sql, conn, trans);
        cmd.Parameters.AddWithValue("@type", source.SourceType);
        cmd.Parameters.AddWithValue("@date", source.LoadDate);

        return Convert.ToInt32(cmd.ExecuteScalar());
    }
}
