using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace KafkaService
{
    public class KafkaProducerService
    {
        
        private readonly IProducer<Null, string> _producer; 

        public KafkaProducerService(KafkaConfig config)
        {
            var producerConfig = new ProducerConfig { BootstrapServers = config.BootstrapServers };
            _producer = new ProducerBuilder<Null, string>(producerConfig).Build();
        }

        public async Task SendMessageAsync(string topic, string message)
        {
            Console.WriteLine($"Producing message -> Topic: {topic}, Message: {message}");
            await _producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
        }
    }

}
