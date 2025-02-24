using KafkaService;
namespace WarehouseSerevice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            // Add Kafka producer services
            //builder.Services.AddSingleton<KafkaConfig>(new KafkaConfig("localhost:9092", "order-group"));
            // Add Kafka consumer services
            builder.Services.AddSingleton<KafkaConfig>(new KafkaConfig("localhost:9092", "order-group"));
            //builder.Services.AddSingleton<KafkaConsumerService>();
            //builder.Services.AddSingleton<KafkaConsumerService>(provider =>
            //    new KafkaConsumerService(provider.GetRequiredService<KafkaConfig>(), "order-topic"));
            builder.Services.AddHostedService<OrderKafkaConsumerService>(sp =>
                new OrderKafkaConsumerService(sp.GetRequiredService<KafkaConfig>(), "order-topic")
            );

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
