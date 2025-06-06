// Revisado 02/06/2025 18:00
using API.DataServices;
using API.DbContexts;
using API.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor.
builder.Services.AddControllers();

// Maneja los errores de ModelState como JSON 
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value.Errors.Count > 0)
            .Select(e => new
            {
                field = e.Key,
                errors = e.Value.Errors.Select(er => er.ErrorMessage)
            });

        return new BadRequestObjectResult(new { errors });
    };
});

var connectionString = builder.Configuration.GetConnectionString("ConexionSGO");

// Registrar los DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseOracle(connectionString);
});

// Registrar los servicios que usan esos DbContext
builder.Services.AddScoped<IOrderDataService, OrderDataService>();
builder.Services.AddScoped<IOrderDetailDataService, OrderDetailDataService>();
builder.Services.AddScoped<IProductDataService, ProductDataService>();
builder.Services.AddScoped<ISupplierDataService, SupplierDataService>();
builder.Services.AddScoped<ICategoryDataService, CategoryDataService>();
builder.Services.AddScoped<IUserDataService, UserDataService>();
builder.Services.AddScoped<ICustomerDataService, CustomerDataService>();

// Configurar Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLogging(); // Habilitar logging

// Configurar logging con ConsoleLogger
builder.Logging.ClearProviders(); // Limpia los proveedores predeterminados
builder.Logging.AddConsole();     // Agrega logging a la consola
builder.Logging.AddDebug();       // Agrega logging para depuración

var app = builder.Build();

// Configurar el pipeline de solicitudes HTTP.
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
