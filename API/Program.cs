// Revisado 02/06/2025 18:00
using API.DataServices;
using API.DbContexts;
using API.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

// Configuración de autenticación JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

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

app.UseAuthentication(); // <-- Agrega esta línea antes de UseAuthorization
app.UseAuthorization();

app.MapControllers();

app.Run();