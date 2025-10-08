using CatalogService.CoreLib.DTOs.Requests;
using CatalogService.CoreLib.DTOs;

namespace CatalogService.Logic.Interfaces
{
    public interface IProductService
    {
        Task<ProductDetailsDto> GetProductAsync(Guid id);
        Task<List<ProductDto>> GetProductsAsync(ProductFilter filter);
        Task<ProductDetailsDto> CreateProductAsync(CreateProductRequest request);
        Task<ProductDetailsDto> UpdateProductAsync(Guid id, UpdateProductRequest request);
        Task<bool> DeleteProductAsync(Guid id);
    }
}
