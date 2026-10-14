using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

public static class OperationCoreServiceCollectionExtensions
{
    public static IServiceCollection AddCoreOperations(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddCoreData(configuration);

        return services;
    }
}