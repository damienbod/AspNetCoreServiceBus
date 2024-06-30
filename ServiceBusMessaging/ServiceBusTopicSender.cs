using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ServiceBusMessaging;

public class ServiceBusTopicSender
{
    private const string TOPIC_PATH = "mytopic";
    private readonly ILogger _logger;
    private readonly ServiceBusClient _client;
    private readonly Azure.Messaging.ServiceBus.ServiceBusSender _clientSender;

    public ServiceBusTopicSender(IConfiguration configuration,
        ILogger<ServiceBusTopicSender> logger)
    {
        _logger = logger;

        var connectionString = configuration.GetConnectionString("ServiceBusConnectionString");
        _client = new ServiceBusClient(connectionString);
        _clientSender = _client.CreateSender(TOPIC_PATH);
    }

    public async Task SendMessage(MyPayload payload)
    {
        string messagePayload = JsonSerializer.Serialize(payload);
        var message = new ServiceBusMessage(messagePayload);

        message.ApplicationProperties.Add("goals", payload.Goals);

        try
        {
            await _clientSender.SendMessageAsync(message);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }
    }
}
