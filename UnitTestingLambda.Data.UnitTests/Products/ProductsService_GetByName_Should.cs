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
            // This is the filter we're going to pass to the method
            const string filter = "Pon";

            // These are the productNames we're going to expect to be returned
            const string expectedProductName1 = "A Poncho";
            const string expectedProductName2 = "Pontiac";
            
            // This is a list of Products that our lambda expression will iterate over looking for matches.
            var productsReturn = new List<Product>
            {
                new Product(1, expectedProductName1, "A good poncho"),
                new Product(2, "A product", "This thing is cool, but you'll never know it."),
                new Product(3, expectedProductName2, "It's a car!"),
            };

            // This is the expected expression. 
            // 
            // This is where we define which of the above products we expect to be returned from the expression.
            Func<Expression<Func<Product, bool>>, bool> expectedExpression = exp =>
            {
                var func = exp.Compile();
                
                return func(productsReturn[0]) &&
                    !func(productsReturn[1]) &&
                    func(productsReturn[2]);
            };

            // On .Setup we can use It.Is to match the expression above and return the expected results.
            _productsRepository.Setup(p => p.ToListAsync(It.Is<Expression<Func<Product, bool>>>(exp => expectedExpression(exp))))
                .ReturnsAsync(new List<Product> { productsReturn[0], productsReturn[2] })
                .Verifiable();

            // Act
            var results = await _sut.GetByName(filter);

            // Assert
            Assert.NotNull(results);
            Assert.Equal(2, results.Count);
            Assert.Equal(expectedProductName1, results[0].Name);
            Assert.Equal(expectedProductName2, results[1].Name);

            _productsRepository.Verify();
        }
    }
}
