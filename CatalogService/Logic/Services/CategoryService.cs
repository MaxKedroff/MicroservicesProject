using CatalogService.CoreLib.DTOs;
using CatalogService.CoreLib.DTOs.Requests;
using CatalogService.CoreLib.Entities;
using CatalogService.CoreLib.Interfaces;
using CatalogService.CoreLib.Utils;
using CatalogService.Logic.Interfaces;

namespace CatalogService.Logic.Services
{
    public class CategoryService : ICategoryService
    {

        private readonly ICategoryRepository _categoryRepository;
        
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequest request)
        {
            var category = new Category { 
                Name = request.Name,
                Description = request.Description,
                ParentCategoryId = request.ParentCategoryId,
                IsActive = true,
                ProductCount = 0
            };
            var createdCategory = await _categoryRepository.AddCategoryAsync(category);
            return Mapper.MapToCategoryDto(createdCategory);
        }

        public async Task<bool> DeleteCategoryAsync(Guid id)
        {
            return await _categoryRepository.DeleteCategoryAsync(id);
        }

        public async Task<List<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            return categories.Select(Mapper.MapToCategoryDto).ToList();
        }

        public async Task<CategoryDto> GetCategoryAsync(Guid id)
        {
            var category = await _categoryRepository.GetCategoryByIdASync(id);
            return category == null ? null : Mapper.MapToCategoryDto(category);
        }

        public async Task<CategoryDto> UpdateCategoryAsync(Guid id, CreateCategoryRequest request)
        {
            var category = await _categoryRepository.GetCategoryByIdASync(id);
            if (category == null) return null;

            category.Name = request.Name;
            category.Description = request.Description;
            category.ParentCategoryId = request.ParentCategoryId;

            var updated = await _categoryRepository.UpdateCategoryAsync(category);
            return Mapper.MapToCategoryDto(updated);
        }
    }
}
