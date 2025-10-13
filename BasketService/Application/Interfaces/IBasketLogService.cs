using Application.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IBasketLogService
    {
        Task LogOperationAsync(Guid userId, string operation, Guid? productId = null, int? amount = null, string details = null);

        Task<List<BasketOperationLog>> GetUserOperationsHistoryAsync(Guid userId);
    }
}
