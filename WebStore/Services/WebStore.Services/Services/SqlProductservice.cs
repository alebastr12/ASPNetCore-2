using WebStore.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL;
using WebStore.Domain.Entitys;
using WebStore.Domain.Filters;
using WebStore.Domain.EntitysDTO;
using WebStore.Services.Map;
using Microsoft.Extensions.Logging;

namespace WebStore.Services.Services
{
    public class SqlProductService:IProductService
    {
        private readonly WebStoreContext _context;
        private readonly ILogger logger;

        public SqlProductService(WebStoreContext context, ILogger<SqlProductService> logger)
        {
            _context = context;
            this.logger = logger;
        }

        public void AddProduct(ProductDTO item)
        {
            using(logger.BeginScope("Добавление нового продукта в БД."))
            {
                if (item is null)
                {
                    logger.LogError("При попытке добавления пользователя обнаружено, что передана пустая ссылка на тип.");
                    throw new ArgumentNullException(nameof(item));
                }
                logger.LogInformation($"Добавление продукта с именем {item.Name}");    
                _context.Products.Add(item.FromDTO());
                if (_context.SaveChanges() > 0)
                    logger.LogInformation($"Добавление продукта с именем {item.Name} прошло успешно. ИД: {item.Id}");
                else
                    logger.LogError($"При записи продукта {item.Name} в БД произошла ошибка.");
            }
            
        }
        public void AddProduct(Product item)
        {
            if (item is null)
                return;
            _context.Products.Add(item);
            Commit();
        }
        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Delete(int? id)
        {
            using(logger.BeginScope("Удаление продукта из БД"))
            {
                if (id.HasValue)
                {
                    var prod = _context.Products.FirstOrDefault(p => p.Id == id);
                    if (prod is null)
                    {
                        logger.LogError($"Продукт с ИД {id} отсутствует в БД.");
                        return;
                    }
                    logger.LogInformation($"Попытка удалить продукт с ИД {id} из БД");    
                    _context.Products.Remove(prod);
                    if (_context.SaveChanges() > 0)
                        logger.LogInformation($"Удаление продукта с ИД {id} прошло успешно.");
                    else
                        logger.LogError($"При удалении продукта с ИД {id} в БД произошла ошибка.");
                }
                else
                    logger.LogWarning("Id продукта не содержит значения.");
            }
            
        }

        public BrandDTO GetBrandById(int Id)
        {
            return _context.Brands.FirstOrDefault(b => b.Id == Id).ToDTO();
        }

        public IEnumerable<BrandDTO> GetBrands()
        {
            return _context.Brands.AsEnumerable().Select(BrandMapper.ToDTO);
        }

        public IEnumerable<CategoryDTO> GetCategories()
        {
            var cat = _context.Categories
                .Include(category => category.ParentCategory)
                .AsEnumerable()
                .Select(CategoryMapper.ToDTO);
            return cat;
        }

        public CategoryDTO GetCategoryById(int Id)
        {
            return _context.Categories.FirstOrDefault(b => b.Id == Id).ToDTO();
        }

        public ProductDTO GetProductById(int id)
        {
            return _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .FirstOrDefault(p => p.Id == id)
                .ToDTO();
        }

        public IEnumerable<ProductDTO> GetProducts(ProductFilter filter)
        {
            var dbItems = _context.Products.Include(p=>p.Category).Include(p=>p.Brand).AsQueryable();
            if (filter.Ids?.Count > 0)
            {
                dbItems = dbItems.Where(p => filter.Ids.Contains(p.Id));
            }
            if (filter.BrandId.HasValue)
            {
                dbItems = dbItems.Where(p => p.BrandId == filter.BrandId);
            }
            if (filter.CategoryId.HasValue)
            {
                dbItems = dbItems.Where(p => p.CategoryId == filter.CategoryId);
            }
            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                dbItems = dbItems.Where(p => p.Name.Contains(filter.Name));
            }
            return dbItems.AsEnumerable().Select(ProductMapper.ToDTO);
        }

        public void UpdateProduct(ProductDTO item)
        {
            using (logger.BeginScope("Обновление данных о продукте."))
            {
                if (item is null)
                {
                    logger.LogError($"При обновлении данных о продукте передана пустая ссылка на тип.");
                    throw new ArgumentNullException(nameof(item));
                }
                var prod = _context.Products.FirstOrDefault(p => p.Id == item.Id);
                if (prod is null)
                {
                    logger.LogError($"Продукт с ИД {item.Id} не найден в БД.");
                    return;
                }
                logger.LogInformation($"Обновление данных о продукте ИД: {item.Id}, Имя: {item.Name}.");    
                prod.ImageUrl = item.ImageUrl;
                prod.Name = item.Name;
                prod.Order = item.Order;
                prod.Price = item.Price;
                prod.BrandId = item.Brand.Id;
                prod.CategoryId = item.Category.Id;

                if (_context.SaveChanges() > 0)
                    logger.LogInformation($"Обновление данных продукта с именем {item.Name} прошло успешно. ИД: {item.Id}");
                else
                    logger.LogError($"При записи данных продукта {item.Name} в БД произошла ошибка.");
            }
            
        }
    }
}
