using HotChocolate.Types;
using Wolverine;

using Amiasea.Enterprise.Contract.Core.Commands;
using Amiasea.Enterprise.Contract.Core.Types;

namespace Amiasea.Enterprise.Operation.Core.Mutations;

[MutationType]
public static partial class ProductMutations
{
  public static async Task<Product> CreateProductAsync(
      string name,
      decimal price,
      IMessageBus bus,
      CancellationToken cancellationToken)
  {
    ArgumentException.ThrowIfNullOrWhiteSpace(name);

    if (price < 0)
    {
      throw new ArgumentOutOfRangeException(
          nameof(price),
          "Product price cannot be negative.");
    }

    return await bus.InvokeAsync<Product>(
        new CreateProductCommand(name, price),
        cancellationToken);
  }
}