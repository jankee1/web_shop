using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using System.Net;
using Web_Shop.Application.Services;
using Web_Shop.Application.Services.Interfaces;
using Web_Shop.Persistence.UOW.Interfaces;
using WWSI_Shop.Persistence.MySQL.Model;

namespace Web_Shop.Application.Services
{
    public class CategoryService : BaseService<Category>, ICategoryService
    {
        public CategoryService(ILogger<Category> logger,
                               ISieveProcessor sieveProcessor,
                               IOptions<SieveOptions> sieveOptions,
                               IUnitOfWork unitOfWork)
            : base(logger, sieveProcessor, sieveOptions, unitOfWork)
        {

        }
    }
}