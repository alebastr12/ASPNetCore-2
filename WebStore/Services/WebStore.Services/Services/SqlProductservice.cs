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

namespace WebStore.Services.Services
{
    public class SqlProductService:IProductService
    {
        private readonly WebStoreContext _context;

        public SqlProductService(WebStoreContext context)
        {
            _context = context;
        }

        public void AddProduct(ProductDTO item)
        {
            if (item is null)
                return;
            _context.Products.Add(item.FromDTO());
            Commit();
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
            if (id.HasValue)
            {
                var prod = _context.Products.FirstOrDefault(p => p.Id == id);
                if (prod is null)
                    return;
                _context.Products.Remove(prod);
                Commit();
            }
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
            var prod = _context.Products.FirstOrDefault(p => p.Id == item.Id);
            if (prod is null)
                return;
            prod.ImageUrl = item.ImageUrl;
            prod.Name = item.Name;
            prod.Order = item.Order;
            prod.Price = item.Price;
            prod.BrandId = item.Brand.Id;
            prod.CategoryId = item.Category.Id;
            
            Commit();
        }
    }
}
