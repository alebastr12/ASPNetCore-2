using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entitys;
using WebStore.Domain.Filters;

namespace WebStore.Infrastructure.Interfaces
{
    public interface IProductService
    {
        IEnumerable<Category> GetCategories();
        IEnumerable<Brand> GetBrands();
        IEnumerable<Product> GetProducts(ProductFilter filter);
        Product GetProductById(int id);
        void Delete(int? id);
        void AddProduct(Product item);
        void Commit();
    }
}
