using Microsoft.EntityFrameworkCore;
using OrderDeliveryService.Infrastructure.Persistence.DbContexts;

namespace OrderDeliveryService.Tests;

public static class DbContextFactory
{
    public static PostgresDbContext Create(string connectionString)
    {
        var options = new DbContextOptionsBuilder<PostgresDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        return new PostgresDbContext(options);
    }
}