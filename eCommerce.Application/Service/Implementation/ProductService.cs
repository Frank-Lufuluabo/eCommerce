﻿using AutoMapper;
using eCommerce.Application.DTOs;
using eCommerce.Application.DTOs.Product;
using eCommerce.Application.Service.Interface;
using eCommerce.Domain.Entities;
using eCommerce.Domain.Interfaces;


namespace eCommerce.Application.Service.Implementation
{
    public class ProductService(IGeneric<Product> productInterface, IMapper mapper) : IProductService
    {
        public async Task<ServiceResponse> AddAsync(CreateProduct product)
        {
            var mappedData = mapper.Map<Product>(product);
            int result = await productInterface.AddAsync(mappedData);
            return result > 0 ? new ServiceResponse(true, "Product added!") :
                new ServiceResponse(false, "Product failed to be added");
        }

        public async Task<ServiceResponse> DeleteAsync(Guid id)
        {
            int result = await productInterface.DeleteAsync(id);

            return result > 0 ? new ServiceResponse(true, "Product deleted!") :
                new ServiceResponse(false, "Product failed or to be delete.");
        }

        public async Task<IEnumerable<GetProduct>> GetAllAsync()
        {
            var rawData = await productInterface.GetAllAsync();
            if (!rawData.Any()) return [];

            return mapper.Map<IEnumerable<GetProduct>>(rawData);
        }

        public async Task<GetProduct> GetByIdAsync(Guid id)
        {
            var rawData = await productInterface.GetByIdAsync(id);
            if (rawData == null) return new GetProduct();

            return mapper.Map<GetProduct>(rawData);
        }

        public async Task<ServiceResponse> UpdateAsync(UpdateProduct product)
        {
            var mappedData = mapper.Map<Product>(product);
            int result = await productInterface.UpdateAsync(mappedData);
            return result > 0 ? new ServiceResponse(true, "Product updated!") :
                new ServiceResponse(false, "Product failed to be updated!");
        }
    }
}
