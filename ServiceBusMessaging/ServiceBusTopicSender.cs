using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusMessaging
{
    public class ServiceBusTopicSender
    {
        private readonly TopicClient _topicClient;
        private readonly IConfiguration _configuration;
        private const string TOPIC_PATH = "https://damienbod-service-bus.servicebus.windows.net/mytopic";

        public ServiceBusTopicSender(IConfiguration configuration)
        {
            _configuration = configuration;
            _topicClient = new TopicClient(
                _configuration.GetConnectionString("ServiceBusConnectionString"), 
                TOPIC_PATH
            );
        }
        
        public async Task SendMessage(MyPayload payload)
        {
            string data = JsonConvert.SerializeObject(payload);
            Message message = new Message(Encoding.UTF8.GetBytes(data));

            await _topicClient.SendAsync(message);
        }
        
    }
}
