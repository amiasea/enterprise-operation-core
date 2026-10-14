using Moq;
using Wolverine;

using Amiasea.Enterprise.Contract.Core.Commands;
using Product = Amiasea.Enterprise.Contract.Core.Types.Product;
using Amiasea.Enterprise.Operation.Core.Mutations;

namespace Amiasea.Enterprise.Operation.Core.Test;

public sealed class ProductMutationsTest
{
  [Fact]
  public async Task CreateProductAsync_DispatchesCommandAndReturnsProduct()
  {
    // Arrange
    const string name = "Widget";
    const decimal price = 12.50m;

    var expected = new Product(
        Guid.NewGuid(),
        name,
        price);

    using var cts = new CancellationTokenSource();
    var bus = new Mock<IMessageBus>();

    bus
        .Setup(x => x.InvokeAsync<Product>(
            It.IsAny<CreateProductCommand>(),
            cts.Token))
        .ReturnsAsync(expected);

    // Act
    var result = await ProductMutations.CreateProductAsync(
        name,
        price,
        bus.Object,
        cts.Token);

    // Assert
    Assert.Same(expected, result);

    bus.Verify(x => x.InvokeAsync<Product>(
        It.Is<CreateProductCommand>(command =>
            command.Name == name &&
            command.Price == price),
        cts.Token), Times.Once);
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  [InlineData("   ")]
  public async Task CreateProductAsync_RejectsInvalidName(string? name)
  {
    // Arrange
    var bus = new Mock<IMessageBus>();

    // Act
    await Assert.ThrowsAnyAsync<ArgumentException>(() =>
      ProductMutations.CreateProductAsync(
          name!,
          12.50m,
          bus.Object,
          CancellationToken.None));
  }

  [Fact]
  public async Task CreateProductAsync_RejectsNegativePrice()
  {
    // Arrange
    var bus = new Mock<IMessageBus>();

    // Act
    var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(
        () => ProductMutations.CreateProductAsync(
            "Widget",
            -1m,
            bus.Object,
            CancellationToken.None));

    // Assert
    Assert.Equal("price", exception.ParamName);
    bus.VerifyNoOtherCalls();
  }
}