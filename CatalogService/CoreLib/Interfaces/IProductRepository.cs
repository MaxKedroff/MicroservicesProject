using CatalogService.CoreLib.DTOs;
using CatalogService.CoreLib.Entities;

namespace CatalogService.CoreLib.Interfaces
{
    public interface IProductRepository
    {
        /// <summary>
        /// получить конкретный товар по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Product> GetProductByIdAsync(Guid id);


        /// <summary>
        /// получить список товаров по заданной фильтрации
        /// </summary>
        /// <param name="filter">DTO с фильтрами</param>
        /// <returns></returns>
        Task<List<Product>> GetProductsAsync(ProductFilter filter);


        /// <summary>
        /// создание нового товара (требуется права администратора и связка с остатками)
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Task<Product> AddProductAsync(Product product);

        
        /// <summary>
        /// обновление товара (требуется права администратора)
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Task<Product> UpdateProductAsync(Product product);

        /// <summary>
        /// удаление товара (требуется права администратора и связка с остатками)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteProductAsync(Guid id);
    }
}
