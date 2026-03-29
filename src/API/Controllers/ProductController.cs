using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tienda_UCN_api.src.Application.DTO;
using TiendaUCN.src.Application.DTOs.ProductDTO;
using TiendaUCN.src.Application.Services.Interfaces;

namespace TiendaUCN.src.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDTO createProductDTO)
        {
            var result = await _productService.CreateProductAsync(createProductDTO);
            return Created($"/api/product/{result}", new GenericResponse<string>("Producto creado exitosamente", result));
        }

        [HttpPatch("/switch-status/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SwitchStatusProductAsync([FromRoute] int id)
        {
            await _productService.SwitchStatusProductAsync(id);
            return Ok(new GenericResponse<string>("Estado del producto cambiado exitosamente", null));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductByIdForCustomerAsync([FromRoute] int id)
        {
            var result = await _productService.GetProductByIdForCustomerAsync(id);
            return Ok(new GenericResponse<ProductDetailCustomerDTO>("Producto encontrado exitosamente", result));
        }

        [HttpGet("/admin/{id}")]
        public async Task<IActionResult> GetProductByIdForAdminAsync([FromRoute] int id)
        {
            var result = await _productService.GetProductByIdForAdminAsync(id);
            return Ok(new GenericResponse<ProductDetailAdminDTO>("Producto encontrado exitosamente", result));
        }
    }
}