using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace FunctionService3;

public class MyQueueFunction
{
    private readonly ILogger<MyQueueFunction> _logger;

    public MyQueueFunction(ILogger<MyQueueFunction> logger)
    {
        _logger = logger;
    }

    [Function(nameof(MyQueueFunction))]
    public async Task Run([ServiceBusTrigger("myqueue", Connection = "ServiceBusConnectionString")]
        ServiceBusReceivedMessage message, ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation("Message ID: {id}", message.MessageId);
        _logger.LogInformation("Message Body: {body}", message.Body);
        _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

        // Complete the message
        await messageActions.CompleteMessageAsync(message);

        _logger.LogInformation("C# ServiceBus queue trigger function processed message: {myQueueItem}", message);
    }
}
