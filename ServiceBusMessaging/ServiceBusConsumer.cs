using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ServiceBusMessaging;

public interface IServiceBusConsumer
{
    Task RegisterOnMessageHandlerAndReceiveMessages();
    Task CloseQueueAsync();
    ValueTask DisposeAsync();
}

public class ServiceBusConsumer : IServiceBusConsumer
{
    private readonly IProcessData _processData;
    private readonly IConfiguration _configuration;
    private readonly ServiceBusClient _client;
    private const string QUEUE_NAME = "simplequeue";
    private readonly ILogger _logger;
    private ServiceBusProcessor _processor;

    public ServiceBusConsumer(IProcessData processData,
        IConfiguration configuration,
        ILogger<ServiceBusConsumer> logger)
    {
        _processData = processData;
        _configuration = configuration;
        _logger = logger;

        var connectionString = _configuration.GetConnectionString("ServiceBusConnectionString");
        _client = new ServiceBusClient(connectionString);
    }

    public async Task RegisterOnMessageHandlerAndReceiveMessages()
    {
        ServiceBusProcessorOptions _serviceBusProcessorOptions = new ServiceBusProcessorOptions
        {
            MaxConcurrentCalls = 1,
            AutoCompleteMessages = false,
        };

        _processor = _client.CreateProcessor(QUEUE_NAME, _serviceBusProcessorOptions);
        _processor.ProcessMessageAsync += ProcessMessagesAsync;
        _processor.ProcessErrorAsync += ProcessErrorAsync;
        await _processor.StartProcessingAsync().ConfigureAwait(false);
    }

    private Task ProcessErrorAsync(ProcessErrorEventArgs arg)
    {
        _logger.LogError(arg.Exception, "Message handler encountered an exception");
        _logger.LogDebug($"- ErrorSource: {arg.ErrorSource}");
        _logger.LogDebug($"- Entity Path: {arg.EntityPath}");
        _logger.LogDebug($"- FullyQualifiedNamespace: {arg.FullyQualifiedNamespace}");

        return Task.CompletedTask;
    }

    private async Task ProcessMessagesAsync(ProcessMessageEventArgs args)
    {
        var myPayload = args.Message.Body.ToObjectFromJson<MyPayload>();
        await _processData.Process(myPayload).ConfigureAwait(false);
        await args.CompleteMessageAsync(args.Message).ConfigureAwait(false);
    }

    public async ValueTask DisposeAsync()
    {
        if (_processor != null)
        {
            await _processor.DisposeAsync().ConfigureAwait(false);
        }

        if (_client != null)
        {
            await _client.DisposeAsync().ConfigureAwait(false);
        }
    }

    public async Task CloseQueueAsync()
    {
        await _processor.CloseAsync().ConfigureAwait(false);
    }
}
