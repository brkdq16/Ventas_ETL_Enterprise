using Microsoft.Data.SqlClient;
using System.IO;
using Ventas.Core.Domain.Entities;
using Ventas.Core.Domain.Interfaces;
using Ventas.Infrastructure.Data.Repositories;

namespace Ventas.Core.Application.Services;

public class EtlOrchestrator
{
    // RUTAS FÍSICAS
    private readonly string _basePath = @"D:\Projects\Ventas_ETL_Enterprise\02_Data_Sources\Files_CSV\";
    private readonly string _connString = @"Server=localhost\SQLEXPRESS;Database=Ventas_OLTP;Trusted_Connection=True;TrustServerCertificate=True;";

    public void RunFullPipeline(
        IExtractor<Customer> custExt,
        IExtractor<Product> prodExt,
        IExtractor<Order> orderExt,
        IExtractor<OrderDetail> detailExt)
    {
        // 1. Instanciar Especialistas de Aplicación
        var auditor = new AuditService();
        var discoverer = new CategoryDiscoveryService();
        var normalizer = new DataNormalizationService();
        var validator = new DataValidationService();

        using var conn = new SqlConnection(_connString);
        conn.Open();
        using var trans = conn.BeginTransaction();

        try
        {
            Console.WriteLine("=== INICIANDO PIPELINE TRANSACCIONAL ===\n");

            // --- PASO 1: DATA SOURCE (Auditoría) ---
            var sourceEntry = auditor.CreateEntry("Batch_Completo_OLTP.csv");
            int sourceId = new DataSourceRepository().Insert(sourceEntry, conn, trans);
            Console.WriteLine($" [1/6] Auditoría: SourceID {sourceId} generado.");

            // --- PASO 2: CLIENTES ---
            var rawCust = custExt.Extract(Path.Combine(_basePath, "customers.csv")).ToList();
            new CustomerRepository().BulkInsert(rawCust, conn, trans);
            Console.WriteLine($" [2/6] Clientes: {rawCust.Count} insertados.");

            // --- PASO 3: CATEGORÍAS (Descubrimiento de Oro) ---
            var rawProd = prodExt.Extract(Path.Combine(_basePath, "products.csv")).ToList();
            var categories = discoverer.GetUniqueCategories(rawProd);

            if (categories.Count > 0)
                new CategoryRepository().BulkInsert(categories, conn, trans);

            Console.WriteLine($" [3/6] Categorías: {categories.Count} descubiertas e insertadas.");

            // --- PASO 4: PRODUCTOS (Hijos de Categorías) ---
            foreach (var p in rawProd)
                normalizer.NormalizeProduct(p, categories); // Vincula el ID real

            new ProductRepository().BulkInsert(rawProd, conn, trans);
            Console.WriteLine($" [4/6] Productos: {rawProd.Count} insertados.");

            // --- PASO 5: ÓRDENES (Hijos de Clientes y DataSource) ---
            var rawOrders = orderExt.Extract(Path.Combine(_basePath, "orders.csv")).ToList();
            foreach (var o in rawOrders)
                o.SourceID = sourceId; // Sello de auditoría

            new OrderRepository().BulkInsert(rawOrders, conn, trans);
            Console.WriteLine($" [5/6] Órdenes: {rawOrders.Count} vinculadas.");

            // --- PASO 6: DETALLES (Hijos de Órdenes y Productos) ---
            var rawDetails = detailExt.Extract(Path.Combine(_basePath, "order_details.csv")).ToList();

            // IMPORTANTE: El repositorio de detalles NO debe mapear LineTotal si es columna calculada en SQL
            new OrderDetailRepository().BulkInsert(rawDetails, conn, trans);
            Console.WriteLine($" [6/6] Detalles: {rawDetails.Count} procesados.");

            // 🔥 MOMENTO DE LA VERDAD
            trans.Commit();
            Console.WriteLine("\n [ÉXITO TOTAL]: Base de datos sincronizada correctamente.");
        }
        catch (Exception ex)
        {
            trans.Rollback();
            Console.WriteLine($"\n⚠️ [ROLLBACK]: Error detectado. La base de datos sigue limpia.");
            Console.WriteLine($"Detalle: {ex.Message}");
            throw;
        }
    }
}
