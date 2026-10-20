using BibliotecaApi.Services;
using Scalar.AspNetCore;
using Serilog;

// Configuracion inicial de Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{

    var builder = WebApplication.CreateBuilder(args);

    // Integrar Serilog al host
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .WriteTo.Console());

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();
    // Inyección de dependencias de los servicios
    builder.Services.AddScoped<EstudiantesService>();
    builder.Services.AddScoped<LibrosService>();
    builder.Services.AddScoped<PrestamosService>();
    builder.Services.AddScoped<DevolucionesService>();

    var app = builder.Build();

    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference(options =>
        {
            options.WithTitle("API Biblioteca - Préstamos")
                   .WithTheme(ScalarTheme.Moon);
        });
    }

    app.MapGet("/", () => Results.Redirect("/scalar/v1")).ExcludeFromDescription();

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Error critico al inicializar la aplicacion");
}
finally
{
    Log.CloseAndFlush();
}