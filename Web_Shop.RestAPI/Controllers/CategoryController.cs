using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Sieve.Models;
using Web_Shop.Application.Helpers.PagedList;
using Web_Shop.Application.Mappings;
using Web_Shop.Application.DTOs.Category;

namespace Web_Shop_3.RestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ILogger<CategoryController> logger, ICategoryService CategoryService)
        {
            _categoryService = CategoryService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [SwaggerOperation(OperationId = "GetCategoryById")]
        public async Task<ActionResult<GetSingleCategoryDTO>> GetCategory(uint id)
        {
            var result = await _categoryService.GetByIdAsync(id);

            if (!result.IsSuccess)
            {
                return Problem(statusCode: (int)result.StatusCode, title: "Read error.", detail: result.ErrorMessage);
            }

            return StatusCode((int)result.StatusCode, result.entity!.MapGetSingleCategoryDTO());
        }

        [HttpGet("list")]
        [SwaggerOperation(OperationId = "GetCategories")]
        public async Task<ActionResult<IPagedList<GetSingleCategoryDTO>>> GetCategories([FromQuery] SieveModel paginationParams)
        {
            var result = await _categoryService.SearchAsync(paginationParams,
                                                            resultEntity => DomainToDtoMapper.MapGetSingleCategoryDTO(resultEntity));

            if (!result.IsSuccess)
            {
                return Problem(statusCode: (int)result.StatusCode, title: "Read error.", detail: result.ErrorMessage);
            }

            return Ok(result.entityList);
        }
    }
}