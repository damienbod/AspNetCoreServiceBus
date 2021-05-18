# ASP.NET Core Azure Sevice Bus


<ul>
	<li><a href="https://damienbod.com/2019/04/23/using-azure-service-bus-queues-with-asp-net-core-services/">Using Azure Service Bus Queues with ASP.NET Core Services</a></li>
	<li><a href="https://damienbod.com/2019/04/24/using-azure-service-bus-topics-in-asp-net-core/">Using Azure Service Bus Topics in ASP.NET Core</a></li>
	<li><a href="https://damienbod.com/2019/04/27/using-azure-service-bus-topics-subscription-filters-in-asp-net-core/">Using Azure Service Bus Topics Subscription Filters in ASP.NET Core</a></li>
	<li><a href="https://damienbod.com/2019/04/30/using-ef-core-to-process-azure-service-messages-in-asp-net-core/">Using Entity Framework Core to process Azure Service Messages in ASP.NET Core</a></li>
	<li><a href="https://damienbod.com/2019/05/03/using-an-azure-service-bus-topic-subscription-in-an-azure-function/">Using an Azure Service Bus Topic Subscription in an Azure Function</a></li>
	<li><a href="https://damienbod.com/2020/03/06/using-azure-service-bus-with-restricted-access/">Using Azure Service Bus with restricted access</a></li>
</ul>

user secrets
```
{
  "ConnectionStrings": {
    "ServiceBusConnectionString": "--your-servicebus-connection--string--"
  }
}
```
## History

2020-03-06 Added restricted access example

2019-12-08 Updated to .NET Core 3.1

## Links

https://docs.microsoft.com/en-us/azure/service-bus-messaging/

https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-dotnet-get-started-with-queues

https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/multi-container-microservice-net-applications/integration-event-based-microservice-communications

https://www.nuget.org/packages/Microsoft.Azure.ServiceBus

https://www.planetgeek.ch/2019/01/07/azure-service-bus-topologies/

https://connectedcircuits.blog/2019/04/24/always-subscribe-to-dead-lettered-messages-in-an-azure-service-bus/

https://www.tomfaltesek.com/azure-functions-local-settings-json-and-source-control/

https://www.ben-morris.com/optimizing-performance-of-the-azure-service-bus-net-standard-sdk/

https://ml-software.ch/posts/stripe-api-with-asp-net-core-part-3

https://www.serverless360.com/blog/auto-forwarding-a-hidden-gem-of-service-bus

https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-tutorial-topics-subscriptions-cli

https://github.com/paolosalvatori/ServiceBusExplorer/

https://docs.microsoft.com/en-us/cli/azure/install-azure-cli
