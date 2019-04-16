using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusMessaging
{
    public class ServiceBusSender
    {
        private readonly QueueClient _queueClient;
        private const string QUEUE_NAME = "simplequeue";

        public ServiceBusSender()
        {
            _queueClient = new QueueClient("Endpoint=sb://damienbodservicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=16+6eQLwf+yGrFbqvW1/ssr8QojrI3b6wDaRM89hBPU=", QUEUE_NAME);
        }
        
        public async Task SendMessage(MyPayload payload)
        {
            string data = JsonConvert.SerializeObject(payload);
            Message message = new Message(Encoding.UTF8.GetBytes(data));

            await _queueClient.SendAsync(message);
        }
        
    }
}
