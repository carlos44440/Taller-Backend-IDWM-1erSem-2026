using Mapster;
using Serilog;
using TiendaUCN.src.Application.DTOs.ProductDTO;
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
        public ProductService(IProductRepository productRepository, IImageService imageService, IBrandRepository brandRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _imageService = imageService;
            _brandRepository = brandRepository;
            _categoryRepository = categoryRepository;
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
            foreach (var image in createProductDTO.Images)
            {
                Log.Information("Imagen asociada al producto: {@Image}", image);
                await _imageService.UploadAsync(image, product.Id);
            }

            return product.Id.ToString();
        }
    }
}