using Ventas.Core.Application.Services;
using Ventas.Infrastructure.External.CsvMappers;
using Ventas.Core.Domain.Entities;

// 1. Encendemos el Orquestador
var orchestrator = new EtlOrchestrator();

// 2. Disparamos el Pipeline pasándole las herramientas (Inyección)
orchestrator.RunFullPipeline(
    new CsvExtractor<Customer>(),
    new CsvExtractor<Product>(),
    new CsvExtractor<Order>(),
    new CsvExtractor<OrderDetail>()
);

// 3. Pausa para ver los mensajes que imprimió el Orquestador
Console.WriteLine("\nPresiona cualquier tecla para cerrar...");
Console.ReadKey();
