using Microsoft.EntityFrameworkCore;
using OrderDeliveryService.Application.Models.Orders;

namespace OrderDeliveryService.Infrastructure.Persistence.DbContexts;

public class PostgresDbContext : DbContext
{
    public DbSet<Order> Orders => Set<Order>();

    public PostgresDbContext(DbContextOptions<PostgresDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("orders");

            entity.HasKey(x => x.OrderId);

            entity.Property(x => x.OrderId)
                .HasColumnName("order_id")
                .ValueGeneratedOnAdd();

            entity.Property(x => x.SendersCity)
                .HasColumnName("senders_city");

            entity.Property(x => x.SendersAddress)
                .HasColumnName("senders_address");

            entity.Property(x => x.RecipientsCity)
                .HasColumnName("recipients_city");

            entity.Property(x => x.RecipientsAddress)
                .HasColumnName("recipients_address");

            entity.Property(x => x.CargoWeight)
                .HasColumnName("cargo_weight");

            entity.Property(x => x.CargoCollectionDate)
                .HasColumnName("cargo_collection_date");
        });
    }
}