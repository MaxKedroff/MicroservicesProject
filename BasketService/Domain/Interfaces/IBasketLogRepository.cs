using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IBasketLogRepository
    {
        Task AddLogAsync(BasketOperationLog log);
        Task<List<BasketOperationLog>> GetUserLogsAsync(Guid userId, int count = 50);
    }
}
