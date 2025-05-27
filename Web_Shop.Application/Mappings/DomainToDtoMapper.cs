using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HashidsNet;
using Web_Shop.Application.DTOs;
using Web_Shop.Application.DTOs.Product;
using WWSI_Shop.Persistence.MySQL.Model;
using Web_Shop.Application.DTOs.Category;
using Web_Shop.Application.Utils;

namespace Web_Shop.Application.Mappings
{
    public static class DomainToDtoMapper
    {
        public static GetSingleCustomerDTO MapGetSingleCustomerDTO(this Customer domainCustomer, IHashids hashIds)
        {
            if (domainCustomer == null)
                throw new ArgumentNullException(nameof(domainCustomer));

            GetSingleCustomerDTO getSingleCustomerDTO = new()
            {
                hashIdCustomer = domainCustomer.IdCustomer.EncodeHashId(hashIds),
                Name = domainCustomer.Name,
                Surname = domainCustomer.Surname,
                Email = domainCustomer.Email,
                BirthDate = domainCustomer.BirthDate,
            };

            return getSingleCustomerDTO;
        }

        public static GetSingleCategoryDTO MapGetSingleCategoryDTO(this Category domainCategory, IHashids hashIds)
        {
            if (domainCategory == null)
                throw new ArgumentNullException(nameof(domainCategory));

            GetSingleCategoryDTO getSingleCategoryDTO = new()
            {
                IdCategory = domainCategory.IdCategory.EncodeHashId(hashIds),
                Name = domainCategory.Name,
                Description = domainCategory.Description,
            };

            return getSingleCategoryDTO;
        }

        public static GetSingleProductDTO MapGetSingleProductDTO(this Product domainProduct, IHashids hashIds)
        {
            if (domainProduct == null)
                throw new ArgumentNullException(nameof(domainProduct));

            GetSingleProductDTO getSingleProductDTO = new()
            {
                ProductId = domainProduct.IdProduct.EncodeHashId(hashIds),
                Name = domainProduct.Name,
                Description = domainProduct.Description,
                Price = domainProduct.Price,
                Sku = domainProduct.Sku,
            };

            if(domainProduct.IdCategories != null && domainProduct.IdCategories.Count != 0)
            {
                getSingleProductDTO.Category = domainProduct.IdCategories
                    .Select(c => c.MapGetSingleCategoryDTO(hashIds))
                    .ToList();
            }

            return getSingleProductDTO;
        }
    }
}
