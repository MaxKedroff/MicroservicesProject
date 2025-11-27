using CatalogService.CoreLib.DTOs;
using CatalogService.CoreLib.DTOs.Requests;
using CatalogService.Logic.Interfaces;
using CustomDistributedSemaphore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IDistributedSemaphoreFactory _semaphoreFactory;


        public CategoriesController(ICategoryService categoryService, IDistributedSemaphoreFactory semaphoreFactory)
        {
            _categoryService = categoryService;
            _semaphoreFactory = semaphoreFactory;
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryDto>>> GetCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(Guid id)
        {
            var category = await _categoryService.GetCategoryAsync(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDto>> CreateCategory(
            CreateCategoryRequest request)
        {
            var semaphore = _semaphoreFactory.CreateSemaphore("category_operations", 3);

            try
            {
                await semaphore.AcquireAsync();

                var category = await _categoryService.CreateCategoryAsync(request);
                return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
            }
            catch (TimeoutException)
            {
                return StatusCode(429, new { Message = "Too many concurrent category operations" });
            }
            finally
            {
                await semaphore.ReleaseAsync();
            }

        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDto>> UpdateCategory(
            Guid id, CreateCategoryRequest request)
        {
            var semaphore = _semaphoreFactory.CreateSemaphore($"category_update_{id}", 1);
            try
            {
                await semaphore.AcquireAsync(TimeSpan.FromSeconds(5));

                var category = await _categoryService.UpdateCategoryAsync(id, request);
                if (category == null) return NotFound();
                return Ok(category);
            }
            catch (TimeoutException)
            {
                return StatusCode(409, new
                {
                    Message = "Category is currently being updated by another process"
                });
            }
            finally
            {
                await semaphore.ReleaseAsync();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var semaphore = _semaphoreFactory.CreateSemaphore($"category_delete_{id}", 1);
            try
            {
                await semaphore.AcquireAsync();

                var result = await _categoryService.DeleteCategoryAsync(id);
                if (!result) return NotFound();
                return NoContent();
            }
            finally
            {
                await semaphore.ReleaseAsync();
            }
            
        }
    }
}
