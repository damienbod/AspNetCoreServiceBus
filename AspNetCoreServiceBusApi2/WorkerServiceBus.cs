using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServiceBusMessaging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AspNetCoreServiceBusApi2
{
    public class WorkerServiceBus : IHostedService, IDisposable
    {
        private readonly ILogger<WorkerServiceBus> _logger;
        private readonly IServiceBusConsumer _serviceBusConsumer;
        private readonly IServiceBusTopicSubscription _serviceBusTopicSubscription;

        public WorkerServiceBus(IServiceBusConsumer serviceBusConsumer,
            IServiceBusTopicSubscription serviceBusTopicSubscription,
            ILogger<WorkerServiceBus> logger)
        {
            _serviceBusConsumer = serviceBusConsumer;
            _serviceBusTopicSubscription = serviceBusTopicSubscription;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("Starting the service bus queue consumer and the subscription");
            await _serviceBusConsumer.RegisterOnMessageHandlerAndReceiveMessages();
            await _serviceBusTopicSubscription.PrepareFiltersAndHandleMessages();
        }
  
        public async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("Stopping the service bus queue consumer and the subscription");
            await _serviceBusConsumer.CloseQueueAsync();
            await _serviceBusTopicSubscription.CloseQueueAsync();
        }

        public void Dispose()
        {
            _ = _serviceBusConsumer.DisposeAsync();
            _ = _serviceBusTopicSubscription.DisposeAsync();
        }
    }
}
