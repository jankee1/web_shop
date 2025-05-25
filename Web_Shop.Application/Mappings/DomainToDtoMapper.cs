using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Shop.Application.DTOs;
using Web_Shop_3.Application.DTOs.CustomerDTOs;
using Web_Shop.Application.DTOs.Product;
using WWSI_Shop.Persistence.MySQL.Model;

namespace Web_Shop.Application.Mappings
{
    public static class DomainToDtoMapper
    {
        public static GetSingleCustomerDTO MapGetSingleCustomerDTO(this Customer domainCustomer)
        {
            if (domainCustomer == null)
                throw new ArgumentNullException(nameof(domainCustomer));

            GetSingleCustomerDTO getSingleCustomerDTO = new()
            {
                IdCustomer = domainCustomer.IdCustomer,
                Name = domainCustomer.Name,
                Surname = domainCustomer.Surname,
                Email = domainCustomer.Email,
                BirthDate = domainCustomer.BirthDate,
            };

            return getSingleCustomerDTO;
        }

        public static GetSingleCategoryDTO MapGetSingleCategoryDTO(this Category domainCategory)
        {
            if (domainCategory == null)
                throw new ArgumentNullException(nameof(domainCategory));

            GetSingleCategoryDTO getSingleCategoryDTO = new()
            {
                IdCategory = domainCategory.IdCategory,
                Name = domainCategory.Name,
                Description = domainCategory.Description,
            };

            return getSingleCategoryDTO;
        }

        public static GetSingleProductDTO MapGetSingleProductDTO(this Product domainProduct)
        {
            if (domainProduct == null)
                throw new ArgumentNullException(nameof(domainProduct));

            GetSingleProductDTO getSingleProductDTO = new()
            {
                Name = domainProduct.Name,
                Description = domainProduct.Description,
                Price = domainProduct.Price,
                Sku = domainProduct.Sku,
            };

            return getSingleProductDTO;
        }
    }
}
