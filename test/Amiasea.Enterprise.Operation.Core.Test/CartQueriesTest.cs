using Moq;
using Wolverine;

using Amiasea.Enterprise.Contract.Core.Queries;
using CartContract = Amiasea.Enterprise.Contract.Core.Types.Cart;
using Amiasea.Enterprise.Operation.Core.Queries;

namespace Amiasea.Enterprise.Operation.Core.Test;

public sealed class CartQueriesTest
{
    [Fact]
    public async Task GetCartByIdAsync_DispatchesQuery()
    {
        // Arrange
        var id = Guid.NewGuid();
        using var cts = new CancellationTokenSource();

        var bus = new Mock<IMessageBus>();

        bus
            .Setup(x => x.InvokeAsync<CartContract?>(
                It.IsAny<GetCartByIdQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((CartContract?)null);

        // Act
        await CartQueries.GetCartByIdAsync(
            id,
            bus.Object,
            cts.Token);

        // Assert
        bus.Verify(x => x.InvokeAsync<CartContract?>(
            It.Is<GetCartByIdQuery>(query => query.Id == id),
            cts.Token), Times.Once);
    }
}