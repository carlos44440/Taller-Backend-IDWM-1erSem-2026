using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tienda_UCN_api.src.Application.DTO;
using TiendaUCN.src.Application.DTOs.BrandCategoryDTO;
using TiendaUCN.src.Application.Services.Interfaces;

namespace TiendaUCN.src.API.Controllers
{
    /// <summary>
    /// Controlador de categorías.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : ControllerBase
    {
        /// <summary>
        /// Interfaz del servicio de categoría.
        /// </summary>
        private readonly ICategoryService _categoryService;

        /// <summary>
        /// Constructor del controlador de categorías.
        /// </summary>
        /// <param name="categoryService">Interfaz del servicio de categoría</param>
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Endpoint para listar categorías activas.
        /// </summary>
        /// <returns>Lista de categorías</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetActiveCategories()
        {
            var categories = await _categoryService.GetActiveCategoriesAsync();
            return Ok(new GenericResponse<List<CatalogItemDTO>>("Categorías obtenidas exitosamente", categories));
        }

        /// <summary>
        /// Endpoint para crear una nueva categoría.
        /// </summary>
        /// <param name="createCategoryDTO">DTO para la creación de categoría</param>
        /// <returns>Mensaje de éxito</returns>
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CreateBrandCategoryDTO createCategoryDTO)
        {
            var message = await _categoryService.CreateCategoryAsync(createCategoryDTO);
            return Ok(new GenericResponse<string>(message, null));
        }

        /// <summary>
        /// Endpoint para actualizar una categoría existente.
        /// </summary>
        /// <param name="id">ID de la categoría a actualizar</param>
        /// <param name="updateCategoryDTO">DTO para la actualización de categoría</param>
        /// <returns>Mensaje de éxito</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] int id, [FromBody] UpdateBrandCategoryDTO updateCategoryDTO)
        {
            var message = await _categoryService.UpdateCategoryAsync(id, updateCategoryDTO);
            return Ok(new GenericResponse<string>(message, null));
        }

        /// <summary>
        /// Endpoint para eliminar una categoría existente.
        /// </summary>
        /// <param name="id">ID de la categoría a eliminar</param>
        /// <returns>Mensaje de éxito</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] int id)
        {
            var message = await _categoryService.DeleteCategoryAsync(id);
            return Ok(new GenericResponse<string>(message, null));
        }
    }
}