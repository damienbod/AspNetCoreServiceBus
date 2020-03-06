using Microsoft.Azure.ServiceBus;
using System.Text;
using System.Threading.Tasks;

namespace ReducedAccessRightsQueueSender
{
    class Program
    {
        private static string ConnectionStringQueueBus = "your connection string";
        public static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

        static async Task MainAsync()
        {
            var myQueueConnectionString = new ServiceBusConnectionStringBuilder(ConnectionStringQueueBus);
            var myQueue = new QueueClient(myQueueConnectionString);
            var queueMessage = new Message(Encoding.UTF8.GetBytes("some message from somewhere"));
            await myQueue.SendAsync(queueMessage);

        }
    }
}
