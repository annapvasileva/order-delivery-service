using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrderDeliveryService.Application.Abstractions.Persistence.Repositories;
using OrderDeliveryService.Infrastructure.Persistence.DbContexts;
using OrderDeliveryService.Infrastructure.Persistence.Migrations;
using OrderDeliveryService.Infrastructure.Persistence.Options.Postgres;
using OrderDeliveryService.Infrastructure.Persistence.Repositories.Postgres;

namespace OrderDeliveryService.Infrastructure.Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPostgresRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }

    public static IServiceCollection AddPostgresMigrations(
        this IServiceCollection services)
    {
        services
            .AddFluentMigratorCore()
            .ConfigureRunner(runner => runner
                .AddPostgres()
                .WithGlobalConnectionString(sp =>
                {
                    PostgresOptions options = sp
                        .GetRequiredService<IOptions<PostgresOptions>>()
                        .Value;

                    return options.ConnectionString;
                })
                .WithMigrationsIn(typeof(InitialMigration).Assembly));

        services.AddHostedService<MigrationRunnerService>();

        return services;
    }

    public static IServiceCollection AddPostgresDbContext(
        this IServiceCollection services)
    {
        services.AddDbContext<PostgresDbContext>((sp, options) =>
        {
            PostgresOptions postgresOptions = sp
                .GetRequiredService<IOptions<PostgresOptions>>()
                .Value;

            options.UseNpgsql(postgresOptions.ConnectionString);
        });

        return services;
    }
}