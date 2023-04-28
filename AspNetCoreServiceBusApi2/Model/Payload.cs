using System.ComponentModel.DataAnnotations;

namespace AspNetCoreServiceBusApi2.Model;

public class Payload
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Goals { get; set; }
    public DateTime Created { get; set; }
}
