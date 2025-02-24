using KafkaService;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderService;
using OrderService.Models;

namespace OrderService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<MyWebApplicationContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("MyWebApplicationDb") ?? throw new InvalidOperationException("Connection string 'MyWebApplicationDb' not found.")));

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddOpenApiDocument(configure =>
            {
                configure.Title = "Your API Title";
                configure.Version = "v1";
                configure.DocumentName = "v1"; // Ensure the document name is specified
            });
                // Add Kafka producer services
                builder.Services.AddSingleton<KafkaConfig>(new KafkaConfig("127.0.0.1:9092", "order-group"));
            builder.Services.AddSingleton<KafkaProducerService>();

                        builder.Services.AddEndpointsApiExplorer();

                        builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseOpenApi(); // This method is provided by NSwag.AspNetCore
                app.UseSwaggerUi();
            }

                        
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();


            app.Run();
        }
    }
}
