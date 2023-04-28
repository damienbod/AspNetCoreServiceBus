using AspNetCoreServiceBusApi2.Model;
using ServiceBusMessaging;

namespace AspNetCoreServiceBusApi2;

public class ProcessData : IProcessData
{
    private IConfiguration _configuration;

    public ProcessData(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task Process(MyPayload myPayload)
    {
        using (var payloadMessageContext =
            new PayloadMessageContext(
                _configuration.GetConnectionString("DefaultConnection")))
        {
            await payloadMessageContext.AddAsync(new Payload
            {
                Name = myPayload.Name,
                Goals = myPayload.Goals,
                Created = DateTime.UtcNow
            }).ConfigureAwait(false);

            await payloadMessageContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
