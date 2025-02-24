using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Confluent.Kafka;
using System;
using System.Threading;

namespace KafkaService
{
    /// <summary>
    /// Kafka consumer service, used to consume messages from Kafka
    /// </summary>
    public class KafkaConsumerService
    {
        private readonly IConsumer<Null, string> _consumer;
        private readonly string _topic = "order-topic";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="config">KafkaConfig</param>
        /// <param name="topic">A string represent the topic</param>
        public KafkaConsumerService(KafkaConfig config, string topic)
        {
            _topic = topic;
            var consumerConfig = new ConsumerConfig
            {
                GroupId = config.GroupId,
                BootstrapServers = config.BootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            _consumer = new ConsumerBuilder<Null, string>(consumerConfig).Build();
            _consumer.Subscribe(_topic);
        }

        /// <summary>
        /// Consume messages from Kafka, process message with provided action
        /// </summary>
        /// <param name="processMessage"></param> 
        public void ConsumeMessages(Action<string> processMessage)
        {
            while (true)
            {
                var consumeResult = _consumer.Consume(CancellationToken.None);

                Console.WriteLine($"Received message: {consumeResult.Message.Value}");
                processMessage(consumeResult.Message.Value);
            }
        }

        public void Dispose()
        {
            _consumer.Close();
            _consumer.Dispose();
        }

    }

}
