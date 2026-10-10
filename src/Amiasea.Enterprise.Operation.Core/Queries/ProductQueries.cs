using GreenDonut.Data;
using HotChocolate.Types;
using HotChocolate.Types.Pagination;
using Microsoft.EntityFrameworkCore;

using Amiasea.Enterprise.Data.Core;
using Amiasea.Enterprise.Data.Core.DataLoader;
using Amiasea.Enterprise.Operation.Core.Mapper;

using ProductContract = Amiasea.Enterprise.Contract.Core.Types.Product;

namespace Amiasea.Enterprise.Operation.Core.Queries;

[QueryType]
public static partial class ProductQueries
{
    public static async Task<ProductContract?> GetProductByIdAsync(
        Guid id,
        IProductByIdDataLoader productsById,
        CancellationToken cancellationToken)
    {
        var product = await productsById.LoadAsync(id, cancellationToken);

        return product?.ToContract();
    }

    public static async Task<PageConnection<ProductContract>> GetProductsAsync(
        PagingArguments pagingArgs,
        CoreDbContext db,
        CancellationToken cancellationToken)
    {
        return await db.Products
            .AsNoTracking()
            .OrderBy(product => product.Id)
            .Select(product => new ProductContract(
                product.Id,
                product.Name,
                product.Price))
            .ToPageAsync(pagingArgs, cancellationToken);
    }
}