using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServiceBusMessaging;

namespace AspNetCoreServiceBusApi2
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private IServiceBusConsumer _serviceBusConsumer;

        public Worker(ILogger<Worker> logger, IServiceBusConsumer serviceBusConsumer)
        {
            _serviceBusConsumer = serviceBusConsumer;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _serviceBusConsumer.RegisterOnMessageHandlerAndReceiveMessages();
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"Worker running at: {DateTime.Now}");

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
