using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {

        private readonly IBasketService basketService;

        public BasketController(IBasketService basketService)
        {
            this.basketService = basketService;
        }

        [HttpGet]
        public async Task<ActionResult<BasketDto>> GetBasket()
        {
            var userId = GetUserId();
            var basket = await basketService.GetBasketAsync(userId);
            return Ok(Mapper.MapToDto(basket));
        }

        [HttpPost("items")]
        public async Task<ActionResult<BasketDto>> AddItem(AddItemRequest request)
        {
            var userId = GetUserId();
            var basketItem = new BasketItem
            {
                ProductId = request.ProductId,
                Amount = request.Amount
            };

            var basket = await basketService.AddItemAsync(userId, basketItem);
            return Ok(Mapper.MapToDto(basket));
        }

        [HttpPut("items/{productId}")]
        public async Task<ActionResult<BasketDto>> UpdateItem(Guid productId, UpdateItemRequest request)
        {
            var userId = GetUserId();
            var basket = await basketService.UpdateItemAsync(userId, productId, request.Amount);
            return Ok(Mapper.MapToDto(basket));
        }

        [HttpDelete("items/{productId}")]
        public async Task<IActionResult> RemoveItem(Guid productId)
        {
            var userId = GetUserId();
            var result = await basketService.RemoveItemAsync(userId, productId);
            return result ? NoContent() : NotFound();
        }

        [HttpDelete]
        public async Task<IActionResult> ClearBasket()
        {
            var userId = GetUserId();
            await basketService.ClearBasketAsync(userId);
            return NoContent();
        }




        /// <summary>
        /// заглушка для того чтобы вытащить корзину юзера
        /// в дальнейшем данные будут браться из отдельного сервиса аутентификации
        /// </summary>
        /// <returns></returns>
        private Guid GetUserId()
        {
            return Guid.NewGuid();
        }
    }
}
