using Ventas.Core.Domain.Entities;

namespace Ventas.Core.Application.Services;

public class AuditService
{
    public DataSource CreateEntry(string fileName)
    {
        return new DataSource
        {
            SourceType = $"IMPORT_CSV: {fileName}",
            LoadDate = DateTime.Now
        };
    }
}
