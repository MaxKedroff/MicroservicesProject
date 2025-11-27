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
    public class ProductsController : ControllerBase
    {
        public readonly IProductService _productService;
        private readonly IDistributedSemaphoreFactory _semaphoreFactory;


        public ProductsController(IProductService productService, IDistributedSemaphoreFactory semaphoreFactory)

        {
            _productService = productService;
            _semaphoreFactory = semaphoreFactory;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> GetProducts([FromQuery] ProductFilter filter)
        {
            var products = await _productService.GetProductsAsync(filter);
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDetailsDto>> GetProduct(Guid id)
        {
            var product = await _productService.GetProductAsync(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ProductDetailsDto>> CreateProduct(CreateProductRequest request)
        {
            var semaphore = _semaphoreFactory.CreateSemaphore("product_creation", 2);
            try
            {
                await semaphore.AcquireAsync();
                var product = await _productService.CreateProductAsync(request);
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }catch (TimeoutException)
            {
                return StatusCode(429, new { Message = "Too many concurrent product creations" });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDetailsDto>> UpdateProduct(Guid id, UpdateProductRequest request)
        {
            var semaphore = _semaphoreFactory.CreateSemaphore($"product_update_{id}", 1);

            try
            {
                await semaphore.AcquireAsync(TimeSpan.FromSeconds(5));
                var product = await _productService.UpdateProductAsync(id, request);
                if (product == null) return NotFound();
                return Ok(product);
            }catch (TimeoutException)
            {
                return StatusCode(409, new
                {
                    Message = "Product is currently being updated by another process. Please try again later."
                });
            }
            finally
            {
                await semaphore.ReleaseAsync();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var semaphore = _semaphoreFactory.CreateSemaphore($"product_delete_{id}", 1);
            try
            {
                await semaphore.AcquireAsync();

                var result = await _productService.DeleteProductAsync(id);
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
