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
    public class BrandController : ControllerBase
    {
        private readonly IBrandService _brandService;
        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateBrand(CreateBrandCategoryDTO createBrandDTO)
        {
            var message = await _brandService.CreateBrandAsync(createBrandDTO);
            return Ok(new GenericResponse<string>(message, null));
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateBrand(int id, UpdateBrandCategoryDTO updateBrandDTO)
        {
            var message = await _brandService.UpdateBrandAsync(id, updateBrandDTO);
            return Ok(new GenericResponse<string>(message, null));
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            var message = await _brandService.DeleteBrandAsync(id);
            return Ok(new GenericResponse<string>(message, null));
        }
    }
}