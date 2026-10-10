using Microsoft.EntityFrameworkCore;
using HotChocolate.Types;
using Amiasea.Enterprise.Data.Core;
using Amiasea.Enterprise.Data.Core.DataLoader;
using Amiasea.Enterprise.Operation.Core.Mapper;
using CartContract = Amiasea.Enterprise.Contract.Core.Types.Cart;
using CartItemContract = Amiasea.Enterprise.Contract.Core.Types.CartItem;

namespace Amiasea.Enterprise.Operation.Core.Queries;

[QueryType]
public static partial class CartQueries
{
    public static async Task<CartContract?> GetCartByIdAsync(
        Guid id,
        CoreDbContext db,
        IProductByIdDataLoader productsById,
        CancellationToken cancellationToken)
    {
        var cart = await db.Carts
            .AsNoTracking()
            .Include(cart => cart.Items)
            .SingleOrDefaultAsync(
                cart => cart.Id == id,
                cancellationToken);

        if (cart is null)
        {
            return null;
        }

        var items = await Task.WhenAll(
            cart.Items.Select(async item =>
            {
                var product = await productsById.LoadAsync(
                    item.ProductId,
                    cancellationToken);

                if (product is null)
                {
                    throw new InvalidOperationException(
                        $"Product '{item.ProductId}' referenced by cart '{cart.Id}' was not found.");
                }

                return new CartItemContract(
                    product.ToContract(),
                    item.Quantity);
            }));

        return new CartContract(cart.Id, items);
    }
}