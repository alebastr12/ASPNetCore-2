using WebStore.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL;
using WebStore.Domain.Entitys;
using WebStore.Domain.Filters;

namespace WebStore.Services.Services
{
    public class SqlProductService:IProductService
    {
        private readonly WebStoreContext _context;

        public SqlProductService(WebStoreContext context)
        {
            _context = context;
        }

        public void AddProduct(Product item)
        {
            if (item is null)
                return;
            _context.Products.Add(item);
            
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Delete(int? id)
        {
            if (id.HasValue)
            {
                var prod = GetProductById(id.Value);
                if (prod is null)
                    return;
                _context.Products.Remove(prod);
                
            }
        }

        public IEnumerable<Brand> GetBrands()
        {
            return _context.Brands.ToList();
        }

        public IEnumerable<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public Product GetProductById(int id)
        {
            return _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Product> GetProducts(ProductFilter filter)
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
            return dbItems.ToList();
        }
    }
}
