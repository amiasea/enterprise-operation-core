using GreenDonut.Data;
using HotChocolate.Types;
using HotChocolate.Types.Pagination;

using ProductContract = Amiasea.Enterprise.Contract.Core.Types.Product;
using Wolverine;
using Amiasea.Enterprise.Contract.Core.Queries;
using Amiasea.Enterprise.Contract.Core.Types;

namespace Amiasea.Enterprise.Operation.Core.Queries;

[QueryType]
public static partial class ProductQueries
{
  public static Task<ProductContract?> GetProductByIdAsync(
      Guid id,
      IMessageBus bus,
      CancellationToken cancellationToken)
  {
    return bus.InvokeAsync<ProductContract?>(
        new GetProductByIdQuery(id),
        cancellationToken);
  }

  public static Task<PageConnection<ProductContract>> GetProductsAsync(
        PagingArguments pagingArgs,
        IMessageBus bus,
        CancellationToken cancellationToken)
    {
        return bus.InvokeAsync<PageConnection<ProductContract>>(
            new GetProductsQuery(pagingArgs),
            cancellationToken);
    }
}