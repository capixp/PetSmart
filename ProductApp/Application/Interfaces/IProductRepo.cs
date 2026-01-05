using ProductApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApp.Application.Interfaces
{
    public interface IProductRepo
    {
        /// <summary>
        /// Retrieves all the product details by Id
        /// </summary>
        /// <returns></returns>
        Task<Product> GetProductByIdAsync(int id, CancellationToken cancellationToken);
        /// <summary>
        /// Retrieves all the product details by name
        /// </summary>
        /// <returns></returns>
        Task<Product> GetProductByNameAsync(string name, CancellationToken cancellationToken);
        /// <summary>
        /// Retrieves all the product details by Sku
        /// </summary>
        /// <returns></returns>
        Task<Product> GetProductBySKUAsync(int sku, CancellationToken cancellationToken);
        /// <summary>
        /// Retrieves all the products
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken);
        /// <summary>
        /// save product details
        /// </summary>
        /// <returns></returns>
        Task AddProductAsync(Product product, CancellationToken cancellationToken);
        /// <summary>
        /// update product by productId all the products
        /// </summary>
        /// <returns></returns>
        Task UpdateProductAsync(int Id, Product product, CancellationToken cancellationToken);
        /// <summary>
        /// Delete product by product id
        /// </summary>
        /// <returns></returns>
        Task DeleteProductAsync(int id, CancellationToken cancellationToken);
    }
}

