using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class BasketLogService : IBasketLogService
    {

        private readonly IBasketLogRepository _logRepository;

        public BasketLogService(IBasketLogRepository logRepository)
        {
            _logRepository = logRepository;
        }


        public async Task<List<BasketOperationLog>> GetUserOperationsHistoryAsync(Guid userId)
        {
            return await _logRepository.GetUserLogsAsync(userId);
        }

        public async Task LogOperationAsync(Guid userId, string operation, Guid? productId = null, int? amount = null, string details = null)
        {
            var log = new BasketOperationLog
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Operation = operation,
                Amount = amount,
                ProductId = productId,
                Timestamp = DateTime.UtcNow,
                details = details
            };

            await _logRepository.AddLogAsync(log);
        }
    }
}
