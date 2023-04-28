using Microsoft.EntityFrameworkCore;

namespace AspNetCoreServiceBusApi2.Model;

public class PayloadContext : DbContext
{
    public PayloadContext(DbContextOptions<PayloadContext> options) : base(options)
    {
    }

    public DbSet<Payload> Payloads { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Payload>().Property(n => n.Id).ValueGeneratedOnAdd();
        builder.Entity<Payload>().HasKey(m => m.Id);
        base.OnModelCreating(builder);
    }
}