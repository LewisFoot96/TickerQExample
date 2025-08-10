using Microsoft.EntityFrameworkCore;
using TickerQ.EntityFrameworkCore.Configurations;

namespace TickerQExample
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options)
       : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply TickerQ entity configurations explicitly
            modelBuilder.ApplyConfiguration(new TimeTickerConfigurations());
            modelBuilder.ApplyConfiguration(new CronTickerConfigurations());
            modelBuilder.ApplyConfiguration(new CronTickerOccurrenceConfigurations());

            // Alternatively, apply all configurations from assembly:
            // builder.ApplyConfigurationsFromAssembly(typeof(TimeTickerConfigurations).Assembly);
        }
    }

}
