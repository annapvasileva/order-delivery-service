using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace OrderManagementService.Infrastructure.Persistence.Extensions;

public static class ServiceProviderExtensions
{
    public static async Task<IServiceProvider> MigrateUp(this IServiceProvider serviceProvider)
    {
        await using AsyncServiceScope scope = serviceProvider.CreateAsyncScope();
        IMigrationRunner migrationRunner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        migrationRunner.MigrateUp();

        return serviceProvider;
    }
}