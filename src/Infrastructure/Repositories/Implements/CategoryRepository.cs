using Microsoft.EntityFrameworkCore;
using TiendaUCN.src.Infrastructure.Data;
using TiendaUCN.src.Infrastructure.Repositories.Interfaces;

namespace TiendaUCN.src.Infrastructure.Repositories.Implements
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;
        public CategoryRepository(DataContext context)
        {
            _context = context;
        }
    }
}