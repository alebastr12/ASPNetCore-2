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
            _context.Products.Add(new Product {
                BrandId=item.Brand.Id,
                CategoryId=item.Category.Id,
                ImageUrl=item.ImageUrl,
                Name=item.Name,
                Order=item.Order,
                Price=item.Price
            });
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
            return _context.Brands.AsEnumerable().Select(b=>new BrandDTO {
                Id=b.Id,
                Name=b.Name,
                Order=b.Order
                
            });
        }

        public IEnumerable<CategoryDTO> GetCategories()
        {
            var cat = _context.Categories
                .Include(category => category.ParentCategory)
                .AsEnumerable()
                .Select(c => new CategoryDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Order = c.Order,
                    ParentCategory = (c.ParentCategory is null) ? null : new CategoryDTO
                    {
                        Id = c.ParentCategory.Id,
                        Name = c.Name,
                        ParentCategory = null
                    }
                });
            return cat;
        }
        public ProductDTO GetProductById(int id)
        {
            return _context.Products
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Select(p=> new ProductDTO {
                    Id=p.Id,
                    ImageUrl=p.ImageUrl,
                    Name=p.Name,
                    Order=p.Order,
                    Price=p.Price,
                    Brand = p.Brand == null ? null: new BrandDTO { Id=p.BrandId, Name=p.Brand.Name, Order=p.Brand.Order},
                    Category = p.Category == null ? null : new CategoryDTO { Id = p.CategoryId, Name=p.Category.Name,Order=p.Category.Order}
                    
                })
                .FirstOrDefault(p => p.Id == id);
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
            return dbItems.AsEnumerable().Select(p => new ProductDTO
            {
                Id = p.Id,
                ImageUrl = p.ImageUrl,
                Name = p.Name,
                Order = p.Order,
                Price = p.Price,
                Brand = p.Brand == null ? null : new BrandDTO { Id = p.BrandId, Name = p.Brand.Name, Order = p.Brand.Order },
                Category = p.Category == null ? null : new CategoryDTO { Id = p.CategoryId, Name = p.Category.Name, Order = p.Category.Order }
            });
        }
    }
}
