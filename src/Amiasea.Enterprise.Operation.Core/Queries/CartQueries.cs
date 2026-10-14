using HotChocolate.Types;
using Wolverine;

using Amiasea.Enterprise.Contract.Core.Queries;
using CartContract = Amiasea.Enterprise.Contract.Core.Types.Cart;

namespace Amiasea.Enterprise.Operation.Core.Queries;

[QueryType]
public static partial class CartQueries
{
  public static Task<CartContract?> GetCartByIdAsync(
      Guid id,
      IMessageBus bus,
      CancellationToken cancellationToken)
  {
    return bus.InvokeAsync<CartContract?>(
        new GetCartByIdQuery(id),
        cancellationToken);
  }
}