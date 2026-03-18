using Ventas.Core.Domain.Entities;

namespace Ventas.Core.Application.Services;

public class DataValidationService
{
    public IEnumerable<T> RemoveNullsAndDuplicates<T>(IEnumerable<T> data, Func<T, object> keySelector)
    {
        // Filtramos nulos y aplicamos Distinct por la llave (ID o Email)
        return data.Where(x => x != null).DistinctBy(keySelector);
    }
}
