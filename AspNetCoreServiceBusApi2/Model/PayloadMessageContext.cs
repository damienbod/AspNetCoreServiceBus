using Microsoft.EntityFrameworkCore;

namespace AspNetCoreServiceBusApi2.Model
{
    public class PayloadMessageContext : DbContext
    {
        public PayloadMessageContext(DbContextOptions<PayloadMessageContext> options) : base(options)
        {
        }
        
        public DbSet<Payload> Payloads { get; set; }
      
        protected override void OnModelCreating(ModelBuilder builder)
        { 
            builder.Entity<Payload>().HasKey(m => m.Id); 
            base.OnModelCreating(builder); 
        } 
    }
}