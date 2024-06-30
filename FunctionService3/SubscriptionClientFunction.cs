using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionService3;

public class SubscriptionClientFunction
{
    private readonly ILogger<SubscriptionClientFunction> _logger;

    public SubscriptionClientFunction(ILogger<SubscriptionClientFunction> logger)
    {
        _logger = logger;
    }

    [Function(nameof(SubscriptionClientFunction))]
    public async Task Run([ServiceBusTrigger("mytopic", "functionsubscription", Connection = "ServiceBusConnectionString")]
        ServiceBusReceivedMessage message, ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation("Message ID: {id}", message.MessageId);
        _logger.LogInformation("Message Body: {body}", message.Body);
        _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

        var payload = System.Text.Json.JsonSerializer.Deserialize<MyPayload>(message.Body);

        // Complete the message
        await messageActions.CompleteMessageAsync(message);
    }
}
