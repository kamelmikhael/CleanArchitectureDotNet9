using System.Threading.Tasks;
using Application.Abstractions.Messaging;
using Application.Orders.Create;
using Application.Products.Create;
using Domain.Products;
using Microsoft.Extensions.DependencyInjection;
using SharedKernal.Primitives;

namespace Application.IntegrationTests;

public class ProductsTests : BaseIntegrationTest
{
    private readonly ICommandHandler<CreateProduct.Command, Guid> handler;

    public ProductsTests(IntegrationTestsWebAppFactory factory) : base(factory)
    {
        handler = _scope.ServiceProvider.GetRequiredService<ICommandHandler<CreateProduct.Command, Guid>>();
    }

    //[Fact]
    //public async Task Create_Should_Fail_WhenInvalidSku()
    //{
    //    // Arrange
    //    CreateProduct.Command command = new("Database", "USD", 99.99m, "123");

    //    // Act
    //    Task Action() => handler.Handle(command, default);

    //    // Assert
    //    await Assert.ThrowsAsync<Exception>(Action);
    //}

    //[Fact]
    //public async Task Create_Should_AddNewProductToDatabase()
    //{
    //    // Arrange
    //    CreateProduct.Command command = new("Database", "USD", 99.99m, "123");

    //    // Act
    //    Result<Guid> result = await handler.Handle(command, default);

    //    Product? product = _dbContext.Set<Product>().FirstOrDefault(p => p.Id == new ProductId(result.Value));

    //    // Assert
    //    Assert.NotNull(product);
    //}
}
