using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Linq; // Necesario para .ToList()
using Ventas.Core.Domain.Entities;
using Ventas.Core.Domain.Interfaces;
using Ventas.Infrastructure.External.Extractors.Csv;
using Ventas.Infrastructure.External.Extractors.Database;
using Ventas.Infrastructure.External.Extractors.Api;

namespace Ventas.Presentation.WorkerService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public Worker(ILogger<Worker> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("🚀 [WORKER INICIADO]: Esperando primer ciclo de extracción...");

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("------------------------------------------------------------");
            _logger.LogInformation("⏳ [ETL BATCH START]: {time}", DateTimeOffset.Now);

            try
            {
                // 1. CONFIGURACIÓN LOCAL (Evita errores de contexto en hilos asíncronos)
                string localConn = @"Server=localhost\SQLEXPRESS;Database=Ventas_OLTP;Trusted_Connection=True;TrustServerCertificate=True;";
                string localCsv = @"D:\Projects\Ventas_ETL_Enterprise\02_Data_Sources\Files_CSV\products.csv";
                string localApi = "https://jsonplaceholder.typicode.com/comments";

                // 2. INSTANCIACIÓN DE MOTORES
                var dbExt = new DatabaseExtractor<Customer>(localConn);
                var csvExt = new CsvExtractor<Product>();
                var apiExt = new ApiExtractor<Comment>(_httpClientFactory.CreateClient());

                // 3. EJECUCIÓN EN PARALELO (Atributo de Rendimiento)
                // Usamos Task.Run para que las 3 fuentes se extraigan al mismo tiempo
                var taskCsv = Task.Run(() => csvExt.Extract(localCsv).ToList(), stoppingToken);
                var taskDb = Task.Run(() => dbExt.Extract("SELECT * FROM Customers").ToList(), stoppingToken);
                var taskApi = Task.Run(() => apiExt.Extract(localApi).ToList(), stoppingToken);

                // Esperamos a que el "Tridente" de extracción termine
                await Task.WhenAll(taskCsv, taskDb, taskApi);

                // 4. REPORTE DE OBSERVABILIDAD (Logging de resultados)
                _logger.LogInformation(" RESULTADOS DE LA EXTRACCIÓN:");
                _logger.LogInformation("    [CSV - Productos]: {count} registros leídos.", taskCsv.Result.Count);
                _logger.LogInformation("    [SQL - Clientes]:  {count} obtenidos de la OLTP.", taskDb.Result.Count);
                _logger.LogInformation("    [API - Comentarios]: {count} descargados de la web.", taskApi.Result.Count);

                _logger.LogInformation(" [CICLO EXITOSO]: Datos listos para la Fase de Carga.");
            }
            catch (Exception ex)
            {
                // Si algo falla, el logger nos dirá exactamente por qué (Resiliencia)
                _logger.LogError(ex, "❌ [ERROR CRÍTICO]: El proceso de extracción falló en este ciclo.");
            }

            _logger.LogInformation(" Próximo intento en 1 minuto...");
            _logger.LogInformation("------------------------------------------------------------");

            // Intervalo de 1 minuto (Requisito de procesamiento periódico)
            await Task.Delay(60000, stoppingToken);
        }
    }
}

// Entidad temporal para cumplir con el requisito de "Fuente Externa API"
public class Comment
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Body { get; set; }
}
