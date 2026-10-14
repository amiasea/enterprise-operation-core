using Moq;
using Wolverine;

using Amiasea.Enterprise.Contract.Core.Queries;
using ProductContract = Amiasea.Enterprise.Contract.Core.Types.Product;
using Amiasea.Enterprise.Operation.Core.Queries;
using GreenDonut.Data;
using HotChocolate.Types.Pagination;

namespace Amiasea.Enterprise.Operation.Core.Test;

public sealed class ProductQueriesTest
{
  [Fact]
  public async Task GetProductByIdAsync_DispatchesQueryAndReturnsProduct()
  {
    // Arrange
    var id = Guid.NewGuid();
    var expected = new ProductContract(id, "Widget", 12.50m);
    using var cts = new CancellationTokenSource();

    var bus = new Mock<IMessageBus>();

    bus
        .Setup(x => x.InvokeAsync<ProductContract?>(
            It.Is<GetProductByIdQuery>(q => q.Id == id),
            cts.Token))
        .ReturnsAsync(expected);

    // Act
    var result = await ProductQueries.GetProductByIdAsync(
        id,
        bus.Object,
        cts.Token);

    // Assert
    Assert.Same(expected, result);

    bus.Verify(x => x.InvokeAsync<ProductContract?>(
        It.Is<GetProductByIdQuery>(q => q.Id == id),
        cts.Token), Times.Once);
  }

  [Fact]
  public async Task GetProductsAsync_DispatchesQuery()
  {
    // Arrange
    var pagingArgs = new PagingArguments(first: 10);
    using var cts = new CancellationTokenSource();

    var bus = new Mock<IMessageBus>();

    bus
    .Setup(x => x.InvokeAsync<PageConnection<ProductContract>>(
        It.IsAny<GetProductsQuery>(),
        It.IsAny<CancellationToken>()))
    .ReturnsAsync((PageConnection<ProductContract>)null!);

    // Act
    await ProductQueries.GetProductsAsync(
        pagingArgs,
        bus.Object,
        cts.Token);

    // Assert
    bus.Verify(x => x.InvokeAsync<PageConnection<ProductContract>>(
        It.Is<GetProductsQuery>(query => query.Args == pagingArgs),
        cts.Token), Times.Once);
  }
}