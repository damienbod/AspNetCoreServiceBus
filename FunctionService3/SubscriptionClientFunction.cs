using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace FunctionService3
{
    public static class Function1
    {
        [FunctionName("SubscriptionClientFunction")]
        public static void Run([ServiceBusTrigger("mytopic", "functionsubscription", Connection = "ServiceBusConnectionString")]string mySbMsg, ILogger log)
        {
            log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
        }
    }
}
