using Microsoft.EntityFrameworkCore;

namespace AspNetCoreServiceBusApi2.Model;

public class PayloadMessageContext : DbContext
{
    private readonly string _connectionString;

    public DbSet<Payload> Payloads { get; set; }

    public PayloadMessageContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Payload>().Property(n => n.Id).ValueGeneratedOnAdd();
        builder.Entity<Payload>().HasKey(m => m.Id);
        base.OnModelCreating(builder);
    }
}