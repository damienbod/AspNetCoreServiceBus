using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace FunctionService3;

public static class SubscriptionClientFunction
{
    [FunctionName("SubscriptionClientFunction")]
    public static void Run([ServiceBusTrigger(
        "mytopic",
        "functionsubscription",
        Connection = "ServiceBusConnectionString")]string message, ILogger log)
    {
        log.LogInformation("C# ServiceBus topic trigger function processed message: {message}", message);

        var payload = System.Text.Json.JsonSerializer.Deserialize<MyPayload>(message);
    }
}
