namespace Ventas.Core.Domain.Interfaces;

public interface IRepository<T> where T : class
{
    // Solo una cosa: Tomar una lista y mandarla a SQL masivamente
    void BulkInsert(IEnumerable<T> items);
}
