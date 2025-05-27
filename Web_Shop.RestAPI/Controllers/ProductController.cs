using HashidsNet;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using Swashbuckle.AspNetCore.Annotations;
using Web_Shop.Application.DTOs.Product;
using Web_Shop.Application.Helpers.PagedList;
using Web_Shop.Application.Mappings;
using Web_Shop.Application.Services.Interfaces;
using Web_Shop.Application.Utils;

namespace Web_Shop.RestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger, IProductService productService, IHashids hashids) : base(hashids)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet("{hashid}")]
        [SwaggerOperation(OperationId = "GetProductById")]
        public async Task<ActionResult<GetSingleProductDTO>> GetProductWithDetails(string hashid)
        {
            var validated = ValidateAndDecodeSingleId(hashid);

            if (validated.Result is not null)
            {
                return validated.Result;
            }

            var result = await _productService.GetProductWithDetails(validated.Value);

            if (!result.IsSuccess)
            {
                return Problem(statusCode: (int)result.StatusCode, title: "Read error.", detail: result.ErrorMessage);
            }

            return StatusCode((int)result.StatusCode, result.entity!.MapGetSingleProductDTO(_hashIds));
        }

        [HttpGet("list")]
        [SwaggerOperation(OperationId = "GetProducts")]
        public async Task<ActionResult<IPagedList<GetSingleProductDTO>>> GeProducts([FromQuery] SieveModel paginationParams)
        {
            var result = await _productService.SearchAsync(paginationParams, resultEntity => DomainToDtoMapper.MapGetSingleProductDTO(resultEntity, _hashIds));

            if (!result.IsSuccess)
            {
                return Problem(statusCode: (int)result.StatusCode, title: "Read error.", detail: result.ErrorMessage);
            }

            return Ok(result.entityList);
        }

        [HttpPost("add")]
        [SwaggerOperation(OperationId = "AddProduct")]
        public async Task<ActionResult<GetSingleProductDTO>> AddProduct([FromBody] AddUpdateProductDto dto)
        {
            var categoryIds = new List<ulong>();

            foreach (var hashid in dto.IdCategories)
            {
                var item = ValidateAndDecodeSingleId(hashid);
                if (item.Result is not null)
                {
                    return item.Result;
                }
                else
                {
                    categoryIds.Add(item.Value);
                }
            }

            var result = await _productService.CreateNewProductAsync(dto, categoryIds);

            if (!result.IsSuccess)
            {
                return Problem(statusCode: (int)result.StatusCode, title: "Add error.", detail: result.ErrorMessage);
            }

            return CreatedAtAction(nameof(AddProduct), new { id = result.entity!.IdProduct }, result.entity.MapGetSingleProductDTO(_hashIds));
        }

        [HttpPut("update/{hashid}")]
        [SwaggerOperation(OperationId = "UpdateProduct")]
        public async Task<ActionResult<GetSingleProductDTO>> UpdateProduct(string hashid, [FromBody] AddUpdateProductDto dto)
        {
            ulong validatedProductId = 0;
            var hashIds = dto.IdCategories.ToList();
            hashIds.Insert(0, hashid);
            var categoryIds = new List<ulong>();

            for (int i = 0; i < hashIds.Count; i++)
            {
                var item = ValidateAndDecodeSingleId(hashIds[i]);
  
                if (item.Result is not null)
                {
                    return item.Result;
                } else if (i == 0)
                {
                    validatedProductId = item.Value;
                } else
                {
                    categoryIds.Add(item.Value);
                }
            }

            var result = await _productService.UpdateExistingProductAsync(dto, validatedProductId, categoryIds);

            if (!result.IsSuccess)
            {
                return Problem(statusCode: (int)result.StatusCode, title: "Update error.", detail: result.ErrorMessage);
            }

            return StatusCode((int)result.StatusCode, result.entity!.MapGetSingleProductDTO(_hashIds));
        }

        [HttpDelete("{hashid}")]
        [SwaggerOperation(OperationId = "DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(string hashid)
        {
            var validated = ValidateAndDecodeSingleId(hashid);

            if (validated.Result is not null)
            {
                return validated.Result;
            }

            var result = await _productService.DeleteAndSaveAsync(validated.Value);

            if (!result.IsSuccess)
            {
                return Problem(statusCode: (int)result.StatusCode, title: "Delete error.", detail: result.ErrorMessage);
            }

            return NoContent();
        }
    }
}
