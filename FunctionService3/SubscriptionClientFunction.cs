using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionService3
{
    public static class SubscriptionClientFunction
    {
        [FunctionName("SubscriptionClientFunction")]
        public static void Run([ServiceBusTrigger(
            "mytopic", 
            "functionsubscription",
            Connection = "ServiceBusConnectionString")]string mySbMsg, ILogger log)
        {
            log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");

            var payload = JsonConvert.DeserializeObject<MyPayload>(mySbMsg);
        }
    }
}
