[![.NET](https://github.com/damienbod/AspNetCoreServiceBus/actions/workflows/dotnet.yml/badge.svg)](https://github.com/damienbod/AspNetCoreServiceBus/actions/workflows/dotnet.yml)

# ASP.NET Core Azure Service Bus

- [Using Azure Service Bus Queues with ASP.NET Core Services](https://damienbod.com/2019/04/23/using-azure-service-bus-queues-with-asp-net-core-services/)
- [Using Azure Service Bus Topics in ASP.NET Core](https://damienbod.com/2019/04/24/using-azure-service-bus-topics-in-asp-net-core/)
- [Using Azure Service Bus Topics Subscription Filters in ASP.NET Core](https://damienbod.com/2019/04/27/using-azure-service-bus-topics-subscription-filters-in-asp-net-core/)
- [Using Entity Framework Core to process Azure Service Messages in ASP.NET Core](https://damienbod.com/2019/04/30/using-ef-core-to-process-azure-service-messages-in-asp-net-core/)
- [Using an Azure Service Bus Topic Subscription in an Azure Function](https://damienbod.com/2019/05/03/using-an-azure-service-bus-topic-subscription-in-an-azure-function/)
- [Using Azure Service Bus with restricted access](https://damienbod.com/2020/03/06/using-azure-service-bus-with-restricted-access/)
- [Using an ASP.NET Core IHostedService to run Azure Service Bus subscriptions and consumers](https://damienbod.com/2021/07/20/using-an-asp-net-core-ihostedservice-to-run-azure-service-bus-subscriptions-and-consumers/)

user secrets
```
{
  "ConnectionStrings": {
    "ServiceBusConnectionString": "--your-servicebus-connection--string--"
  }
}
```

## History

- 2024-06-30 Updated Azure function .NET 8 isolated v4
- 2024-06-29 Updated packages
- 2023-11-24 Updated .NET 8
- 2023-11-03 Updated packages
- 2023-04-28 Updated .NET 7
- 2021-11-08 Updated .NET 6
- 2021-07-20 Added IHostedService to consumer the message in the ASP.NET Core app
- 2021-05-19 Updated to .NET 5, migrated to Azure.Messaging.ServiceBus, switched Json serializer
- 2020-03-06 Added restricted access example
- 2019-12-08 Updated to .NET Core 3.1

## Links

https://github.com/Azure/azure-sdk-for-net/tree/master/sdk/servicebus/Azure.Messaging.ServiceBus

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
