using Amiasea.Enterprise.Data.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Amiasea.Enterprise.Operation.Core;

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