using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Sieve.Models;
using Web_Shop.Application.Helpers.PagedList;
using Web_Shop.Application.Mappings;
using Web_Shop.Application.DTOs.Category;
using Web_Shop.Application.Services.Interfaces;
using Web_Shop.RestAPI.Controllers;
using HashidsNet;

namespace Web_Shop_3.RestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ILogger<CategoryController> logger, ICategoryService CategoryService, IHashids hashids) : base(hashids)
        {
            _categoryService = CategoryService;
            _logger = logger;
        }

        [HttpGet("{hashid}")]
        [SwaggerOperation(OperationId = "GetCategoryById")]
        public async Task<ActionResult<GetSingleCategoryDTO>> GetCategory(string hashid)
        {
            var validated = ValidateAndDecodeSingleId(hashid);

            if (validated.Result is not null)
            {
                return validated.Result;
            }

            var result = await _categoryService.GetByIdAsync(validated.Value);

            if (!result.IsSuccess)
            {
                return Problem(statusCode: (int)result.StatusCode, title: "Read error.", detail: result.ErrorMessage);
            }

            return StatusCode((int)result.StatusCode, result.entity!.MapGetSingleCategoryDTO(_hashIds));
        }

        [HttpGet("list")]
        [SwaggerOperation(OperationId = "GetCategories")]
        public async Task<ActionResult<IPagedList<GetSingleCategoryDTO>>> GetCategories([FromQuery] SieveModel paginationParams)
        {
            var result = await _categoryService.SearchAsync(paginationParams,
                                                            resultEntity => DomainToDtoMapper.MapGetSingleCategoryDTO(resultEntity, _hashIds));

            if (!result.IsSuccess)
            {
                return Problem(statusCode: (int)result.StatusCode, title: "Read error.", detail: result.ErrorMessage);
            }

            return Ok(result.entityList);
        }
    }
}