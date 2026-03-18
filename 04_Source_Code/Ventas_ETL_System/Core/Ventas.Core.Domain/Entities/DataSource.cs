namespace Ventas.Core.Domain.Entities;

public class DataSource
{
    public int SourceID { get; set; }
    public string SourceType { get; set; } = string.Empty; // 'CSV', 'API', etc.
    public DateTime LoadDate { get; set; } = DateTime.Now;
}



