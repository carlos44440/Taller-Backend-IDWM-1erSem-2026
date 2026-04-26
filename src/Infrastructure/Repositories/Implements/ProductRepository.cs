using Microsoft.EntityFrameworkCore;
using TiendaUCN.src.Application.DTOs.ProductDTO;
using TiendaUCN.src.Domain.Models;
using TiendaUCN.src.Infrastructure.Data;
using TiendaUCN.src.Infrastructure.Repositories.Interfaces;

namespace TiendaUCN.src.Infrastructure.Repositories.Implements
{
    /// <summary>
    /// Implementación del repositorio de productos.
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        /// <summary>
        /// Contexto de la base de datos.
        /// </summary>
        private readonly DataContext _context;

        /// <summary>
        /// Constructor del repositorio de productos.
        /// </summary>
        /// <param name="context">Contexto de la base de datos</param>
        public ProductRepository(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Verifica si un producto existe por su nombre y el nombre de su marca.
        /// </summary>
        /// <param name="name">El nombre del producto</param>
        /// <param name="brandName">El nombre de la marca</param>
        /// <returns>true si existe, false si no</returns>
        public async Task<bool> ExistsByNameAndBrandAsync(string name, string brandName)
        {
            // Verificar producto unico, antes de crear un producto. 
            // El producto no debe existir, por la combinacion de su nombre y marca, y ambos modelos no deben estar eliminados
            return await _context.Products
                .Include(p => p.Brand)
                .AnyAsync(p =>
                    p.Name.ToLower() == name.ToLower() &&
                    p.Brand.Name.ToLower() == brandName.ToLower() &&
                    p.IsDeleted == false &&
                    p.Brand.IsDeleted == false);
        }

        /// <summary>
        /// Crea un nuevo producto en la base de datos.
        /// </summary>
        /// <param name="product">El producto a crear</param>
        /// <returns>true si lo crea, false si no</returns>
        public async Task<bool> CreateAsync(Product product)
        {
            _context.Products.Add(product);
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Verifica si un producto existe por su ID.
        /// </summary>
        /// <param name="id">El ID del producto</param>
        /// <returns>true si existe, false si no</returns>
        public async Task<bool> ExistsByIdAsync(int id)
        {
            // Verificar producto existente.
            // Para un admin.
            // El producto debe existir y no estar eliminado
            return await _context.Products
                .AnyAsync(p => p.Id == id && p.IsDeleted == false);
        }

        /// <summary>
        /// Cambia el estado de un producto (activo/inactivo).
        /// </summary>
        /// <param name="id">El ID del producto</param>
        /// <returns>true si lo cambia, false si no</returns>
        public async Task<bool> SwitchStatusAsync(int id)
        {
            return await _context.Products
                .Where(p => p.Id == id && p.IsDeleted == false)
                .ExecuteUpdateAsync(p =>
                    p.SetProperty(p => p.IsActive, p => !p.IsActive)) > 0;
        }

        /// <summary>
        /// Obtiene el estado de un producto por su ID.
        /// </summary>
        /// <param name="id">El ID del producto</param>
        /// <returns>El estado del producto o null si no se encuentra</returns>
        public async Task<string?> GetStatusAsync(int id)
        {
            return await _context.Products
                .Where(p => p.Id == id && p.IsDeleted == false)
                .Select(p => p.IsActive ? "Activo" : "Inactivo")
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Verifica si un producto existe por su ID y si está activo (para clientes).
        /// </summary>
        /// <param name="id">El ID del producto</param>
        /// <returns>true si existe y está activo, false si no</returns>
        public async Task<bool> ExistsByIdCustomerAsync(int id)
        {
            // Verificar producto existente.
            // Para un cliente.
            // El producto debe existir, esté activo y no estar eliminado
            return await _context.Products
                .AnyAsync(p =>
                    p.Id == id &&
                    p.IsActive == true &&
                    p.IsDeleted == false);
        }

        /// <summary>
        /// Obtiene un producto por su ID para clientes (solo si está activo).
        /// </summary>
        /// <param name="id">El ID del producto</param>
        /// <returns>El producto o null si no se encuentra o no está activo</returns>
        public async Task<Product?> GetProductByIdForCustomerAsync(int id)
        {
            // Obtener producto por id.
            // Para un cliente.
            // El producto debe existir, estar activo y no estar eliminado
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p =>
                    p.Id == id &&
                    p.IsDeleted == false &&
                    p.IsActive == true);
        }

        /// <summary>
        /// Obtiene un producto por su ID para administradores (sin importar su estado).
        /// </summary>
        /// <param name="id">El ID del producto</param>
        /// <returns>El producto o null si no se encuentra</returns>
        public async Task<Product?> GetProductByIdForAdminAsync(int id)
        {
            // Obtener producto por id.
            // Para un admin.
            // El producto debe existir y no estar eliminado
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p =>
                    p.Id == id &&
                    p.IsDeleted == false);
        }

        /// <summary>
        /// Elimina un producto por su ID.
        /// </summary>
        /// <param name="id">El ID del producto</param>
        /// <returns>true si lo elimina, false si no</returns>
        public async Task<bool> DeleteAsync(int id)
        {
            // Eliminar producto por id.
            // El producto debe existir y no estar eliminado
            return await _context.Products
                .Where(p => p.Id == id && p.IsDeleted == false)
                .ExecuteUpdateAsync(p =>
                    p.SetProperty(p => p.IsDeleted, true)) > 0;
        }

        /// <summary>
        /// Obtiene una lista de productos filtrados y paginados para administradores.
        /// </summary>
        /// <param name="searchParams">Los parámetros de búsqueda</param>
        /// <returns>Una tupla con la lista de productos y el total de registros</returns>
        public async Task<(IEnumerable<Product> products, int totalCount)> GetFilteredForAdminAsync(SearchParamsDTO searchParams)
        {
            // Obtener queryable de productos que no estén eliminados
            var query = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Images.Take(1))
                .Where(p => p.IsDeleted == false)
                .AsNoTracking(); // Para mejorar el rendimiento en consultas de solo lectura

            // Aplicar filtro de búsqueda si se proporciona un término de búsqueda
            if (!string.IsNullOrWhiteSpace(searchParams.SearchTerm))
            {
                var searchTerm = searchParams.SearchTerm.Trim().ToLower();
                query = query.Where(p =>
                    p.Name.ToLower().Contains(searchTerm) ||
                    p.Description.ToLower().Contains(searchTerm) ||
                    p.Price.ToString().Contains(searchTerm) ||
                    p.Stock.ToString().Contains(searchTerm) ||
                    p.Category.Name.ToLower().Contains(searchTerm) ||
                    (p.Category.Description != null && p.Category.Description.ToLower().Contains(searchTerm)) ||
                    p.Brand.Name.ToLower().Contains(searchTerm) ||
                    (p.Brand.Description != null && p.Brand.Description.ToLower().Contains(searchTerm)));
            }

            // Obtener el total de productos que cumplen con el filtro
            int totalCount = await query.CountAsync();

            // Aplicar paginación
            var products = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((searchParams.PageNumber - 1) * searchParams.PageSize)
                .Take(searchParams.PageSize)
                .ToArrayAsync();

            // Ej: Si el tamaño de página es 10, y el número de página es 2
            // Se omitirán los primeros 10 productos y se tomarán los siguientes 10 productos para mostrar en la página 2.

            return (products, totalCount);
        }

        /// <summary>
        /// Obtiene una lista de productos filtrados y paginados para clientes (solo activos).
        /// </summary>
        /// <param name="searchParams">Los parámetros de búsqueda</param>
        /// <returns>Una tupla con la lista de productos y el total de registros</returns>
        public async Task<(IEnumerable<Product> products, int totalCount)> GetFilteredForCustomerAsync(SearchParamsDTO searchParams)
        {
            // Obtener queryable de productos que estén activos y no eliminados
            var query = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Images.Take(1))
                .Where(p => p.IsDeleted == false && p.IsActive == true)
                .AsNoTracking(); // Para mejorar el rendimiento en consultas de solo lectura

            // Aplicar filtro de búsqueda si se proporciona un término de búsqueda
            if (!string.IsNullOrWhiteSpace(searchParams.SearchTerm))
            {
                var searchTerm = searchParams.SearchTerm.Trim().ToLower();
                query = query.Where(p =>
                    p.Name.ToLower().Contains(searchTerm) ||
                    p.Description.ToLower().Contains(searchTerm) ||
                    p.Price.ToString().Contains(searchTerm) ||
                    p.Stock.ToString().Contains(searchTerm) ||
                    p.Category.Name.ToLower().Contains(searchTerm) ||
                    (p.Category.Description != null && p.Category.Description.ToLower().Contains(searchTerm)) ||
                    p.Brand.Name.ToLower().Contains(searchTerm) ||
                    (p.Brand.Description != null && p.Brand.Description.ToLower().Contains(searchTerm)));
            }

            // Obtener el total de productos que cumplen con el filtro
            int totalCount = await query.CountAsync();

            // Aplicar paginación
            var products = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((searchParams.PageNumber - 1) * searchParams.PageSize)
                .Take(searchParams.PageSize)
                .ToArrayAsync();

            return (products, totalCount);
        }

        /// <summary>
        /// Actualiza un producto existente.
        /// </summary>
        /// <param name="product">El producto a actualizar</param>
        /// <returns>true si lo actualiza, false si no</returns>
        public async Task<bool> UpdateAsync(Product product)
        {
            return await _context.Products
                .Where(p => p.Id == product.Id && p.IsDeleted == false)
                .ExecuteUpdateAsync(p => p
                    .SetProperty(p => p.Name, product.Name)
                    .SetProperty(p => p.Description, product.Description)
                    .SetProperty(p => p.Price, product.Price)
                    .SetProperty(p => p.Stock, product.Stock)
                    .SetProperty(p => p.CategoryId, product.CategoryId)
                    .SetProperty(p => p.BrandId, product.BrandId)) > 0;
        }

        /// <summary>
        /// Actualiza el stock de un producto por su ID.
        /// </summary>
        /// <param name="productId">El ID del producto</param>
        /// <param name="newStock">El nuevo stock</param>
        /// <returns>true si lo actualiza, false si no</returns>
        public async Task<bool> UpdateStockAsync(int productId, int newStock)
        {
            return await _context.Products
                .Where(p => p.Id == productId && p.IsDeleted == false)
                .ExecuteUpdateAsync(p =>
                    p.SetProperty(p => p.Stock, newStock)) > 0;
        }
    }
}