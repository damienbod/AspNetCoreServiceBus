namespace ServiceBusMessaging;

public interface IServiceBusTopicSubscription
{
    Task PrepareFiltersAndHandleMessages();
    Task CloseSubscriptionAsync();
    ValueTask DisposeAsync();
}
