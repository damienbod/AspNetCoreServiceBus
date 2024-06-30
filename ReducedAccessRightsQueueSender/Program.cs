using Azure.Messaging.ServiceBus;

namespace ReducedAccessRightsQueueSender;

class Program
{
    private static readonly string ConnectionStringQueueBus = "your connection string";
    private static ServiceBusClient? _client;
    private static ServiceBusSender? _clientSender;

    static async Task Main(string[] args)
    {
        _client = new ServiceBusClient(ConnectionStringQueueBus);
        _clientSender = _client.CreateSender("myqueue");

        //string messagePayload = JsonSerializer.Serialize(payload);
        var message = new ServiceBusMessage("some message from somewhere");
        await _clientSender.SendMessageAsync(message);
    }
}
