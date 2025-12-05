using Microsoft.EntityFrameworkCore;
using Tekus.Services.Application.Interfaces;
using Tekus.Services.Infrastructure.Persistence;
using Tekus.Services.Infrastructure.Services;
using Tekus.Services.Application.Interfaces;
using Tekus.Services.Infrastructure.Services;

namespace Tekus.Services.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // -------------------------------
            // Configuración de infraestructura
            // -------------------------------

            // DbContext con SQL Server usando la cadena de conexión del appsettings.json
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
                options.UseSqlServer(connectionString);
            });

            // Registro del servicio de aplicación de proveedores
            builder.Services.AddScoped<IProviderService, ProviderService>();

            // -------------------------------
            // Servicios de la API
            // -------------------------------

            builder.Services.AddControllers();
            // Swagger / OpenAPI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IProviderService, ProviderService>();
            builder.Services.AddScoped<IServiceService, ServiceService>();
            builder.Services.AddScoped<ICountryService, CountryService>();

            var app = builder.Build();

            // -------------------------------
            // Pipeline HTTP
            // -------------------------------

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}