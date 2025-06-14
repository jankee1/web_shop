﻿using FluentValidation;
using HashidsNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sieve.Models;
using Sieve.Services;
using Web_Shop.Application.CustomQueries;
using Web_Shop.Application.DTOs;
using Web_Shop.Application.Mappings.PropertiesMappings;
using Web_Shop.Application.Services;
using Web_Shop.Application.Services.Interfaces;
using Web_Shop.Application.Validation;

namespace Web_Shop.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SieveOptions>(sieveOptions =>
            {
                configuration.GetSection("Sieve").Bind(sieveOptions);
            });

            services
                 .AddSingleton<IHashids>(_ =>
                 {
                     var hashIdsSalt = configuration["Secret:hashids:salt"];
                     var hashIdsLength = int.Parse(configuration["Secret:hashids:length"]);
                     return new Hashids(hashIdsSalt, hashIdsLength);
                 });

            services
                .AddScoped<ISieveCustomSortMethods, SieveCustomSortMethods>()
                .AddScoped<ISieveCustomFilterMethods, SieveCustomFilterMethods>()
                .AddScoped<ISieveProcessor, ApplicationSieveProcessor>();

            services
                .AddScoped(typeof(ICategoryService), typeof(CategoryService))
                .AddScoped(typeof(ICustomerService), typeof(CustomerService));

            services.AddScoped<IValidator<AddUpdateCustomerDTO>, AddUpdateCustomerDTOValidator>();
        }
    }
}
