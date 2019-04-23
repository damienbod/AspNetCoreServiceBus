using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceBusMessaging
{
    public interface IServiceBusTopicSubscription
    {
        Task PrepareFiltersAndHandleMessages();
        Task CloseQueueAsync();
    }

    public class ServiceBusTopicSubscription : IServiceBusTopicSubscription
    {
        private readonly IProcessData _processData;
        private readonly IConfiguration _configuration;
        private readonly SubscriptionClient _subscriptionClient;
        private const string TOPIC_PATH = "mytopic";
        private const string SUBSCRIPTION_NAME = "mytopicsubscription";
        private readonly ILogger _logger;

        public ServiceBusTopicSubscription(IProcessData processData, 
            IConfiguration configuration, 
            ILogger<ServiceBusTopicSubscription> logger)
        {
            _processData = processData;
            _configuration = configuration;
            _logger = logger;

            _subscriptionClient = new SubscriptionClient(
                _configuration.GetConnectionString("ServiceBusConnectionString"), 
                TOPIC_PATH, 
                SUBSCRIPTION_NAME);
        }

        public async Task PrepareFiltersAndHandleMessages()
        {
            await RemoveDefaultFilters();
            await AddFilters();

            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false,
            };

            _subscriptionClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        private async Task RemoveDefaultFilters()
        {
            try
            {
                await _subscriptionClient.RemoveRuleAsync(RuleDescription.DefaultRuleName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private async Task AddFilters()
        {
            try
            {
                var filter = new SqlFilter("goals > 7");
                await _subscriptionClient.AddRuleAsync("GoalsGreaterThanSeven", filter);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            var myPayload = JsonConvert.DeserializeObject<MyPayload>(Encoding.UTF8.GetString(message.Body));
            _processData.Process(myPayload);
            await _subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            _logger.LogError(exceptionReceivedEventArgs.Exception, "Message handler encountered an exception");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;

            _logger.LogDebug($"- Endpoint: {context.Endpoint}");
            _logger.LogDebug($"- Entity Path: {context.EntityPath}");
            _logger.LogDebug($"- Executing Action: {context.Action}");

            return Task.CompletedTask;
        }

        public async Task CloseQueueAsync()
        {
            await _subscriptionClient.CloseAsync();
        }
    }
}
