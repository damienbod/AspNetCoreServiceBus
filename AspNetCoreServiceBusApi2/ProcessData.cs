using AspNetCoreServiceBusApi2.Model;
using ServiceBusMessaging;
using System;
using System.Threading.Tasks;

namespace AspNetCoreServiceBusApi2
{
    public class ProcessData : IProcessData
    {
        private readonly PayloadMessageContext _payloadMessageContext;

        public ProcessData(PayloadMessageContext payloadMessageContext)
        {
            _payloadMessageContext = payloadMessageContext;
        }
        public async Task Process(MyPayload myPayload)
        {
            await _payloadMessageContext.AddAsync(new Payload
            {
                Name = myPayload.Name,
                Goals = myPayload.Goals,
                Created = DateTime.UtcNow
            });

            await _payloadMessageContext.SaveChangesAsync();
        }
    }
}
