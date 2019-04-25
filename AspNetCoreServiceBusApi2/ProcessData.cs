using AspNetCoreServiceBusApi2.Model;
using ServiceBusMessaging;
using System;

namespace AspNetCoreServiceBusApi2
{
    public class ProcessData : IProcessData
    {
        private readonly PayloadMessageContext _payloadMessageContext;

        public ProcessData(PayloadMessageContext payloadMessageContext)
        {
            _payloadMessageContext = payloadMessageContext;
        }
        public void Process(MyPayload myPayload)
        {
            _payloadMessageContext.AddAsync(new Payload
            {
                Name = myPayload.Name,
                Goals = myPayload.Goals,
                Created = DateTime.UtcNow
            });
        }
    }
}
