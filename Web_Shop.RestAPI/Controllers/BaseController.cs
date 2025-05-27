using System.Net;
using HashidsNet;
using Microsoft.AspNetCore.Mvc;
using Web_Shop.Application.Utils;
using WWSI_Shop.Persistence.MySQL.Model;

namespace Web_Shop.RestAPI.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IHashids _hashIds;

        public BaseController(IHashids hashIds)
        {
            _hashIds = hashIds;
        }

        protected ActionResult<ulong> ValidateAndDecodeSingleId(string encodedId)
        {
            try
            {
                ulong id = encodedId.DecodeHashId(_hashIds);
                return id;
            }
            catch (Exception ex)
            {
                return Problem(statusCode: (int)HttpStatusCode.BadRequest, title: "Hash decode error.", detail: ex.Message);
            }
        }
    }
}
