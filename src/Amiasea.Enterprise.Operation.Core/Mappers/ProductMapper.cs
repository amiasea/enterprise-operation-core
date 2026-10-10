using Riok.Mapperly.Abstractions;
using ProductEntity = Amiasea.Enterprise.Data.Core.Entity.Product;
using ProductContract = Amiasea.Enterprise.Contract.Core.Types.Product;

namespace Amiasea.Enterprise.Operation.Core.Mapper;

[Mapper]
public static partial class ProductMapper
{
    public static partial ProductContract ToContract(this ProductEntity source);
}