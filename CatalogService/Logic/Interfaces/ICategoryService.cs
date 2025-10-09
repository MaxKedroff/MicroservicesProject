using CatalogService.CoreLib.DTOs;
using CatalogService.CoreLib.DTOs.Requests;

namespace CatalogService.Logic.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryDto> GetCategoryAsync(Guid id);
        Task<List<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequest request);
        Task<CategoryDto> UpdateCategoryAsync(Guid id, CreateCategoryRequest request);
        Task<bool> DeleteCategoryAsync(Guid id);
    }
}
