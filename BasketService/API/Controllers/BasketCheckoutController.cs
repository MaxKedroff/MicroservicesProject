using Application.DTOs;
using Domain.Contracts;
using Infrastructure.Sagas;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketCheckoutController : ControllerBase
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IRequestClient<IGetBasketCheckoutStatus> _statusClient;

        public BasketCheckoutController(IPublishEndpoint publishEndpoint, IRequestClient<IGetBasketCheckoutStatus> statusClient)
        {
            _publishEndpoint = publishEndpoint;
            _statusClient = statusClient;
        }

        public async Task<IActionResult> StartCheckoutOrchestration([FromBody] StartCheckoutRequest request)
        {
            var basketId = Guid.NewGuid();

            await _publishEndpoint.Publish<IGetBasketCheckoutStatus>(new
            {
                basketId,
                request.UserId,
                request.Items,
                DateTime.UtcNow
            });

            return Accepted(new
            {
                BasketId = basketId,
                Status = "CheckoutStarted",
                SagaType = "Orchestration",
                Message = "Basket checkout process started using orchestration pattern"
            });
        }

        [HttpPost("coordination")]
        public async Task<IActionResult> StartCheckoutCoordination([FromBody] StartCheckoutRequest request)
        {
            var basketId = Guid.NewGuid();

            await _publishEndpoint.Publish<IReserveInventoryArguments>(new
            {
                basketId,
                request.Items
            });

            return Accepted(new
            {
                BasketId = basketId,
                Status = "InventoryReservationStarted",
                SagaType = "Coordination",
                Message = "Inventory reservation started using coordination pattern"
            });
        }

        [HttpGet("{basketId:guid}/status")]
        public async Task<IActionResult> GetCheckoutStatus(Guid basketId)
        {
            try
            {
                var response = await _statusClient.GetResponse<IBasketCheckoutStatusResponse>(new
                {
                    BasketId = basketId
                });

                return Ok(response.Message);
            }
            catch (RequestTimeoutException)
            {
                return StatusCode(408, new { Message = "Request timeout" });
            }
        }
    }
}
