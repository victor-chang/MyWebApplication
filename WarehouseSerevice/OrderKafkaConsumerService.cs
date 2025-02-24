using KafkaService;
namespace WarehouseSerevice
{
    /// <summary>
    /// Kafka consumer service for orders, used to process order in warehouse when order is created
    /// </summary>
    public class OrderKafkaConsumerService : BackgroundService
    {
        private readonly KafkaConfig _config;
        private readonly KafkaConsumerService _kafkaConsumerService;
        private readonly string _topic;
        

        public OrderKafkaConsumerService(KafkaConfig config, string topic)
        {
            _config = config;
            _topic = topic;
            _kafkaConsumerService = new KafkaConsumerService(_config, _topic);
            
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(() => _kafkaConsumerService.ConsumeMessages(_processMessage), stoppingToken);
            
        }

        private void _processMessage(string message)
        {
            Console.WriteLine($"Processing message: {message}");
            // first check if order is paid in databasee
            // if not paid, send message to payment service to pay
            // if paid, send message to warehouse service to process order
        
        }

        public override void Dispose()
        {
            _kafkaConsumerService.Dispose();
            base.Dispose();
        }

    }
}
