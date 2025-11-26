using Application.DTOs;
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
    }
}
