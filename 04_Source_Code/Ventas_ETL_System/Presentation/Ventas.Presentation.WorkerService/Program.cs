using Ventas.Presentation.WorkerService;
using Microsoft.Extensions.DependencyInjection; // <--- Importante

var builder = Host.CreateApplicationBuilder(args);

// REGISTRO DE SERVICIOS
builder.Services.AddHttpClient(); // Permite que el Worker use la Web
builder.Services.AddHostedService<Worker>(); // Enciende el Worker

var host = builder.Build();
host.Run();
