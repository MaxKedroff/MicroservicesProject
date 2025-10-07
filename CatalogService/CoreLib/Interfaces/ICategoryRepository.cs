using CatalogService.CoreLib.Entities;

namespace CatalogService.CoreLib.Interfaces
{
    public interface ICategoryRepository
    {
        /// <summary>
        /// получение информации о конкретной категории
        /// </summary>
        /// <param name="id">id категории</param>
        /// <returns>возвращает найденную категорию</returns>


        Task<Category> GetCategoryByIdASync(Guid id);
        /// <summary>
        /// получение информации о всех категориях
        /// </summary>
        /// <returns>список категорий</returns>
        Task<List<Category>> GetAllCategoriesAsync();


        /// <summary>
        /// создание новой категории(здесь потом необходимо настроить авторизацию, нужны права администратора)
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        Task<Category> AddCategoryAsync(Category category);


        /// <summary>
        /// обновление существующей категории (нужны права администратора)
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        Task<Category> UpdateCategoryAsync(Category category);


        /// <summary>
        /// удаление категории (нужны права администратора)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteCategoryAsync(Guid id);
    }
}
