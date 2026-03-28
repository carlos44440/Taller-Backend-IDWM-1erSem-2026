using TiendaUCN.src.Application.Services.Interfaces;
using TiendaUCN.src.Infrastructure.Repositories.Interfaces;

namespace TiendaUCN.src.Application.Services.Implements
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IImageService _imageService;
        private readonly IBrandRepository _brandRepository;
        public ProductService(IProductRepository productRepository, IImageService imageService, IBrandRepository brandRepository)
        {
            _productRepository = productRepository;
            _imageService = imageService;
            _brandRepository = brandRepository;
        }


    }
}