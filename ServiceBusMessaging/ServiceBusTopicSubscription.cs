using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceBusMessaging
{
    public interface IServiceBusTopicSubscription
    {
        Task PrepareFiltersAndHandleMessages();
        Task CloseQueueAsync();
        ValueTask DisposeAsync();

    }

    public class ServiceBusTopicSubscription : IServiceBusTopicSubscription
    {
        private readonly IProcessData _processData;
        private readonly IConfiguration _configuration;
        private const string TOPIC_PATH = "mytopic";
        private const string SUBSCRIPTION_NAME = "mytopicsubscription";
        private readonly ILogger _logger;
        private readonly ServiceBusClient _client;
        private ServiceBusProcessor _processor;

        public ServiceBusTopicSubscription(IProcessData processData, 
            IConfiguration configuration, 
            ILogger<ServiceBusTopicSubscription> logger)
        {
            _processData = processData;
            _configuration = configuration;
            _logger = logger;

            var connectionString = _configuration.GetConnectionString("ServiceBusConnectionString");
            _client = new ServiceBusClient(connectionString);
        }

        public async Task PrepareFiltersAndHandleMessages()
        {
            ServiceBusProcessorOptions _serviceBusProcessorOptions = new ServiceBusProcessorOptions
            {
                MaxConcurrentCalls = 1,
                AutoCompleteMessages = false,
            };

            _processor = _client.CreateProcessor(TOPIC_PATH, SUBSCRIPTION_NAME, _serviceBusProcessorOptions);
            _processor.ProcessMessageAsync += ProcessMessagesAsync;
            _processor.ProcessErrorAsync += ProcessErrorAsync;

            await RemoveDefaultFilters();
            await AddFilters();
        }

        private async Task RemoveDefaultFilters()
        {
        //    try
        //    {
        //        var rules = await _subscriptionClient.GetRulesAsync();
        //        foreach(var rule in rules)
        //        {
        //            if(rule.Name == RuleDescription.DefaultRuleName)
        //            {
        //                await _subscriptionClient.RemoveRuleAsync(RuleDescription.DefaultRuleName);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogWarning(ex.ToString());
        //    }
        }

        private async Task AddFilters()
        {
            //try
            //{
            //    var rules = await _subscriptionClient.GetRulesAsync();
            //    if(!rules.Any(r => r.Name == "GoalsGreaterThanSeven"))
            //    {
            //        var filter = new SqlFilter("goals > 7");
            //        await _subscriptionClient.AddRuleAsync("GoalsGreaterThanSeven", filter);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogWarning(ex.ToString());
            //}
        }

        private async Task ProcessMessagesAsync(ProcessMessageEventArgs args)
        {
            var myPayload = args.Message.Body.ToObjectFromJson<MyPayload>();
            await _processData.Process(myPayload);
            await args.CompleteMessageAsync(args.Message);
        }

        private Task ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            _logger.LogError(arg.Exception, "Message handler encountered an exception");
            _logger.LogDebug($"- ErrorSource: {arg.ErrorSource}");
            _logger.LogDebug($"- Entity Path: {arg.EntityPath}");
            _logger.LogDebug($"- FullyQualifiedNamespace: {arg.FullyQualifiedNamespace}");

            return Task.CompletedTask;
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
}
