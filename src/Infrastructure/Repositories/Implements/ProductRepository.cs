using Microsoft.EntityFrameworkCore;
using TiendaUCN.src.Domain.Models;
using TiendaUCN.src.Infrastructure.Data;
using TiendaUCN.src.Infrastructure.Repositories.Interfaces;

namespace TiendaUCN.src.Infrastructure.Repositories.Implements
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;
        public ProductRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> ExistsByNameAndBrandAsync(string name, string brandName)
        {
            return await _context.Products.Include(p => p.Brand).AnyAsync(p => p.Name.ToLower() == name.ToLower()
                && p.Brand.Name.ToLower() == brandName.ToLower() && p.IsDeleted == false && p.Brand.IsDeleted == false);
        }

        public async Task<bool> CreateAsync(Product product)
        {
            _context.Products.Add(product);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}