using Mapster;
using Serilog;
using System.Diagnostics;
using TiendaUCN.src.Application.DTOs.ProductDTO;
using TiendaUCN.src.Application.DTOs.ProductDTO.Admin;
using TiendaUCN.src.Application.DTOs.ProductDTO.Customer;
using TiendaUCN.src.Application.Services.Interfaces;
using TiendaUCN.src.Domain.Models;
using TiendaUCN.src.Infrastructure.Repositories.Interfaces;

namespace TiendaUCN.src.Application.Services.Implements
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IImageService _imageService;
        private readonly IBrandRepository _brandRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IConfiguration _configuration;
        public ProductService(IProductRepository productRepository, IImageService imageService, IBrandRepository brandRepository, ICategoryRepository categoryRepository, IConfiguration configuration)
        {
            _productRepository = productRepository;
            _imageService = imageService;
            _brandRepository = brandRepository;
            _categoryRepository = categoryRepository;
            _configuration = configuration;
        }

        public async Task<string> CreateProductAsync(CreateProductDTO createProductDTO)
        {
            // Verificar si la categoría existe
            var categoryExists = await _categoryRepository.ExistsByNameAsync(createProductDTO.CategoryName);
            if (!categoryExists)
            {
                Log.Error("La categoría no existe: {CategoryName}", createProductDTO.CategoryName);
                throw new Exception("La categoría no existe: " + createProductDTO.CategoryName);
            }

            // Verificar si la marca existe
            var brandExists = await _brandRepository.ExistsByNameAsync(createProductDTO.BrandName);
            if (!brandExists)
            {
                Log.Error("La marca no existe: {BrandName}", createProductDTO.BrandName);
                throw new Exception("La marca no existe: " + createProductDTO.BrandName);
            }

            // Verificar si el producto ya existe, por la combinacion de su nombre y marca
            var productExists = await _productRepository.ExistsByNameAndBrandAsync(createProductDTO.Name, createProductDTO.BrandName);
            if (productExists)
            {
                Log.Error("Ya existe un producto con el mismo nombre y marca: {Name} - {Brand}", createProductDTO.Name, createProductDTO.BrandName);
                throw new Exception("Ya existe un producto con el mismo nombre y marca: " + createProductDTO.Name + " - " + createProductDTO.BrandName);
            }

            // Crear el producto
            var product = createProductDTO.Adapt<Product>();
            product.CategoryId = await _categoryRepository.GetIdByNameAsync(createProductDTO.CategoryName);
            product.BrandId = await _brandRepository.GetIdByNameAsync(createProductDTO.BrandName);

            // Guardar el producto en la base de datos
            var isCreated = await _productRepository.CreateAsync(product);
            if (!isCreated)
            {
                Log.Error("Error al crear el producto: {Name}", createProductDTO.Name);
                throw new Exception("Error al crear el producto: " + createProductDTO.Name);
            }

            // Subir las imágenes asociadas al producto
            foreach (var image in createProductDTO.ImagesFiles)
            {
                Log.Information("Imagen asociada al producto: {@Image}", image);
                await _imageService.UploadAsync(image, product.Id);
            }

            return product.Id.ToString();
        }

        public async Task<string> SwitchStatusProductAsync(int id)
        {
            var productExists = await _productRepository.ExistsByIdAsync(id);
            if (!productExists)
            {
                Log.Error("Producto no encontrado con ID: {ProductId}", id);
                throw new Exception("Producto no encontrado con ID: " + id);
            }

            var isUpdated = await _productRepository.SwitchStatusAsync(id);
            if (!isUpdated)
            {
                Log.Error("Error al cambiar el estado del producto con ID: {ProductId}", id);
                throw new Exception("Error al cambiar el estado del producto con ID: " + id);
            }

            var statusUpdated = await _productRepository.GetStatusAsync(id);
            return "Se cambio el estado a: " + statusUpdated;
        }

        public async Task<ProductDetailCustomerDTO> GetProductByIdForCustomerAsync(int id)
        {
            // Verificar si el producto existe
            var productExists = await _productRepository.ExistsByIdCustomerAsync(id);
            if (!productExists)
            {
                Log.Error("Producto no encontrado con ID: {ProductId}", id);
                throw new KeyNotFoundException("Producto no encontrado con ID: " + id);
            }

            // Obtener el producto de la base de datos
            var product = await _productRepository.GetProductByIdForCustomerAsync(id)
                ?? throw new KeyNotFoundException("Producto no encontrado con ID: " + id);

            // Si la categoría del producto está eliminada, asignar un nombre y descripción genéricos
            if (product.Category.IsDeleted)
            {
                product.Category.Name = "Categoría no disponible";
                product.Category.Description = "Categoría no disponible";
            }

            // Si la marca del producto está eliminada, asignar un nombre y descripción genéricos
            if (product.Brand.IsDeleted)
            {
                product.Brand.Name = "Marca no disponible";
                product.Brand.Description = "Marca no disponible";
            }

            // Mapear el producto a un DTO para el cliente
            var productDetailCustomerDTO = product.Adapt<ProductDetailCustomerDTO>();
            return productDetailCustomerDTO!;
        }

        public async Task<ProductDetailAdminDTO> GetProductByIdForAdminAsync(int id)
        {
            // Verificar si el producto existe
            var productExists = await _productRepository.ExistsByIdAsync(id);
            if (!productExists)
            {
                Log.Error("Producto no encontrado con ID: {ProductId}", id);
                throw new KeyNotFoundException("Producto no encontrado con ID: " + id);
            }

            // Obtener el producto de la base de datos
            Product product = await _productRepository.GetProductByIdForAdminAsync(id)
                ?? throw new KeyNotFoundException("Producto no encontrado con ID: " + id);

            // Si la categoría del producto está eliminada, asignar un nombre y descripción genéricos
            if (product.Category.IsDeleted)
            {
                product.Category.Name = "Categoría eliminada";
                product.Category.Description = "Categoría eliminada";
            }

            // Si la marca del producto está eliminada, asignar un nombre y descripción genéricos
            if (product.Brand.IsDeleted)
            {
                product.Brand.Name = "Marca eliminada";
                product.Brand.Description = "Marca eliminada";
            }

            // Mapear el producto a un DTO para el admin
            var productDetailAdminDTO = product.Adapt<ProductDetailAdminDTO>();
            return productDetailAdminDTO!;
        }

        public async Task DeleteProductAsync(int id)
        {
            var productExists = await _productRepository.ExistsByIdAsync(id);
            if (!productExists)
            {
                Log.Error("Producto no encontrado con ID: {ProductId}", id);
                throw new KeyNotFoundException("Producto no encontrado con ID: " + id);
            }

            var isDeleted = await _productRepository.DeleteAsync(id);
            if (!isDeleted)
            {
                Log.Error("Error al eliminar el producto con ID: {ProductId}", id);
                throw new Exception("Error al eliminar el producto con ID: " + id);
            }
        }

        public async Task<ListedProductsForAdminDTO> GetListedProductsForAdminAsync(SearchParamsDTO searchParams)
        {
            // Obtener los productos filtrados y el total de productos que cumplen con el filtro
            var (products, totalCount) = await _productRepository.GetFilteredForAdminAsync(searchParams);

            if (totalCount == 0)
            {
                Log.Information("No se encontraron productos que cumplan con los criterios de búsqueda para el admin. Filtros: {@SearchParams}", searchParams);
                throw new KeyNotFoundException("No se encontraron productos que cumplan con los criterios de búsqueda para el admin.");
            }

            var totalPages = (int)Math.Ceiling((double)totalCount / searchParams.PageSize);
            var productsInPage = products.Count();

            if (searchParams.PageNumber > totalPages)
            {
                Log.Information("No se encontraron productos en la página solicitada para el admin. Filtros: {@SearchParams}", searchParams);
                throw new KeyNotFoundException($"La página {searchParams.PageNumber} no existe. Total: {totalPages}.");
            }

            // Convertimos los productos filtrados a DTOs para la respuesta
            return new ListedProductsForAdminDTO
            {
                Products = products.Adapt<List<ProductForAdminDTO>>(),
                TotalCount = totalCount,
                TotalPages = totalPages,
                CurrentPage = searchParams.PageNumber,
                PageSize = searchParams.PageSize,
                ProductsInPage = productsInPage
            };
        }

        public async Task<ListedProductsForCustomerDTO> GetListedProductsForCustomerAsync(SearchParamsDTO searchParams)
        {
            // Obtener los productos filtrados y el total de productos que cumplen con el filtro
            var (products, totalCount) = await _productRepository.GetFilteredForCustomerAsync(searchParams);

            if (totalCount == 0)
            {
                Log.Information("No se encontraron productos que cumplan con los criterios de búsqueda para el customer. Filtros: {@SearchParams}", searchParams);
                throw new KeyNotFoundException("No se encontraron productos que cumplan con los criterios de búsqueda.");
            }

            var totalPages = (int)Math.Ceiling((double)totalCount / searchParams.PageSize);
            var productsInPage = products.Count();

            if (searchParams.PageNumber > totalPages)
            {
                Log.Information("No se encontraron productos en la página solicitada para el customer. Filtros: {@SearchParams}", searchParams);
                throw new KeyNotFoundException($"La página {searchParams.PageNumber} no existe. Total: {totalPages}.");
            }

            // Convertimos los productos filtrados a DTOs para la respuesta
            return new ListedProductsForCustomerDTO
            {
                Products = products.Adapt<List<ProductForCustomerDTO>>(),
                TotalCount = totalCount,
                TotalPages = totalPages,
                CurrentPage = searchParams.PageNumber,
                PageSize = searchParams.PageSize,
                ProductsInPage = productsInPage
            };
        }
    }
}