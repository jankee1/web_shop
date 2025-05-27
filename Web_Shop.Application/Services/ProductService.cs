using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using Web_Shop.Application.DTOs.Product;
using Web_Shop.Application.Mappings;
using Web_Shop.Application.Services.Interfaces;
using Web_Shop.Application.Utils;
using Web_Shop.Persistence.UOW.Interfaces;
using WWSI_Shop.Persistence.MySQL.Model;

namespace Web_Shop.Application.Services
{
    public class ProductService : BaseService<Product>, IProductService
    {
        public ProductService(ILogger<Product> logger,
                              ISieveProcessor sieveProcessor,
                              IOptions<SieveOptions> sieveOptions,
                              IUnitOfWork unitOfWork)
            : base(logger, sieveProcessor, sieveOptions, unitOfWork)
        {
        }

        public async Task<(bool IsSuccess, Product? entity, HttpStatusCode StatusCode, string ErrorMessage)> CreateNewProductAsync(AddUpdateProductDto dto, ICollection<ulong> categoryIds)
        {
            try
            {
                var newEntity = dto.MapProduct();
                var categories = await _unitOfWork.CategoryRepository.WithoutTracking().GetManyByIdAsync<ulong>(categoryIds, cat => cat.IdCategory);

                if(categoryIds.Count != categories.Count)
                {
                    var missingIds = CollectionHelper.GetDifference(categoryIds, CollectionHelper.MapTo(categories, c => c.IdCategory));
                    return (false, null, HttpStatusCode.NotFound, "Category IDs not found: " + string.Join(", ", missingIds));
                }

                foreach (var category in categories)
                {
                    newEntity.IdCategories.Add(category);
                }

                var result = await AddAndSaveAsync(newEntity);

                return (true, result.entity, HttpStatusCode.OK, string.Empty);
            }
            catch (Exception ex)
            {
                return LogError(ex.Message);
            }
        }

        public async Task<(bool IsSuccess, Product? entity, HttpStatusCode StatusCode, string ErrorMessage)> UpdateExistingProductAsync(AddUpdateProductDto dto, ulong id, ICollection<ulong> categoryIds)
        {
            try
            {
                var existingEntity = await WithoutTracking().GetByIdAsync(id);

                if (!existingEntity.IsSuccess)
                {
                    return (false, null, HttpStatusCode.NotFound, "Product not found.");
                }

                var categories = await _unitOfWork.CategoryRepository.GetManyByIdAsync<ulong>(categoryIds, cat => cat.IdCategory);

                if (categoryIds.Count != categories.Count)
                {
                    var missingIds = CollectionHelper.GetDifference(categoryIds, CollectionHelper.MapTo(categories, c => c.IdCategory));
                    return (false, null, HttpStatusCode.NotFound, "Category IDs not found: " + string.Join(", ", missingIds));
                }

                var updatedEntity = dto.MapProduct();
                updatedEntity.IdProduct = id;

                foreach (var category in categories)
                {
                    updatedEntity.IdCategories.Add(category);
                }

                var result = await UpdateAndSaveAsync(updatedEntity, id);

                return (true, result.entity, HttpStatusCode.OK, string.Empty);
            }
            catch (Exception ex)
            {
                return LogError(ex.Message);
            }
        }

        public async Task<(bool IsSuccess, Product? entity, HttpStatusCode StatusCode, string ErrorMessage)> GetProductWithDetails(ulong id)
        {
            try
            {
                var productEntity = await _unitOfWork.ProductRepository.GetOneByIdWithRelationAsync(id, p => p.IdCategories);

                if (productEntity == null)
                {
                    return (false, null, HttpStatusCode.NotFound, "Product not found.");
                }
   
                return (true, productEntity, HttpStatusCode.OK, string.Empty);
            }
            catch (Exception ex)
            {
                return LogError(ex.Message);
            }
        }

        public async Task<(bool IsSuccess, Product? entity, HttpStatusCode StatusCode, string ErrorMessage)> Delete(ulong productId)
        {
            var productEntity = await _unitOfWork.ProductRepository.GetOneByIdWithRelationAsync(productId, p => p.IdCategories);

            if (productEntity == null)
            {
                return (false, null, HttpStatusCode.NotFound, "Product not found.");
            }

            productEntity.IdCategories.Clear();
            await _unitOfWork.ProductRepository.DeleteAsync(productEntity);

            return (true, productEntity, HttpStatusCode.OK, string.Empty);
        }
    }
}
