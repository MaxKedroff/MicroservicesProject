using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class BasketLogsRepository : IBasketLogRepository
    {

        private readonly BasketLogDbContext _context;

        public BasketLogsRepository(BasketLogDbContext context)
        {
            _context = context;
        }

        public async Task AddLogAsync(BasketOperationLog log)
        {
            _context.BasketOperationsLogs.Add(log);
            await _context.SaveChangesAsync();
        }

        public async Task<List<BasketOperationLog>> GetUserLogsAsync(Guid userId, int count = 50)
        {
            return await _context.BasketOperationsLogs
                .Where(log => log.UserId == userId)
                .OrderByDescending(log => log.Timestamp)
                .Take(count)
                .ToListAsync();
        }
    }
}
