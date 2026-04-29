using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tienda_UCN_api.src.Application.DTO;
using TiendaUCN.src.Application.DTOs.ProductDTO;
using TiendaUCN.src.Application.DTOs.ProductDTO.Admin;
using TiendaUCN.src.Application.DTOs.ProductDTO.Customer;
using TiendaUCN.src.Application.Services.Interfaces;

namespace TiendaUCN.src.API.Controllers
{
    /// <summary>
    /// Controlador de productos.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class ProductController : ControllerBase
    {
        /// <summary>
        /// Interfaz del servicio de productos.
        /// </summary>
        private readonly IProductService _productService;

        /// <summary>
        /// Constructor del controlador de productos.
        /// </summary>
        /// <param name="productService">Interfaz del servicio de productos</param>
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Endpoint para crear un nuevo producto.
        /// </summary>
        /// <param name="createProductDTO">DTO para crear el producto</param>
        /// <returns>Id del producto creado</returns>
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductDTO createProductDTO)
        {
            var result = await _productService.CreateProductAsync(createProductDTO);
            return Created($"api/product/{result}", new GenericResponse<string>("Producto creado exitosamente", result));
        }

        /// <summary>
        /// Endpoint para cambiar el estado de un producto (activo/inactivo).
        /// </summary>
        /// <param name="id">El ID del producto</param>
        /// <returns>Un mensaje indicando el éxito de la operación</returns>
        [HttpPatch("switch-status/{id}")]
        public async Task<IActionResult> SwitchStatusProduct([FromRoute] int id)
        {
            var result = await _productService.SwitchStatusProductAsync(id);
            return Ok(new GenericResponse<string>("Estado del producto cambiado exitosamente", result));
        }

        /// <summary>
        /// Endpoint para obtener los detalles de un producto específico para clientes.
        /// </summary>
        /// <param name="id">El ID del producto</param>
        /// <returns>Los detalles del producto</returns>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductByIdForCustomer([FromRoute] int id)
        {
            var result = await _productService.GetProductByIdForCustomerAsync(id);
            return Ok(new GenericResponse<ProductDetailCustomerDTO>("Producto encontrado exitosamente", result));
        }

        /// <summary>
        /// Endpoint para obtener los detalles de un producto específico para administradores.
        /// </summary>
        /// <param name="id">El ID del producto</param>
        /// <returns>Los detalles del producto</returns>
        [HttpGet("admin/{id}")]
        public async Task<IActionResult> GetProductByIdForAdmin([FromRoute] int id)
        {
            var result = await _productService.GetProductByIdForAdminAsync(id);
            return Ok(new GenericResponse<ProductDetailAdminDTO>("Producto encontrado exitosamente", result));
        }

        /// <summary>
        /// Endpoint para eliminar un producto existente.
        /// </summary>
        /// <param name="id">El ID del producto</param>
        /// <returns>Mensaje de éxito</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            await _productService.DeleteProductAsync(id);
            return Ok(new GenericResponse<string>("Producto eliminado exitosamente", null));
        }

        /// <summary>
        /// Endpoint para obtener una lista de productos con filtros de búsqueda para clientes.
        /// </summary>
        /// <param name="searchParams">Los parámetros de búsqueda</param>
        /// <returns>La lista de productos</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ListProductsForCustomer([FromQuery] SearchParamsDTO searchParams)
        {
            var result = await _productService.GetListedProductsForCustomerAsync(searchParams);
            return Ok(new GenericResponse<ListedProductsForCustomerDTO>("Productos encontrados exitosamente", result));
        }

        /// <summary>
        /// Endpoint para obtener una lista de productos con filtros de búsqueda para administradores.
        /// </summary>
        /// <param name="searchParams">Los parámetros de búsqueda</param>
        /// <returns>La lista de productos</returns>
        [HttpGet("admin")]
        public async Task<IActionResult> ListProductsForAdmin([FromQuery] SearchParamsDTO searchParams)
        {
            var result = await _productService.GetListedProductsForAdminAsync(searchParams);
            return Ok(new GenericResponse<ListedProductsForAdminDTO>("Productos encontrados exitosamente", result));
        }

        /// <summary>
        /// Endpoint para actualizar un producto existente.
        /// </summary>
        /// <param name="id">El ID del producto</param>
        /// <param name="updateProductDTO">Los datos para actualizar el producto</param>
        /// <returns>Mensaje de éxito</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct([FromRoute] int id, [FromForm] UpdateProductDTO updateProductDTO)
        {
            await _productService.UpdateProductAsync(id, updateProductDTO);
            return Ok(new GenericResponse<string>("Producto actualizado exitosamente", null));
        }
    }
}