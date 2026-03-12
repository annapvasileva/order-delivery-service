using Microsoft.Extensions.DependencyInjection;

namespace OrderDeliveryService.Presentation.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddEndpointsApiExplorer();
        serviceCollection.AddSwaggerGen();
        serviceCollection.ConfigureSwaggerGen(opt =>
        {
            opt.UseOneOfForPolymorphism();
            opt.SelectDiscriminatorNameUsing(_ => "$type");
        });

        return serviceCollection;
    }
}