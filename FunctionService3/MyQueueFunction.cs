using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace FunctionService3
{
    public static class MyQueueFunction
    {
        [FunctionName("MyQueueFunction")]
        public static void Run([ServiceBusTrigger("myqueue", Connection = "ServiceBusConnectionString")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        }
    }
}
