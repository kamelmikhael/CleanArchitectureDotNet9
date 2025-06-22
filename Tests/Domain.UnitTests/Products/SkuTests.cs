using Domain.Products;

namespace Domain.UnitTests.Products;

public class SkuTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Create_Should_ReturnNull_WhenValueIsNullOrEmpty(string? value)
    {
        // Arrange

        // Act
        var sku = Sku.Create(value);

        // Assert
        Assert.Null(sku);
    }

    public static IEnumerable<object[]> InvalidSkuLengthData = new List<object[]>
    {
        new object[] { "invalid_sku_1" },
        new object[] { "invalid_sku_2" },
        new object[] { "invalid_sku_3" },
    };

    public static IEnumerable<object[]> InvalidSkuLengthDataProp => new List<object[]>
    {
        new object[] { "invalid_sku_4" },
        new object[] { "invalid_sku_5" },
        new object[] { "invalid_sku_6" },
    };

    public static IEnumerable<object[]> InvalidSkuLengthDataMethod() => new List<object[]>
    {
        new object[] { "invalid_sku_7" },
        new object[] { "invalid_sku_8" },
        new object[] { "invalid_sku_9" },
    };

    [Theory]
    [MemberData(nameof(InvalidSkuLengthData))]
    [MemberData(nameof(InvalidSkuLengthDataProp))]
    [MemberData(nameof(InvalidSkuLengthDataMethod))]
    public void Create_Should_ReturnNull_WhenValueLengthIsInvalid(string value)
    {
        // Arrange

        // Act
        var sku = Sku.Create(value);

        // Assert
        Assert.Null(sku);
    }
}
