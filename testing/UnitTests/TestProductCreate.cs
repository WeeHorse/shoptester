using server.Records;

namespace testing.UnitTests;

public class TestProductCreate
{
    [Fact]
    public void TestCreate()
    {
        var product = new ProductCreate("Testproduct",999,1,null,null);
        Assert.Equal("Testproduct", product.Name);
        Assert.Equal(999, product.Price);
        Assert.Equal(1, product.CategoryId);
        Assert.Null(product.Description);
        Assert.Null(product.Image_url);
    }
}