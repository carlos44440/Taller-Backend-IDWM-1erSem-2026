using Microsoft.EntityFrameworkCore;
using TiendaUCN.src.Domain.Models;
using TiendaUCN.src.Infrastructure.Data;
using TiendaUCN.src.Infrastructure.Repositories.Interfaces;

namespace TiendaUCN.src.Infrastructure.Repositories.Implements
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DataContext _context;
        public OrderRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsByCodeAsync(string code)
        {
            return await _context.Orders.AnyAsync(o => o.Code == code);
        }

        public async Task<bool> CreateAsync(Order order)
        {
            _context.Orders.Add(order);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}