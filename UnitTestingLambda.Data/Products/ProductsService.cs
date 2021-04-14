using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnitTestingLinqLambda.Data.Products
{
    public sealed class ProductsService
    {
        private readonly IProductsRepository _productsRepository;

        /// <summary>
        ///     Creates a new instance of <see cref="ProductsService"/>
        /// </summary>
        /// <param name="productsRepository"></param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when an injected param is null.
        /// </exception>
        public ProductsService(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository ?? throw new ArgumentNullException(nameof(productsRepository));
        }

        /// <summary>
        ///     Finds products by a given filter on name.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<List<Product>> GetByName(string input)
        {
            return await _productsRepository
                .ToListAsync(p => p.Name.Contains(input));
        }
    }
}
