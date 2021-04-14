using System;

namespace UnitTestingLinqLambda.Data.Products
{
    /// <summary>
    ///     Example Product
    /// </summary>
    public sealed class Product
    {
        /// <summary>
        ///     Id
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        ///     Name
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        ///     Description
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        ///     Creates a new instance of <see cref="Product" />
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when name or description are empty.
        /// </exception>
        public Product(Guid id, string name, string description)
        {
            Id = id;
            Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentNullException(nameof(name)) : name;
            Description = string.IsNullOrWhiteSpace(description) ? throw new ArgumentNullException(nameof(description)) : description;
        }
    }
}
