using AspNetCoreServiceBusApi2.Model;
using ServiceBusMessaging;

namespace AspNetCoreServiceBusApi2;

public class ProcessData : IProcessData
{
    private readonly IConfiguration _configuration;

    public ProcessData(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task Process(MyPayload myPayload)
    {
        var connection = _configuration.GetConnectionString("DefaultConnection");
        if (connection == null) throw new ArgumentNullException(nameof(connection));

        using var payloadMessageContext = new PayloadMessageContext(connection);
        await payloadMessageContext.AddAsync(new Payload
        {
            Name = myPayload.Name,
            Goals = myPayload.Goals,
            Created = DateTime.UtcNow
        });

        await payloadMessageContext.SaveChangesAsync();
    }
}
