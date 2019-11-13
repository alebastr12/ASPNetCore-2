using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entitys;
using WebStore.Domain.EntitysDTO;
using WebStore.Domain.Filters;
using WebStore.Interfaces.Services;

namespace WebStore.ServicesHosting.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase, IProductService
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }
        [HttpPost("add")]
        public void AddProduct([FromBody] ProductDTO item) => productService.AddProduct(item);
        [NonAction]
        public void Commit() { }
        [HttpDelete("{id?}")]
        public void Delete(int? id) => productService.Delete(id);
        [HttpGet("brands")]
        public IEnumerable<BrandDTO> GetBrands() => productService.GetBrands();
        [HttpGet("categoryes")]
        public IEnumerable<CategoryDTO> GetCategories() => productService.GetCategories();
        [HttpGet("{id}")]
        public ProductDTO GetProductById(int id) => productService.GetProductById(id);
        [HttpPost]
        public IEnumerable<ProductDTO> GetProducts([FromBody] ProductFilter filter) => productService.GetProducts(filter);
    }
}