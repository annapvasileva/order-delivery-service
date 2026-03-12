using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using OrderDeliveryService.Infrastructure.Persistence.Migrations;
using Testcontainers.PostgreSql;

namespace OrderDeliveryService.Tests;

public class PostgreSqlFixture : IAsyncLifetime
{
    public PostgreSqlContainer Container { get; }

    public string ConnectionString => Container.GetConnectionString();

    public PostgreSqlFixture()
    {
        Container = new PostgreSqlBuilder()
            .WithImage("postgres:16")
            .WithDatabase("postgres-orders")
            .WithUsername("user")
            .WithPassword("pass")
            .Build();
    }

    public async Task InitializeAsync()
    {
        await Container.StartAsync();

        var services = new ServiceCollection();

        services.AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddPostgres()
                .WithGlobalConnectionString(ConnectionString)
                .ScanIn(typeof(InitialMigration).Assembly).For.Migrations());

        ServiceProvider serviceProvider = services.BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
    }

    public async Task DisposeAsync()
    {
        await Container.DisposeAsync();
    }
}