using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace FunctionService3;

public static class MyQueueFunction
{
    [FunctionName("MyQueueFunction")]
    public static void Run([ServiceBusTrigger("myqueue", Connection = "ServiceBusConnectionString")] string myQueueItem, ILogger log)
    {
        //throw new Exception("Cannot not process for some reason");
        log.LogInformation("C# ServiceBus queue trigger function processed message: {myQueueItem}", myQueueItem);
    }
}
