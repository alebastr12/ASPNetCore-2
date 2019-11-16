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
        /// <summary>
        /// Конструктор сервиса
        /// </summary>
        /// <param name="productService">Сервис работы с продуктами</param>
        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }
        /// <summary>
        /// Добавить продукт в базу
        /// </summary>
        /// <param name="item">Продукт для добавления</param>
        [HttpPost("add")]
        public void AddProduct([FromBody] ProductDTO item) => productService.AddProduct(item);
        /// <summary>
        /// 
        /// </summary>
        [NonAction]
        public void Commit() { }
        /// <summary>
        /// Удалить продукт из базы
        /// </summary>
        /// <param name="id">Идентификатор продукта, который нужно удалить</param>
        [HttpDelete("{id?}")]
        public void Delete(int? id) => productService.Delete(id);
        /// <summary>
        /// Получить список брэндов
        /// </summary>
        /// <returns>Список всех брэндов</returns>
        [HttpGet("brands")]
        public IEnumerable<BrandDTO> GetBrands() => productService.GetBrands();
        /// <summary>
        /// Получить список категорий
        /// </summary>
        /// <returns>Список всех категорий</returns>
        [HttpGet("categoryes")]
        public IEnumerable<CategoryDTO> GetCategories() => productService.GetCategories();
        /// <summary>
        /// Получить продукт по его идентификатору
        /// </summary>
        /// <param name="id">Идентификатор продукта</param>
        /// <returns>Продукт по заданному идентификатору</returns>
        [HttpGet("{id}")]
        public ProductDTO GetProductById(int id) => productService.GetProductById(id);
        /// <summary>
        /// Получить список продуктов по заданному фильтру
        /// </summary>
        /// <param name="filter">Фильтр запроса</param>
        /// <returns>Список продуктов</returns>
        [HttpPost]
        public IEnumerable<ProductDTO> GetProducts([FromBody] ProductFilter filter) => productService.GetProducts(filter);
    }
}