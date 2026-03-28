using TiendaUCN.src.Infrastructure.Data;
using TiendaUCN.src.Infrastructure.Repositories.Interfaces;

namespace TiendaUCN.src.Infrastructure.Repositories.Implements
{
    public class BrandRepository : IBrandRepository
    {
        private readonly DataContext _context;
        public BrandRepository(DataContext context)
        {
            _context = context;
        }
    }
}