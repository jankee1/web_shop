using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using Web_Shop.Application.DTOs;
using Web_Shop.Application.Services.Interfaces;
using Web_Shop.Persistence.UOW.Interfaces;
using Web_Shop.Application.Mappings;
using Sieve.Models;
using Web_Shop.Application.Helpers.PagedList;
using WWSI_Shop.Persistence.MySQL.Model;

using Web_Shop.Application.Utils;
using System.Net;
using HashidsNet;

namespace Web_Shop.RestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : BaseController
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ILogger<CustomerController> logger, ICustomerService customerService, IHashids hashids) : base(hashids)
        {
            _customerService = customerService;
            _logger = logger;
        }

        [HttpGet("{hashid}")]
        [SwaggerOperation(OperationId = "GetCustomerById")]
        public async Task<ActionResult<GetSingleCustomerDTO>> GetCustomer(string hashid)
        {
            ulong id;
            try
            {
                id = hashid.DecodeHashId(_hashIds);
            }
            catch (Exception ex)
            {
                return Problem(statusCode: (int)HttpStatusCode.BadRequest, title: "Hash decode error.", detail: ex.Message);
            }
            var result = await _customerService.GetByIdAsync(id);

            if (!result.IsSuccess)
            {
                return Problem(statusCode: (int)result.StatusCode, title: "Read error.", detail: result.ErrorMessage);
            }

            return StatusCode((int)result.StatusCode, result.entity!.MapGetSingleCustomerDTO(_hashIds));
        }

        [HttpGet("list")]
        [SwaggerOperation(OperationId = "GetCustomers")]
        public async Task<ActionResult<IPagedList<GetSingleCustomerDTO>>> GetCustomers([FromQuery] SieveModel paginationParams)
        {
            var result = await _customerService.SearchAsync(paginationParams, resultEntity => DomainToDtoMapper.MapGetSingleCustomerDTO(resultEntity, _hashIds));

            if (!result.IsSuccess)
            {
                return Problem(statusCode: (int)result.StatusCode, title: "Read error.", detail: result.ErrorMessage);
            }

            return Ok(result.entityList);
        }

        [HttpPost("add")]
        [SwaggerOperation(OperationId = "AddCustomer")]
        public async Task<ActionResult<GetSingleCustomerDTO>> AddCustomer([FromBody] AddUpdateCustomerDTO dto)
        {
            var result = await _customerService.CreateNewCustomerAsync(dto);

            if (!result.IsSuccess)
            {
                return Problem(statusCode: (int)result.StatusCode, title: "Add error.", detail: result.ErrorMessage);
            }

            return CreatedAtAction(nameof(GetCustomer), new { id = result.entity!.IdCustomer }, result.entity.MapGetSingleCustomerDTO(_hashIds));
        }

        [HttpPut("update/{id}")]
        [SwaggerOperation(OperationId = "UpdateCustomer")]
        public async Task<ActionResult<GetSingleCustomerDTO>> UpdateCustomer(ulong id, [FromBody] AddUpdateCustomerDTO dto)
        {
            var result = await _customerService.UpdateExistingCustomerAsync(dto, id);

            if (!result.IsSuccess)
            {
                return Problem(statusCode: (int)result.StatusCode, title: "Update error.", detail: result.ErrorMessage);
            }

            return StatusCode((int)result.StatusCode, result.entity!.MapGetSingleCustomerDTO(_hashIds));
        }

        [HttpGet("verifyPassword/{email}/{password}")]
        [SwaggerOperation(OperationId = "VerifyPasswordByEmail")]
        public async Task<ActionResult<GetSingleCustomerDTO>> VerifyPasswordByEmail(string email, string password)
        {
            var result = await _customerService.VerifyPasswordByEmail(email, password);

            if (!result.IsSuccess)
            {
                return Problem(statusCode: (int)result.StatusCode, title: "Read error.", detail: result.ErrorMessage);
            }

            return StatusCode((int)result.StatusCode, result.entity!.MapGetSingleCustomerDTO(_hashIds));
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(OperationId = "DeleteCustomer")]
        public async Task<IActionResult> DeleteCustomer(ulong id)
        {
            var result = await _customerService.DeleteAndSaveAsync(id);

            if (!result.IsSuccess)
            {
                return Problem(statusCode: (int)result.StatusCode, title: "Delete error.", detail: result.ErrorMessage);
            }

            return NoContent();
        }
    }
}
