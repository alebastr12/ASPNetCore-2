using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entitys;
using WebStore.Domain.EntitysDTO;
using WebStore.Domain.Filters;

namespace WebStore.Interfaces.Services
{
    public interface IProductService
    {
        IEnumerable<CategoryDTO> GetCategories();
        CategoryDTO GetCategoryById(int Id);
        IEnumerable<BrandDTO> GetBrands();
        BrandDTO GetBrandById(int Id);
        PagedProductDTO GetProducts(ProductFilter filter);
        ProductDTO GetProductById(int id);
        void Delete(int? id);
        //void AddProduct(Product item);
        void AddProduct(ProductDTO item);
        void UpdateProduct(ProductDTO item);
        void Commit();
    }
}
