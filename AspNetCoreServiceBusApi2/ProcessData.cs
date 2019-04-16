using AspNetCoreServiceBusApi2.Model;
using ServiceBusMessaging;

namespace AspNetCoreServiceBusApi2
{
    public class ProcessData : IProcessData
    {
        public void Process(MyPayload myPayload)
        {
            DataServiceSimi.Data.Add(new Payload
            {
                Name = myPayload.Name,
                Goals = myPayload.Goals
            });
        }
    }
}
