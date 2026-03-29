using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tienda_UCN_api.src.Application.DTO;
using TiendaUCN.src.Application.DTOs.ProductDTO;
using TiendaUCN.src.Application.Services.Interfaces;

namespace TiendaUCN.src.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateBrandCategoryDTO createCategoryDTO)
        {
            var message = await _categoryService.CreateCategoryAsync(createCategoryDTO);
            return Ok(new GenericResponse<string>(message, null));
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] int id, [FromBody] UpdateBrandCategoryDTO updateCategoryDTO)
        {
            var message = await _categoryService.UpdateCategoryAsync(id, updateCategoryDTO);
            return Ok(new GenericResponse<string>(message, null));
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] int id)
        {
            var message = await _categoryService.DeleteCategoryAsync(id);
            return Ok(new GenericResponse<string>(message, null));
        }
    }
}