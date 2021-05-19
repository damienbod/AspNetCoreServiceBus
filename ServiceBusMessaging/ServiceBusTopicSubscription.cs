using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly ServiceBusAdministrationClient _adminClient;
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
            _adminClient = new ServiceBusAdministrationClient(connectionString);
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

            await _processor.StartProcessingAsync().ConfigureAwait(false);
        }

        private async Task RemoveDefaultFilters()
        {
            try
            {
                var rules = _adminClient.GetRulesAsync(TOPIC_PATH, SUBSCRIPTION_NAME);
                var ruleProperties = new List<RuleProperties>();
                await foreach (var rule in rules)
                {
                    ruleProperties.Add(rule);
                }

                foreach (var rule in ruleProperties)
                {
                    if (rule.Name == "GoalsGreaterThanSeven")
                    {
                        await _adminClient.DeleteRuleAsync(TOPIC_PATH, SUBSCRIPTION_NAME, "GoalsGreaterThanSeven")
                            .ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.ToString());
            }
        }

        private async Task AddFilters()
        {
            try
            {
                var rules = _adminClient.GetRulesAsync(TOPIC_PATH, SUBSCRIPTION_NAME)
                    .ConfigureAwait(false);

                var ruleProperties = new List<RuleProperties>();
                await foreach (var rule in rules)
                {
                    ruleProperties.Add(rule);
                }

                if (!ruleProperties.Any(r => r.Name == "GoalsGreaterThanSeven"))
                {
                    CreateRuleOptions createRuleOptions = new CreateRuleOptions
                    {
                        Name = "GoalsGreaterThanSeven",
                        Filter = new SqlRuleFilter("goals > 7")
                    };
                    await _adminClient.CreateRuleAsync(TOPIC_PATH, SUBSCRIPTION_NAME, createRuleOptions)
                        .ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.ToString());
            }
        }

        private async Task ProcessMessagesAsync(ProcessMessageEventArgs args)
        {
            var myPayload = args.Message.Body.ToObjectFromJson<MyPayload>();
            await _processData.Process(myPayload).ConfigureAwait(false);
            await args.CompleteMessageAsync(args.Message).ConfigureAwait(false);
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
