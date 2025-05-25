using HashidsNet;
using Microsoft.AspNetCore.Mvc;

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
    }
}
