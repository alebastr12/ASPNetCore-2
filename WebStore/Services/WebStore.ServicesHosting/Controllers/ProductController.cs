using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ProductController> logger;

        /// <summary>
        /// Конструктор сервиса
        /// </summary>
        /// <param name="productService">Сервис работы с продуктами</param>
        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            this.productService = productService;
            this.logger = logger;
        }
        /// <summary>
        /// Добавить продукт в базу
        /// </summary>
        /// <param name="item">Продукт для добавления</param>
        [HttpPost("add")]
        public void AddProduct([FromBody] ProductDTO item)
        {
            if (item is null)
            {
                logger.LogError("При добавлении нового продукта передана пустая ссылка на объект.");
                throw new ArgumentNullException(nameof(item));
            }
            logger.LogInformation($"Добавление нового продукта: id {item.Id}, Имя {item.Name}");
            productService.AddProduct(item);
        }
        
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
        public void Delete(int? id) 
        {
            if (id.HasValue)
            {
                logger.LogInformation($"Удаление продукта с идентификатором {id}");
                productService.Delete(id);
            }
            else
                logger.LogError("При удалении продукта передано пустое значение идентификатора");
                
        }
        /// <summary>
        /// Получить брэнд по его идентификатору
        /// </summary>
        /// <param name="Id">Идентификатор бренда</param>
        /// <returns>Брэнд</returns>
        [HttpGet("brands/{Id}")]
        public BrandDTO GetBrandById(int Id) => productService.GetBrandById(Id);

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
        /// Получить категорию по ее идентификатору
        /// </summary>
        /// <param name="Id">Идетификатор категории</param>
        /// <returns>Категория</returns>
        [HttpGet("categoryes/{Id}")]
        public CategoryDTO GetCategoryById(int Id) => productService.GetCategoryById(Id);

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
        [HttpPut]
        public void UpdateProduct(ProductDTO item)
        {
            if (item is null)
            {
                logger.LogError("При обновлении продукта передана пустая ссылка.");
                throw new ArgumentNullException(nameof(item));
            }
            logger.LogInformation($"Редактировние данных продукта Id - {item.Id}, имя - {item.Name}");
            productService.UpdateProduct(item);

        }
    }
}