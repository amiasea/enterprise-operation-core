using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;

using Amiasea.Enterprise.Data.Core;
using Amiasea.Enterprise.Data.Core.Entity;

using ProductContract = Amiasea.Enterprise.Contract.Core.Types.Product;

namespace Amiasea.Enterprise.Operation.Core.Mutations;

[MutationType]
public static partial class ProductMutations
{
    public static async Task<ProductContract> CreateProductAsync(
        string name,
        decimal price,
        CoreDbContext db,
        CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        if (price < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(price),
                "Product price cannot be negative.");
        }

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = name,
            Price = price
        };

        db.Products.Add(product);
        await db.SaveChangesAsync(cancellationToken);

        return new ProductContract(
            product.Id,
            product.Name,
            product.Price);
    }
}