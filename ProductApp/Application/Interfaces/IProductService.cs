using ProductApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApp.Application.Interfaces
{
    /// <summary>
    /// The product service interface
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Retrieves all the products
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken);
        Task<Product> GetProductByIdAsync(int id, CancellationToken cancellationToken);
        Task<Product> GetProductDetailsAsync(int productId = 0, int sku = 0, CancellationToken cancellationToken = default);
        Task AddProductAsync(Product product, CancellationToken cancellationToken);
        Task UpdateProductAsync(int id, Product updatedProduct, CancellationToken cancellationToken);
        Task DeleteProductAsync(int id, CancellationToken cancellationToken);
    }
}
