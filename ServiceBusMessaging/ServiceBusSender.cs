using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace ServiceBusMessaging;

public class ServiceBusSender
{
    private readonly ServiceBusClient _client;
    private readonly Azure.Messaging.ServiceBus.ServiceBusSender _clientSender;
    private const string QUEUE_NAME = "simplequeue";

    public ServiceBusSender(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ServiceBusConnectionString");
        _client = new ServiceBusClient(connectionString);
        _clientSender = _client.CreateSender(QUEUE_NAME);
    }

    public async Task SendMessage(MyPayload payload)
    {
        string messagePayload = JsonSerializer.Serialize(payload);
        var message = new ServiceBusMessage(messagePayload);
        await _clientSender.SendMessageAsync(message);
    }
}
