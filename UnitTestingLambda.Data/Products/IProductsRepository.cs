using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace UnitTestingLinqLambda.Data.Products
{
    /// <summary>
    ///     Example product repository interface
    /// </summary>
    public interface IProductsRepository
    {
        Task<List<Product>> ToListAsync(Expression<Func<Product, bool>> predicate);
    }
}
