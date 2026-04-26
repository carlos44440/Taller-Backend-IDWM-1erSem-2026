using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tienda_UCN_api.src.Application.DTO;
using TiendaUCN.src.Application.DTOs.BrandCategoryDTO;
using TiendaUCN.src.Application.Services.Interfaces;

namespace TiendaUCN.src.API.Controllers
{
    /// <summary>
    /// Controlador de marcas.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class BrandController : ControllerBase
    {
        /// <summary>
        /// Interfaz del servicio de marca.
        /// </summary>
        private readonly IBrandService _brandService;

        /// <summary>
        /// Constructor del controlador de marcas.
        /// </summary>
        /// <param name="brandService">Interfaz del servicio de marca</param>
        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }


        /// <summary>
        /// Endpoint para crear una nueva marca.
        /// </summary>
        /// <param name="createBrandDTO">DTO para la creación de marca</param>
        /// <returns>Mensaje de éxito</returns>
        [HttpPost]
        public async Task<IActionResult> CreateBrand([FromBody] CreateBrandCategoryDTO createBrandDTO)
        {
            var message = await _brandService.CreateBrandAsync(createBrandDTO);
            return Ok(new GenericResponse<string>(message, null));
        }

        /// <summary>
        /// Endpoint para actualizar una marca existente.
        /// </summary>
        /// <param name="id">ID de la marca a actualizar</param>
        /// <param name="updateBrandDTO">DTO para la actualización de marca</param>
        /// <returns>Mensaje de éxito</returns>
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateBrand([FromRoute] int id, [FromBody] UpdateBrandCategoryDTO updateBrandDTO)
        {
            var message = await _brandService.UpdateBrandAsync(id, updateBrandDTO);
            return Ok(new GenericResponse<string>(message, null));
        }

        /// <summary>
        /// Endpoint para eliminar una marca existente.
        /// </summary>
        /// <param name="id">ID de la marca a eliminar</param>
        /// <returns>Mensaje de éxito</returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteBrand([FromRoute] int id)
        {
            var message = await _brandService.DeleteBrandAsync(id);
            return Ok(new GenericResponse<string>(message, null));
        }
    }
}