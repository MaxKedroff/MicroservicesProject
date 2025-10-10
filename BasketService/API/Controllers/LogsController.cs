using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {

        private readonly IBasketLogService _service;

        public LogsController(IBasketLogService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<OperationLogDto>>> GetUserLogs()
        {
            var userId = GetUserId();
            var logs = await _service.GetUserOperationsHistoryAsync(userId);
            return Ok(logs.Select(Mapper.MapToOperationLogDto).ToList());
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
