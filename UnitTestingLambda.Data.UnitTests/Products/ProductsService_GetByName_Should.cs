using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Moq;
using UnitTestingLinqLambda.Data.Products;
using Xunit;

namespace UnitTestingLinqLambda.Data.UnitTests.Products
{
    public sealed class ProductsService_GetByName_Should
    {
        private readonly Mock<IProductsRepository> _productsRepository = new Mock<IProductsRepository>();

        private readonly ProductsService _sut;

        public ProductsService_GetByName_Should()
        {
            _sut = new ProductsService(_productsRepository.Object);
        }

        [Fact]
        public async Task Return_ExpectedProducts_AsList()
        {
            // These are the productNames we're going to expect to be returned
            const string expectedProductName1 = "Poncho";
            const string expectedProductName2 = "Pontiac";
            
            // This is a list of Products that our lambda expression will iterate over looking for matches.
            var productsReturn = new List<Product>
            {
                new Product(Guid.Parse("00000000-0000-0000-0000-000000000001"), expectedProductName1, "A good poncho"),
                new Product(Guid.Parse("00000000-0000-0000-0000-000000000002"), "A product", "This thing is cool, but you'll never know it."),
                new Product(Guid.Parse("00000000-0000-0000-0000-000000000003"), expectedProductName2, "It's a car!"),
            };

            // This is the expected expression, which shoulkd
            Func<Expression<Func<Product, bool>>, bool> expectedExpression = exp =>
            {
                var func = exp.Compile();
                return func(productsReturn[0]) &&
                    !func(productsReturn[1]) &&
                    func(productsReturn[2]);
            };

            _productsRepository.Setup(p => p.ToListAsync(It.Is<Expression<Func<Product, bool>>>(exp => expectedExpression(exp))))
                .ReturnsAsync(new List<Product> { productsReturn[0], productsReturn[2] })
                .Verifiable();

            var result = await _sut.GetByName("Pon");

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }
    }
}
