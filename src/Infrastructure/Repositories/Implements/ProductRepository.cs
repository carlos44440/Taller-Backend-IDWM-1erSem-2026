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
            // Verificar producto unico, antes de crear un producto. 
            // El producto no debe existir, por la combinacion de su nombre y marca, y ambos modelos no deben estar eliminados
            return await _context.Products.Include(p => p.Brand).AnyAsync(p => p.Name.ToLower() == name.ToLower()
                && p.Brand.Name.ToLower() == brandName.ToLower() && p.IsDeleted == false && p.Brand.IsDeleted == false);
        }

        public async Task<bool> CreateAsync(Product product)
        {
            _context.Products.Add(product);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsByIdAsync(int id)
        {
            // Verificar producto existente.
            // Para un admin.
            // El producto debe existir y no estar eliminado
            return await _context.Products.AnyAsync(p => p.Id == id && p.IsDeleted == false);
        }

        public async Task<bool> SwitchStatusAsync(int id)
        {
            return await _context.Products.Where(p => p.Id == id && p.IsDeleted == false)
                .ExecuteUpdateAsync(p => p.SetProperty(p => p.IsActive, p => !p.IsActive)) > 0;
        }

        public async Task<bool> ExistsByIdCustomerAsync(int id)
        {
            // Verificar producto existente.
            // Para un cliente.
            // El producto debe existir, esté activo y no estar eliminado
            return await _context.Products.AnyAsync(p => p.Id == id && p.IsActive == true && p.IsDeleted == false);
        }

        public async Task<Product?> GetProductByIdForCustomerAsync(int id)
        {
            // Obtener producto por id.
            // Para un cliente.
            // El producto debe existir, estar activo y no estar eliminado
            return await _context.Products.Include(p => p.Brand).Include(p => p.Category).Include(p => p.Images)
                .FirstOrDefaultAsync(
                    p => p.Id == id && p.IsDeleted == false && p.IsActive == true);
        }

        public async Task<Product?> GetProductByIdForAdminAsync(int id)
        {
            // Obtener producto por id.
            // Para un admin.
            // El producto debe existir y no estar eliminado
            return await _context.Products.Include(p => p.Brand).Include(p => p.Category).Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsDeleted == false);
        }
    }
}