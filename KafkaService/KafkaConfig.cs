namespace KafkaService
{
    /// <summary>
    /// Kafka configuration
    /// </summary>
    public class KafkaConfig
    {
        public string BootstrapServers { get; set; } = "localhost:2181";
        public string GroupId { get; set; } = "order-group";

        //public KafkaConfig()
        //{
        //}
        public KafkaConfig(string bootstrapServers, string groupId)
        {
            BootstrapServers = bootstrapServers;
            GroupId = groupId;
        }
    }
}
