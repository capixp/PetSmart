using Moq;
using ProductApp.Application.Common.Exceptions;
using ProductApp.Application.Interfaces;
using ProductApp.Application.Services;
using ProductApp.Domain;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ProductApp.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly ProductService _sut;
        private readonly Mock<IProductRepo> _repo;

        public ProductServiceTests()
        {
            _repo = new Mock<IProductRepo>(MockBehavior.Strict);
            _sut = new ProductService(_repo.Object);
        }

        // -------------------------
        // GetAllProductsAsync
        // -------------------------

        [Fact]
        public async Task GetAllProductsAsync_ReturnsProducts()
        {
            var ct = CancellationToken.None;
            var expected = new List<Product>
            {
                ValidProduct(id: 1, sku: 100),
                ValidProduct(id: 2, sku: 200),
            };

            _repo.Setup(r => r.GetAllProductsAsync(ct))
                 .ReturnsAsync(expected);

            var result = await _sut.GetAllProductsAsync(ct);

            Assert.Same(expected, result);
            _repo.VerifyAll();
            _repo.VerifyNoOtherCalls();
        }

        // -------------------------
        // GetProductByIdAsync
        // -------------------------

        [Fact]
        public async Task GetProductByIdAsync_WhenIdInvalid_ThrowsArgumentOutOfRangeException_AndDoesNotHitRepo()
        {
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                _sut.GetProductByIdAsync(0, CancellationToken.None));

            _repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetProductByIdAsync_WhenNotFound_ThrowsNotFoundException()
        {
            var ct = CancellationToken.None;

            _repo.Setup(r => r.GetProductByIdAsync(10, ct))
                 .ReturnsAsync((Product)null!);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _sut.GetProductByIdAsync(10, ct));

            _repo.VerifyAll();
            _repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetProductByIdAsync_WhenFound_ReturnsProduct()
        {
            var ct = CancellationToken.None;
            var expected = ValidProduct(id: 10, sku: 555);

            _repo.Setup(r => r.GetProductByIdAsync(10, ct))
                 .ReturnsAsync(expected);

            var result = await _sut.GetProductByIdAsync(10, ct);

            Assert.Same(expected, result);
            _repo.VerifyAll();
            _repo.VerifyNoOtherCalls();
        }

        // -------------------------
        // GetProductDetailsAsync
        // -------------------------

        [Fact]
        public async Task GetProductDetailsAsync_WhenNeitherIdNorSkuProvided_ThrowsArgumentException_AndDoesNotHitRepo()
        {
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _sut.GetProductDetailsAsync(productId: 0, sku: 0, cancellationToken: CancellationToken.None));

            _repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetProductDetailsAsync_WhenBothIdAndSkuProvided_ThrowsArgumentException_AndDoesNotHitRepo()
        {
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _sut.GetProductDetailsAsync(productId: 1, sku: 2, cancellationToken: CancellationToken.None));

            _repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetProductDetailsAsync_ById_WhenNotFound_ThrowsNotFoundException()
        {
            var ct = CancellationToken.None;

            _repo.Setup(r => r.GetProductByIdAsync(7, ct))
                 .ReturnsAsync((Product)null!);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _sut.GetProductDetailsAsync(productId: 7, sku: 0, cancellationToken: ct));

            _repo.VerifyAll();
            _repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetProductDetailsAsync_ById_WhenFound_ReturnsProduct_AndDoesNotCallSkuLookup()
        {
            var ct = CancellationToken.None;
            var expected = ValidProduct(id: 7, sku: 123);

            _repo.Setup(r => r.GetProductByIdAsync(7, ct))
                 .ReturnsAsync(expected);

            var result = await _sut.GetProductDetailsAsync(productId: 7, sku: 0, cancellationToken: ct);

            Assert.Same(expected, result);

            _repo.VerifyAll();
            _repo.Verify(r => r.GetProductBySKUAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
            _repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetProductDetailsAsync_BySku_WhenNotFound_ThrowsNotFoundException()
        {
            var ct = CancellationToken.None;

            _repo.Setup(r => r.GetProductBySKUAsync(222, ct))
                 .ReturnsAsync((Product)null!);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _sut.GetProductDetailsAsync(productId: 0, sku: 222, cancellationToken: ct));

            _repo.VerifyAll();
            _repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetProductDetailsAsync_BySku_WhenFound_ReturnsProduct_AndDoesNotCallIdLookup()
        {
            var ct = CancellationToken.None;
            var expected = ValidProduct(id: 9, sku: 222);

            _repo.Setup(r => r.GetProductBySKUAsync(222, ct))
                 .ReturnsAsync(expected);

            var result = await _sut.GetProductDetailsAsync(productId: 0, sku: 222, cancellationToken: ct);

            Assert.Same(expected, result);

            _repo.VerifyAll();
            _repo.Verify(r => r.GetProductByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
            _repo.VerifyNoOtherCalls();
        }

        // -------------------------
        // AddProductAsync
        // -------------------------

        [Fact]
        public async Task AddProductAsync_WhenProductNull_ThrowsArgumentNullException_AndDoesNotHitRepo()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _sut.AddProductAsync(null!, CancellationToken.None));

            _repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task AddProductAsync_WhenProductHasInvalidTextFields_ThrowsArgumentException_AndDoesNotHitRepo()
        {
            var invalid = new Product
            {
                Id = 0,
                Name = "",
                Price = 10m,
                SKU = 1,
                Currency = ""
            };

            await Assert.ThrowsAsync<ArgumentException>(() =>
                _sut.AddProductAsync(invalid, CancellationToken.None));

            _repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task AddProductAsync_WhenProductHasInvalidNumericFields_ThrowsArgumentOutOfRangeException_AndDoesNotHitRepo()
        {
            var invalid = new Product
            {
                Id = 0,
                Name = "Valid Name",
                Price = 0m,    // invalid
                SKU = 0,       // invalid
                Currency = "USD"
            };

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                _sut.AddProductAsync(invalid, CancellationToken.None));

            _repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task AddProductAsync_WhenSkuAlreadyExists_ThrowsConflictException_AndDoesNotAdd()
        {
            var ct = CancellationToken.None;
            var p = ValidProduct(id: 0, sku: 1000);

            _repo.Setup(r => r.GetProductBySKUAsync(p.SKU, ct))
                 .ReturnsAsync(ValidProduct(id: 99, sku: p.SKU));

            await Assert.ThrowsAsync<ConflictException>(() =>
                _sut.AddProductAsync(p, ct));

            _repo.VerifyAll();
            _repo.Verify(r => r.AddProductAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Never);
            _repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task AddProductAsync_WhenNameAlreadyExists_ThrowsConflictException_AndDoesNotAdd()
        {
            var ct = CancellationToken.None;
            var p = ValidProduct(id: 0, sku: 1000);

            _repo.Setup(r => r.GetProductBySKUAsync(p.SKU, ct))
                 .ReturnsAsync((Product)null!);

            _repo.Setup(r => r.GetProductByNameAsync(p.Name, ct))
                 .ReturnsAsync(ValidProduct(id: 88, sku: 2000));

            await Assert.ThrowsAsync<ConflictException>(() =>
                _sut.AddProductAsync(p, ct));

            _repo.VerifyAll();
            _repo.Verify(r => r.AddProductAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Never);
            _repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task AddProductAsync_WhenValid_AddsProduct()
        {
            var ct = CancellationToken.None;
            var p = ValidProduct(id: 0, sku: 1000);

            _repo.Setup(r => r.GetProductBySKUAsync(p.SKU, ct))
                 .ReturnsAsync((Product)null!);

            _repo.Setup(r => r.GetProductByNameAsync(p.Name, ct))
                 .ReturnsAsync((Product)null!);

            _repo.Setup(r => r.AddProductAsync(p, ct))
                 .Returns(Task.CompletedTask);

            await _sut.AddProductAsync(p, ct);

            _repo.VerifyAll();
            _repo.VerifyNoOtherCalls();
        }

        // -------------------------
        // UpdateProductAsync
        // -------------------------

        [Fact]
        public async Task UpdateProductAsync_WhenIdInvalid_ThrowsArgumentOutOfRangeException_AndDoesNotHitRepo()
        {
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                _sut.UpdateProductAsync(0, ValidProduct(id: 0, sku: 1), CancellationToken.None));

            _repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateProductAsync_WhenProductNull_ThrowsArgumentNullException_AndDoesNotHitRepo()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _sut.UpdateProductAsync(1, null!, CancellationToken.None));

            _repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateProductAsync_WhenProductInvalid_Throws_AndDoesNotHitRepo()
        {
            var invalid = new Product
            {
                Id = 0,
                Name = "",
                Price = 0m,
                SKU = 0,
                Currency = ""
            };

            await Assert.ThrowsAnyAsync<Exception>(() =>
                _sut.UpdateProductAsync(1, invalid, CancellationToken.None));

            _repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateProductAsync_WhenNotFound_ThrowsNotFoundException()
        {
            var ct = CancellationToken.None;
            var updated = ValidProduct(id: 999, sku: 111); // id will be overridden

            _repo.Setup(r => r.GetProductByIdAsync(5, ct))
                 .ReturnsAsync((Product)null!);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _sut.UpdateProductAsync(5, updated, ct));

            _repo.VerifyAll();
            _repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateProductAsync_WhenSkuConflicts_ThrowsConflictException_AndDoesNotUpdate()
        {
            var ct = CancellationToken.None;
            var updated = ValidProduct(id: 0, sku: 111);

            _repo.Setup(r => r.GetProductByIdAsync(5, ct))
                 .ReturnsAsync(ValidProduct(id: 5, sku: 500));

            _repo.Setup(r => r.GetProductBySKUAsync(updated.SKU, ct))
                 .ReturnsAsync(ValidProduct(id: 999, sku: updated.SKU)); // different id => conflict

            await Assert.ThrowsAsync<ConflictException>(() =>
                _sut.UpdateProductAsync(5, updated, ct));

            _repo.VerifyAll();
            _repo.Verify(r => r.UpdateProductAsync(It.IsAny<int>(), It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Never);
            _repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateProductAsync_WhenSkuIsSameProduct_DoesNotConflict_AndUpdates()
        {
            var ct = CancellationToken.None;
            var updated = ValidProduct(id: 0, sku: 111);

            _repo.Setup(r => r.GetProductByIdAsync(5, ct))
                 .ReturnsAsync(ValidProduct(id: 5, sku: 500));

            // SKU belongs to same product id => no conflict
            _repo.Setup(r => r.GetProductBySKUAsync(updated.SKU, ct))
                 .ReturnsAsync(ValidProduct(id: 5, sku: updated.SKU));

            _repo.Setup(r => r.GetProductByNameAsync(updated.Name, ct))
                 .ReturnsAsync((Product)null!);

            _repo.Setup(r => r.UpdateProductAsync(
                    5,
                    It.Is<Product>(p => p.Id == 5),
                    ct))
                .Returns(Task.CompletedTask);

            await _sut.UpdateProductAsync(5, updated, ct);

            _repo.VerifyAll();
            _repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateProductAsync_WhenNameConflicts_ThrowsConflictException_AndDoesNotUpdate()
        {
            var ct = CancellationToken.None;
            var updated = ValidProduct(id: 0, sku: 111);
            updated.Name = "New Name";

            _repo.Setup(r => r.GetProductByIdAsync(5, ct))
                 .ReturnsAsync(ValidProduct(id: 5, sku: 500));

            _repo.Setup(r => r.GetProductBySKUAsync(updated.SKU, ct))
                 .ReturnsAsync((Product)null!); // no sku conflict

            _repo.Setup(r => r.GetProductByNameAsync(updated.Name, ct))
                 .ReturnsAsync(ValidProduct(id: 999, sku: 777)); // different id => conflict

            await Assert.ThrowsAsync<ConflictException>(() =>
                _sut.UpdateProductAsync(5, updated, ct));

            _repo.VerifyAll();
            _repo.Verify(r => r.UpdateProductAsync(It.IsAny<int>(), It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Never);
            _repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateProductAsync_WhenNameIsSameProduct_DoesNotConflict_AndUpdates()
        {
            var ct = CancellationToken.None;
            var updated = ValidProduct(id: 0, sku: 111);

            _repo.Setup(r => r.GetProductByIdAsync(5, ct))
                 .ReturnsAsync(ValidProduct(id: 5, sku: 500));

            _repo.Setup(r => r.GetProductBySKUAsync(updated.SKU, ct))
                 .ReturnsAsync((Product)null!);

            // Name belongs to same product id => no conflict
            _repo.Setup(r => r.GetProductByNameAsync(updated.Name, ct))
                 .ReturnsAsync(ValidProduct(id: 5, sku: 999));

            _repo.Setup(r => r.UpdateProductAsync(
                    5,
                    It.Is<Product>(p => p.Id == 5),
                    ct))
                .Returns(Task.CompletedTask);

            await _sut.UpdateProductAsync(5, updated, ct);

            _repo.VerifyAll();
            _repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task UpdateProductAsync_WhenValid_Updates_AndForcesId()
        {
            var ct = CancellationToken.None;
            var updated = ValidProduct(id: 1234, sku: 111); // will be overridden to 5

            _repo.Setup(r => r.GetProductByIdAsync(5, ct))
                 .ReturnsAsync(ValidProduct(id: 5, sku: 500));

            _repo.Setup(r => r.GetProductBySKUAsync(updated.SKU, ct))
                 .ReturnsAsync((Product)null!);

            _repo.Setup(r => r.GetProductByNameAsync(updated.Name, ct))
                 .ReturnsAsync((Product)null!);

            _repo.Setup(r => r.UpdateProductAsync(
                    5,
                    It.Is<Product>(p => p.Id == 5), // confirms id forced to route id
                    ct))
                .Returns(Task.CompletedTask);

            await _sut.UpdateProductAsync(5, updated, ct);

            _repo.VerifyAll();
            _repo.VerifyNoOtherCalls();
        }

        // -------------------------
        // DeleteProductAsync
        // -------------------------

        [Fact]
        public async Task DeleteProductAsync_WhenIdInvalid_ThrowsArgumentOutOfRangeException_AndDoesNotHitRepo()
        {
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                _sut.DeleteProductAsync(0, CancellationToken.None));

            _repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task DeleteProductAsync_WhenNotFound_ThrowsNotFoundException()
        {
            var ct = CancellationToken.None;

            _repo.Setup(r => r.GetProductByIdAsync(9, ct))
                 .ReturnsAsync((Product)null!);

            await Assert.ThrowsAsync<NotFoundException>(() =>
                _sut.DeleteProductAsync(9, ct));

            _repo.VerifyAll();
            _repo.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task DeleteProductAsync_WhenFound_Deletes()
        {
            var ct = CancellationToken.None;

            _repo.Setup(r => r.GetProductByIdAsync(9, ct))
                 .ReturnsAsync(ValidProduct(id: 9, sku: 999));

            _repo.Setup(r => r.DeleteProductAsync(9, ct))
                 .Returns(Task.CompletedTask);

            await _sut.DeleteProductAsync(9, ct);

            _repo.VerifyAll();
            _repo.VerifyNoOtherCalls();
        }

        // -------------------------
        // Helpers
        // -------------------------

        private static Product ValidProduct(int id, int sku)
            => new Product
            {
                Id = id,
                Name = $"Product-{sku}",
                Price = 10.50m,
                SKU = sku,
                Currency = "USD"
            };
    }
}
