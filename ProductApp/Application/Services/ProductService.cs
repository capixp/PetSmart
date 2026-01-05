using ProductApp.Application.Common;
using ProductApp.Application.Common.Exceptions;
using ProductApp.Application.Interfaces;
using ProductApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApp.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepo _productRepo;

        public ProductService(IProductRepo productRepo)
        {
            _productRepo = productRepo ?? throw new ArgumentNullException(nameof(productRepo));
        }

        public async Task AddProductAsync(Product product, CancellationToken cancellationToken)
        {
            ValidateProduct(product);

            // Note: uniqueness must be enforced at the persistence layer (e.g., unique constraints).
            // These checks are additional pre-validation for better error messaging.
            var existingBySku = await _productRepo
                .GetProductBySKUAsync(product!.SKU, cancellationToken)
                .ConfigureAwait(false);

            if (existingBySku is not null)
                throw new ConflictException($"A product with sku '{product.SKU}' already exists.");

            var existingByName = await _productRepo
                .GetProductByNameAsync(product.Name, cancellationToken)
                .ConfigureAwait(false);

            if (existingByName is not null)
                throw new ConflictException($"A product with name '{product.Name}' already exists.");

            await _productRepo
                .AddProductAsync(product, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task DeleteProductAsync(int id, CancellationToken cancellationToken)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Id must be greater than 0.");

            // Ensure the product exists before attempting deletion (better developer experience).
            var existing = await _productRepo
                .GetProductByIdAsync(id, cancellationToken)
                .ConfigureAwait(false);

            if (existing is null)
                throw new NotFoundException($"Product with id '{id}' was not found.");

            await _productRepo
                .DeleteProductAsync(id, cancellationToken)
                .ConfigureAwait(false);
        }

        public Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken)
        => _productRepo.GetAllProductsAsync(cancellationToken);

        public async Task<Product> GetProductByIdAsync(int id, CancellationToken cancellationToken)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "Id must be greater than 0.");

            var product = await _productRepo.GetProductByIdAsync(id, cancellationToken).ConfigureAwait(false);  // Avoid deadlocks
            if (product is null)
                throw new NotFoundException($"Product with id '{id}' was not found.");

            return product;
        }

        public async Task<Product> GetProductDetailsAsync(
            int productId = 0,
            int sku = 0,
            CancellationToken cancellationToken = default)
        {
            // Rule: exactly one identifier must be provided (either productId or sku).
            var hasId = productId > 0;
            var hasSku = sku > 0;

            if (!hasId && !hasSku)
                throw new ArgumentException("Either productId or sku must be provided.");

            if (hasId && hasSku)
                throw new ArgumentException("Provide either productId or sku, not both.");

            if (hasId)
            {
                var product = await _productRepo
                    .GetProductByIdAsync(productId, cancellationToken)
                    .ConfigureAwait(false);

                if (product is null)
                    throw new NotFoundException($"Product with id '{productId}' was not found.");

                return product;
            }

            var productBySku = await _productRepo
                .GetProductBySKUAsync(sku, cancellationToken)
                .ConfigureAwait(false);

            if (productBySku is null)
                throw new NotFoundException($"Product with sku '{sku}' was not found.");

            return productBySku;
        }

        public async Task UpdateProductAsync(int id, Product updatedProduct, CancellationToken cancellationToken)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id), "Id must be greater than 0.");

            ValidateProduct(updatedProduct);

            // Ensure the product exists before updating.
            var existing = await _productRepo
                .GetProductByIdAsync(id, cancellationToken)
                .ConfigureAwait(false);

            if (existing is null)
                throw new NotFoundException($"Product with id '{id}' was not found.");

            // Consistency: the route id is the source of truth.
            updatedProduct!.Id = id;

            // Note: uniqueness must be enforced at the persistence layer (e.g., unique constraints).
            // These checks are additional pre-validation for better error messaging.
            var bySku = await _productRepo
                .GetProductBySKUAsync(updatedProduct.SKU, cancellationToken)
                .ConfigureAwait(false);

            if (bySku is not null && bySku.Id != id)
                throw new ConflictException($"A product with sku '{updatedProduct.SKU}' already exists.");

            var byName = await _productRepo
                .GetProductByNameAsync(updatedProduct.Name, cancellationToken)
                .ConfigureAwait(false);

            if (byName is not null && byName.Id != id)
                throw new ConflictException($"A product with name '{updatedProduct.Name}' already exists.");

            await _productRepo
                .UpdateProductAsync(id, updatedProduct, cancellationToken)
                .ConfigureAwait(false);
        }

        private static void ValidateProduct(Product? product)
        {
            Guard.NotNull(product, nameof(product));
            Guard.NotNullOrWhiteSpace(product!.Name, nameof(product.Name));
            Guard.GreaterThan(product.Price, 0m, nameof(product.Price));
            Guard.GreaterThan(product.SKU, 0, nameof(product.SKU));
            Guard.NotNullOrWhiteSpace(product.Currency, nameof(product.Currency));
        }
    }
}
