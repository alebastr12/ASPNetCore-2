using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using WebStore.Clients.Base;
using WebStore.Domain.Entitys;
using WebStore.Domain.EntitysDTO;
using WebStore.Domain.Filters;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.ProductClient
{
    public class ProductClient : BaseClient, IProductService
    {
        public ProductClient(IConfiguration conf):base(conf,"api/products")
        {

        }

        public void AddProduct(ProductDTO item) => Post<ProductDTO>($"{_serviceAddress}/add", item);

        public void Commit() { }

        public void Delete(int? id) => Delete($"{_serviceAddress}/{id}");

        public BrandDTO GetBrandById(int Id) => Get<BrandDTO>($"{_serviceAddress}/brands/{Id}");

        public IEnumerable<BrandDTO> GetBrands() => Get<List<BrandDTO>>($"{_serviceAddress}/brands");

        public IEnumerable<CategoryDTO> GetCategories()=> Get<List<CategoryDTO>>($"{_serviceAddress}/categoryes");

        public CategoryDTO GetCategoryById(int Id)=> Get<CategoryDTO>($"{_serviceAddress}/categoryes/{Id}");

        public ProductDTO GetProductById(int id) => Get<ProductDTO>($"{_serviceAddress}/{id}");

        public IEnumerable<ProductDTO> GetProducts(ProductFilter filter) => Post(_serviceAddress, filter)
            .Content
            .ReadAsAsync<List<ProductDTO>>()
            .Result;

        public void UpdateProduct(ProductDTO item) => Put<ProductDTO>(_serviceAddress, item);
    }
}
